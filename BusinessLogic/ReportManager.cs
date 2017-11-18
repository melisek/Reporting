using AutoMapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using szakdoga.Models;
using szakdoga.Models.Dtos;
using szakdoga.Models.Dtos.ReportDtos;

namespace szakdoga.BusinessLogic
{
    public class ReportManager
    {
        private readonly IReportRepository _reportRepository;
        private readonly IReportDashboardRelRepository _reportDashboardRel;
        private readonly QueryManager _queryManager;
        private readonly IConfigurationRoot _cfg;

        public ReportManager(IReportRepository reportRepository, IReportDashboardRelRepository repDashRel, QueryManager queryman, IConfigurationRoot cfg)
        {
            _reportRepository = reportRepository;
            _reportDashboardRel = repDashRel;
            _queryManager = queryman;
            _cfg = cfg;
        }

        public ReportDto GetReportStyle(string reportGUID)
        {
            var report = _reportRepository.GetAll().FirstOrDefault(x => x.ReportGUID == reportGUID);

            if (report == null)
                throw new NotFoundException("Invalid reportGUID.");

            return Mapper.Map<ReportDto>(report);
        }

        public GetReportDto GetReport(string reportGUID)
        {
            var report = _reportRepository.Get(reportGUID);
            if (report == null) throw new NotFoundException("Query not found.");
            return new GetReportDto
            {
                Name = report.Name,
                QueryGUID = report.Query.QueryGUID,
                Columns = StringArrayDeserializer(report.Columns),
                Filter = report.Filter,
                Rows = report.Rows,
                Sort = String.IsNullOrEmpty(report.Sort) ? null : JsonConvert.DeserializeObject<SortDto>(report.Sort)
            };
        }

        public string CreateReport(CreateReportDto report)
        {
            var dbReport = new Report
            {
                Name = report.Name,
                ReportGUID = CreateGUID.GetGUID(),
                Query = _reportRepository.GetQuery(report.QueryGUID),
                Columns = StringArraySerializer(report.Columns),
                Filter = report.Filter,
                Sort = JsonConvert.SerializeObject(report.Sort, Formatting.None),
                Rows = report.Rows
            };
            _reportRepository.Add(dbReport);

            return dbReport.ReportGUID;
        }

        private string StringArraySerializer(string[] array)
        {
            string result = String.Empty;

            if (array.Length == 0)
                return result;
            result += array[0];

            for (int i = 1; i < array.Length; i++)
            {
                result += ":" + array[i];
            }

            return result;
        }

        private string[] StringArrayDeserializer(string columns)
        {
            if (String.IsNullOrEmpty(columns))
                return null;

            string[] result = columns.Split(':');
            return result;
        }

        public bool UpdateReport(UpdateReportDto report, string reportGUID)
        {
            var origReport = _reportRepository.Get(reportGUID);
            if (origReport == null)
                throw new NotFoundException("Invalid reportGUID.");

            origReport.Name = report.Name;
            origReport.ReportGUID = report.ReportGUID;
            origReport.Query = _reportRepository.GetQuery(report.QueryGUID);
            origReport.Columns = StringArraySerializer(report.Columns);
            origReport.Filter = report.Filter;
            origReport.Sort = JsonConvert.SerializeObject(report.Sort, Formatting.None);
            origReport.Rows = report.Rows;

            _reportRepository.Update(origReport);
            return true;
        }

        public bool DeleteReport(string reportGUID)
        {
            foreach (var rel in _reportDashboardRel.GetReportDashboards(_reportRepository.Get(reportGUID).Id))
            {
                _reportDashboardRel.Remove(rel.Id);
            }
            return _reportRepository.Remove(reportGUID);
        }

        public AllReportDto GetAllReport(GetAllFilterDto filter)
        {
            IEnumerable<Report> reports = _reportRepository.GetAll()
                             .Where(x => (String.IsNullOrEmpty(filter.Filter) || x.Name.Contains(filter.Filter) || x.LastModifier.Name.Contains(filter.Filter)
                             || x.Query.Name.Contains(filter.Filter))).ToList();

            int count = reports.Count();

            if (filter.Sort.Direction == Direction.Asc)
                reports = reports
                     .OrderBy(z => typeof(Report).GetProperty(filter.Sort.ColumnName).GetValue(z, null))
                     .Skip(filter.Page > 1 ? (filter.Page - 1) * filter.Rows : 0)
                     .Take(filter.Rows).ToList();
            else
                reports = reports
                 .OrderByDescending(z => typeof(Report).GetProperty(filter.Sort.ColumnName).GetValue(z, null))
                 .Skip(filter.Page > 1 ? (filter.Page - 1) * filter.Rows : 0)
                 .Take(filter.Rows).ToList();



            return new AllReportDto
            {
                Reports = Mapper.Map<IEnumerable<ReportForAllDto>>(reports).ToList(),
                TotalCount = count
            };
        }

        public object GetQuerySource(ReportSourceFilterDto filter)
        {
            var report = _reportRepository.Get(filter.ReportGUID);
            return _queryManager.GetQuerySource(
                new Models.Dtos.QueryDtos.QuerySourceFilterDto
                {
                    QueryGUID = report.Query.QueryGUID,
                    Rows = filter.X,
                    Page = filter.Y,
                    Columns = report.Columns.Split(':'),
                    Filter = report.Filter,
                    Sort = JsonConvert.DeserializeObject<SortDto>(report.Sort)
                });
        }

        public void UpdateStyle(UpdateReportStyle report)
        {
            var origReport = _reportRepository.Get(report.ReportGUID);
            if (origReport == null)
                throw new NotFoundException("Invalid reportGUID.");

            origReport.Style = report.Style;

            _reportRepository.Update(origReport);
        }

        public object GetDiscreetRiportDiagram(ReportDiagramDiscDto diagram)
        {
            Report report = _reportRepository.Get(diagram.ReportGUID);
            if (report == null) throw new NotFoundException("Report not found by GUID.");

            DataTable data = new DataTable();
            string sql = $"select {diagram.NameColumn}, {diagram.Aggregation.ToString()} ({diagram.ValueColumn}) as Value from {report.Query.ResultTableName} group by {diagram.NameColumn}";
            string sourceConn = _cfg.GetConnectionString("DefaultConnection");
            using (SqlConnection conn = new SqlConnection(sourceConn))
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();
                data.Load(cmd.ExecuteReader());
            }

            List<DiscreetValueDto> result = new List<DiscreetValueDto>();
            foreach (DataRow row in data.Rows)
            {
                result.Add(new DiscreetValueDto
                {
                    Name = row[diagram.NameColumn].ToString(),
                    Value = double.Parse(row["Value"].ToString())
                });
            }
            return result.ToArray();
        }

        public object GetSeriesRiportDiagram(ReportDiagramSerDto diagram)
        {
            Report report = _reportRepository.Get(diagram.ReportGUID);
            if (report == null) throw new NotFoundException("Report not found by GUID.");

            DataTable data = new DataTable();
            string sql = $"select {diagram.NameColumn}, {diagram.SeriesNameColumn}, {diagram.ValueColumn} from {report.Query.ResultTableName}";
            string sourceConn = _cfg.GetConnectionString("DefaultConnection");
            using (SqlConnection conn = new SqlConnection(sourceConn))
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();
                data.Load(cmd.ExecuteReader());
            }

            List<SeriesDto> result = new List<SeriesDto>();
            string prevName = string.Empty;
            List<SeriesValueDto> values = new List<SeriesValueDto>();
            foreach (DataRow row in data.Rows)
            {
                if (prevName != row[diagram.NameColumn].ToString() && !String.IsNullOrEmpty(prevName))
                {
                    result.Add(new SeriesDto
                    {
                        Name = prevName,
                        Series = values.ToArray()
                    });
                    values.Clear();
                }
                prevName = row[diagram.NameColumn].ToString();
                values.Add(new SeriesValueDto
                {
                    Name = row[diagram.SeriesNameColumn].ToString(),
                    Value = double.Parse(row[diagram.ValueColumn].ToString())
                });
            }
            //add last ones
            result.Add(new SeriesDto
            {
                Name = prevName,
                Series = values.ToArray()
            });
            return result.ToArray();
        }
    }
}
