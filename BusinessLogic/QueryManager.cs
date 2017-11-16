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
                return null;

            var AllColumns = GetAllColumns(filter.QueryGUID);

            DataTable data = new DataTable();

            string sourceConn = _cfg.GetConnectionString("DefaultConnection");
            using (SqlConnection conn = new SqlConnection(sourceConn))
            {
                SqlCommand cmd = new SqlCommand(GetCommandText(AllColumns, filter.X, filter.Y, query.ResultTableName), conn);
                conn.Open();
                data.Load(cmd.ExecuteReader());
            }

            string json = "{\"Data\" : [";

            StringBuilder sb = new StringBuilder(json);

            foreach (DataRow row in data.Rows)
            {
                sb.Append("{");
                bool isFirst = true;
                foreach (Column col in AllColumns.Columns)
                {
                    if (isFirst)
                    {
                        sb.Append($"\"{col.Name}\" : \"{row[col.Name].ToString()}\"");
                        isFirst = false;
                    }
                    else
                        sb.Append($",\"{col.Name}\" : \"{row[col.Name].ToString()}\"");
                }
                sb.Append("},");
            }
            sb[sb.Length - 1] = ']'; //remove last comma
            sb.Append("}");
            return sb.ToString();
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

        private string GetCommandText(AllColumns allColumns, int x, int y, string table)
        {
            string columns = String.Empty;

            foreach (var col in allColumns.Columns)
            {
                if (String.IsNullOrEmpty(columns)) columns += col.Name;
                else columns += ", " + col.Name;
            }

            string skip = String.Empty;
            if (y > 1)
                skip += $"where {allColumns.PrimeryKeyColumn} not in (select top {x * (y - 1)} {allColumns.PrimeryKeyColumn} from {table} )";

            string cmd = $"select top {x} {columns} from {table} {skip} order by {allColumns.PrimeryKeyColumn}";
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