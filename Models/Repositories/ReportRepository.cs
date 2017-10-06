using System.Collections.Generic;
using System.Linq;
using szakdoga.Data;

namespace szakdoga.Models.Repositories
{
    public class ReportRepository : IReportRepository
    {
        public ReportRepository(AppDbContext context)
        {
            _context = context;
        }

        private readonly AppDbContext _context;

        public void Add(Report entity)
        {
            if (entity != null)
            {
                _context.Report.Add(entity);
                _context.SaveChanges();
            }
        }

        public Report Get(int id)
        {
            return _context.Report.SingleOrDefault(x => x.Id == id);
        }

        public IEnumerable<Report> GetAll()
        {
            return _context.Report.ToList();
        }

        public bool Remove(int id)
        {
            var entity = Get(id);
            if (entity != null)
            {
                _context.Report.Remove(entity);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public void Update(Report entity)
        {
            if (entity != null)
            {
                _context.Report.Update(entity);
                _context.SaveChanges();
            }
        }
    }
}