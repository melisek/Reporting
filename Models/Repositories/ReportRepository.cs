using Microsoft.EntityFrameworkCore;
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
                entity.ModifyDate = System.DateTime.Now;
                _context.Report.Add(entity);
                _context.SaveChanges();
            }
        }

        public Report Get(int id)
        {
            return _context.Report.Include(y => y.Query).SingleOrDefault(x => x.Id == id);
        }

        public IEnumerable<Report> GetAll()
        {
            return _context.Report.Include(x => x.LastModifier).
                Include(y => y.Author).
                Include(s => s.LastModifier).
                Include(z => z.Query).ToList();
        }

        public bool Remove(int id)
        {
            var entity = Get(id);
            if (entity != null)
            {
                entity.Deleted = true;
                _context.Update(entity);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public void Update(Report entity)
        {
            if (entity != null)
            {
                var origReport = Get(entity.ReportGUID);
                origReport.Columns = entity.Columns;
                origReport.Filter = entity.Filter;
                origReport.Name = entity.Name;
                origReport.Query = entity.Query;
                origReport.Sort = entity.Sort;
                origReport.Rows = entity.Rows;
                //origReport.Style = origReport.Style;--külön szerkesztjük--patch kéne rá és külön fv?!
                origReport.ModifyDate = System.DateTime.Now;

                _context.Report.Update(origReport);
                _context.SaveChanges();
            }
        }

        public Report Get(string reportGUID)
        {
            return _context.Report.Include(y => y.Query).SingleOrDefault(x => x.ReportGUID == reportGUID);
        }

        public Query GetQuery(string QueryGUID)
        {
            return _context.Query.SingleOrDefault(x => x.QueryGUID == QueryGUID);
        }

        public bool Remove(string ReportGUID)
        {
            var entity = Get(ReportGUID);
            if (entity != null)
            {
                entity.Deleted = true;
                _context.Update(entity);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}