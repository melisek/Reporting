using System.Collections.Generic;
using System.Linq;
using szakdoga.Data;

namespace szakdoga.Models.Repositories
{
    public class DashboardRepository : IDashboardRepository

    {
        private readonly AppDbContext _context;

        public DashboardRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(Dashboard entity)
        {
            if (entity != null)
            {
                _context.Dashboards.Add(entity);
                _context.SaveChanges();
            }
        }

        public Dashboard Get(int id)
        {
            return _context.Dashboards.SingleOrDefault(x => x.Id == id);
        }

        public IEnumerable<Dashboard> GetAll()
        {
            return _context.Dashboards.ToList();
        }

        public bool Remove(int id)
        {
            var entity = Get(id);
            if (entity != null)
            {
                //_context.Dashboards.Remove(entity);
                entity.Deleted = true;
                _context.Update(entity);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public void Update(Dashboard entity)
        {
            if (entity != null)
            {
                _context.Dashboards.Update(entity);
                _context.SaveChanges();
            }
        }
    }
}