using AutoMapper;
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
    }
}
