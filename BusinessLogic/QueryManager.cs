using AutoMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using szakdoga.Models;

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
            return new
            {
                QueryGUID = queryGUID,
                Columns = AllColumns.Columns.Where(x => x.hidden == true).Select(x => x.text).ToArray()
            };
        }
    }
    public class Column
    {
        public string name { get; set; }
        public string text { get; set; }
        public bool hidden { get; set; }
    }
    public class AllColumns
    {
        public Column[] Columns { get; set; }
    }
}
