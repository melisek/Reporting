using AutoMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using szakdoga.Models;
using szakdoga.Models.Dtos.QueryDtos;

namespace szakdoga.BusinessLogic
{
    public class QueryManager : IDisposable
    {
        private IQueryRepository _queryRepository;
        public QueryManager(IQueryRepository queryRepository)
        {
            _queryRepository = queryRepository;
        }

        public void Dispose()
        {
            _queryRepository = null;
        }

        public List<QueryDto> GetAll()
        {
            return Mapper.Map<IEnumerable<QueryDto>>(_queryRepository.GetAll()).ToList();
        }

        public object GetColumnNames(string queryGUID)
        {
            var AllColumns = JsonConvert.DeserializeObject<AllColumns>(_queryRepository.Get(queryGUID).TranslatedColumnNames);
            return new QueryColumnsDto
            {
                QueryGUID = queryGUID,
                Columns = AllColumns.Columns.Where(x => x.Hidden == true).Select(x =>
                new ColumnNamesDto
                {
                    Name = x.Name,
                    Text = x.Text,
                    Type = x.Type
                }).ToArray()
            };
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
        public Column[] Columns { get; set; }
    }
}
