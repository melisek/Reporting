using System.Collections.Generic;
using System.Linq;
using szakdoga.Data;

namespace szakdoga.Models.Repositories
{
    public class QueryRepository : IQueryRepository
    {
        private readonly AppDbContext _context;

        public QueryRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(Query entity)
        {
            if (entity != null)
            {
                _context.Query.Add(entity);
                _context.SaveChanges();
            }
        }

        public Query Get(int id)
        {
            return _context.Query.SingleOrDefault(x => x.Id == id);
        }

        public Query Get(string queryGUID)
        {
            return _context.Query.SingleOrDefault(x => x.QueryGUID == queryGUID);
        }

        public IEnumerable<Query> GetAll()
        {
            return _context.Query.ToList();
        }

        public bool Remove(int id)
        {
            var entity = Get(id);
            if (entity != null)
            {
                //_context.Query.Remove(entity);
                entity.Deleted = true;
                _context.Update(entity);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public void Update(Query entity)
        {
            if (entity != null)
            {
                _context.Query.Update(entity);
                _context.SaveChanges();
            }
        }
    }
}