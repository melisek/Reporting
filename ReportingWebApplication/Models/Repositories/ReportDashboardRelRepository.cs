using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using szakdoga.Data;

namespace szakdoga.Models.Repositories
{
    public class ReportDashboardRelRepository : IReportDashboardRelRepository
    {
        public ReportDashboardRelRepository(AppDbContext context)
        {
            _context = context;
        }

        private readonly AppDbContext _context;

        public void Add(ReportDashboardRel entity)
        {
            if (entity != null)
            {
                _context.ReportDashboardRel.Add(entity);
                _context.SaveChanges();
            }
        }

        public ReportDashboardRel Get(int id)
        {
            return _context.ReportDashboardRel.Include(x => x.Report).
                Include(y => y.Dashboard).
                Include(z => z.Report.Query).
                SingleOrDefault(x => x.Id == id);
        }

        public IEnumerable<ReportDashboardRel> GetAll()
        {
            return _context.ReportDashboardRel.Include(x => x.Report).Include(y => y.Dashboard).ToList();
        }

        public bool Remove(int id)
        {
            var entity = Get(id);
            if (entity != null)
            {
                _context.ReportDashboardRel.Remove(entity);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public void Update(ReportDashboardRel entity)
        {
            if (entity != null)
            {
                _context.ReportDashboardRel.Update(entity);
                _context.SaveChanges();
            }
        }

        public IEnumerable<ReportDashboardRel> GetDashboardReports(int dashId)
        {
            return _context.ReportDashboardRel.
                Include(x => x.Report).
                Include(y => y.Dashboard).
                Include(z => z.Report.Query).
                Where(z => z.Dashboard.Id == dashId).ToList();
        }

        public IEnumerable<ReportDashboardRel> GetReportDashboards(int repId)
        {
            return _context.ReportDashboardRel.
                Include(x => x.Report).
                Include(y => y.Dashboard).
                Include(z => z.Report.Query).
                Where(z => z.Report.Id == repId).ToList();
        }
    }
}