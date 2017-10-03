using System.Collections.Generic;

namespace szakdoga.Models
{
    public interface IQueryRepository
    {
        IEnumerable<Query> Queries { get; }
        Query GetQueryById(int QueryId);
    }
}
