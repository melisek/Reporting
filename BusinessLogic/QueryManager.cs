using AutoMapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using szakdoga.Models;
using szakdoga.Models.Dtos.QueryDtos;

namespace szakdoga.BusinessLogic
{
    public class QueryManager
    {
        private readonly IQueryRepository _queryRepository;
        private readonly IConfigurationRoot _cfg;

        public QueryManager(IQueryRepository queryRepository, IConfigurationRoot cfg)
        {
            _queryRepository = queryRepository;
            _cfg = cfg;
        }

        public List<QueryDto> GetAll()
        {
            return Mapper.Map<IEnumerable<QueryDto>>(_queryRepository.GetAll()).ToList();
        }

        public object GetColumnNames(string queryGUID)
        {
            var AllColumns = GetAllColumns(queryGUID);
            return new QueryColumnsDto
            {
                QueryGUID = queryGUID,
                Columns = AllColumns.Columns.Where(x => x.Hidden == false).Select(x =>
                new ColumnNamesDto
                {
                    Name = x.Name,
                    Text = x.Text,
                    Type = x.Type
                }).ToArray()
            };
        }

        public object GetQuerySource(QuerySourceFilterDto filter)
        {
            var query = _queryRepository.Get(filter.QueryGUID);

            if (query == null)
                throw new NotFoundException("Invalid QueryGUID.");

            var AllColumns = GetAllColumns(filter.QueryGUID);

            DataTable data = new DataTable();
            DataTable count = new DataTable();
            string sourceConn = _cfg.GetConnectionString("DefaultConnection");
            using (SqlConnection conn = new SqlConnection(sourceConn))
            {
                SqlCommand cmd = new SqlCommand(GetCommandText(AllColumns, filter, query.ResultTableName), conn);
                conn.Open();
                data.Load(cmd.ExecuteReader());
                SqlCommand countCmd = new SqlCommand(GetCountCommandText(AllColumns, filter, query.ResultTableName), conn);
                count.Load(countCmd.ExecuteReader());
            }

            string countstring = $"\"TotalCount\":{count.Rows[0]["Count"].ToString()}";
            string json = "{" + countstring + ",\"Data\" : [";

            StringBuilder sb = new StringBuilder(json);

            foreach (DataRow row in data.Rows)
            {
                sb.Append("{");
                bool isFirst = true;
                //foreach (Column col in AllColumns.Columns)//a filterből szedjük az oszlopokat, azért itthagyom
                //{
                //    if (isFirst)
                //    {
                //        sb.Append($"\"{col.Name}\" : \"{row[col.Name].ToString()}\"");
                //        isFirst = false;
                //    }
                //    else
                //        sb.Append($",\"{col.Name}\" : \"{row[col.Name].ToString()}\"");
                //}

                foreach (string col in filter.Columns)
                {
                    if (isFirst)
                    {
                        sb.Append($"\"{col}\" : \"{row[col].ToString()}\"");
                        isFirst = false;
                    }
                    else
                        sb.Append($",\"{col}\" : \"{row[col].ToString()}\"");
                }
                sb.Append("},");
            }
            sb[sb.Length - 1] = ']'; //remove last comma
            sb.Append("}");
            return sb.ToString();
        }

        public DataTable GetQuerySourceInDatatable(QuerySourceFilterDto filter, out string columnNames)
        {
            var query = _queryRepository.Get(filter.QueryGUID);

            if (query == null)
                throw new NotFoundException("Invalid QueryGUID.");

            var AllColumns = GetAllColumns(filter.QueryGUID);

            DataTable data = new DataTable();
            string sourceConn = _cfg.GetConnectionString("DefaultConnection");
            using (SqlConnection conn = new SqlConnection(sourceConn))
            {
                SqlCommand cmd = new SqlCommand(GetCommandText(AllColumns, filter, query.ResultTableName), conn);
                conn.Open();
                data.Load(cmd.ExecuteReader());
            }
            columnNames = String.Empty;
            foreach (Column col in AllColumns.Columns.Where(x=>filter.Columns.Contains(x.Name)).ToList())
            {
                if (String.IsNullOrEmpty(columnNames)) columnNames += col.Text;
                else columnNames += ";"+col.Text;
            }
            return data;
        }

        public QueryColumnCountDto GetQueryColumnCount(string queryGUID)
        {
            var AllColumns = GetAllColumns(queryGUID);
            return new QueryColumnCountDto
            {
                QueryGuid = queryGUID,
                ColumnCount = AllColumns.Columns.Where(x => x.Hidden == false).Count()
            };
        }

        private string GetCommandText(AllColumns allColumns, QuerySourceFilterDto filter, string table)
        {
            string columns = String.Empty;
            string where = String.Empty;

            foreach (var col in filter.Columns)
            {
                if (String.IsNullOrEmpty(columns))
                {
                    columns += col;
                    where += col + $" like '%{filter.Filter}%'";
                }
                else
                {
                    columns += ", " + col;
                    where += "or " + col + $" like '%{filter.Filter}%'";
                }
            }

            string order_by = filter.Sort.ColumnName + " " + filter.Sort.Direction.ToString();

            string skip = String.Empty;
            if (filter.Page > 1)
                skip += $" {allColumns.PrimeryKeyColumn} not in (select top {filter.Rows * (filter.Page - 1)} {allColumns.PrimeryKeyColumn} from {table} ) and ";

            string top = String.Empty;
            if (filter.Rows > 0)
                top = $"top { filter.Rows}";
            string cmd = $"select {top} {columns} from {table}  where {skip} ({where}) order by {order_by}";
            return cmd;
        }

        private string GetCountCommandText(AllColumns allColumns, QuerySourceFilterDto filter, string table)
        {
            string where = String.Empty;

            foreach (var col in filter.Columns)
            {
                if (String.IsNullOrEmpty(where))
                {
                    where += col + $" like '%{filter.Filter}%'";
                }
                else
                {
                    where += "or " + col + $" like '%{filter.Filter}%'";
                }
            }
            string cmd = $"select count ({allColumns.PrimeryKeyColumn}) as Count from {table}  where  ({where})";
            return cmd;
        }

        private AllColumns GetAllColumns(string queryGUID)
        {
            return JsonConvert.DeserializeObject<AllColumns>(_queryRepository.Get(queryGUID).TranslatedColumnNames);
        }
    }

    public class Column
    {
        public string Name { get; set; }
        public string Text { get; set; }
        public bool Hidden { get; set; }
        public string Type { get; set; }
    }

    public class AllColumns
    {
        public string PrimeryKeyColumn { get; set; }
        public Column[] Columns { get; set; }
    }
}