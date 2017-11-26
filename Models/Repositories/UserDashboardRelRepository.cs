using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using szakdoga.Data;

namespace szakdoga.Models.Repositories
{
    public class UserDashboardRelRepository : IUserDashboardRelRepository
    {
        private readonly AppDbContext _context;

        public UserDashboardRelRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(UserDashboardRel entity)
        {
            if (entity != null)
            {
                _context.UserDashboardRel.Add(entity);
                _context.SaveChanges();
            }
        }

        public UserDashboardRel Get(int id)
        {
            return _context.UserDashboardRel.SingleOrDefault(x => x.Id == id);
        }

        public UserDashboardRel Get(int dashboardId, int userId)
        {
            return _context.UserDashboardRel.Include(x => x.User).Include(y => y.Dashboard).FirstOrDefault(z => z.Dashboard.Id == dashboardId && z.User.Id == userId);
        }

        public IEnumerable<UserDashboardRel> GetAll()
        {
            return _context.UserDashboardRel.Include(x => x.User).Include(y => y.Dashboard).ToList();
        }

        public IEnumerable<UserDashboardRel> GetDashboardUsers(int dashboardID)
        {
            return _context.UserDashboardRel.Include(x => x.User).Include(y => y.Dashboard).Where(z => z.Dashboard.Id == dashboardID).ToList();
        }

        public bool Remove(int id)
        {
            var entity = Get(id);
            if (entity != null)
            {
                _context.UserDashboardRel.Remove(entity);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public void Update(UserDashboardRel entity)
        {
            if (entity != null)
            {
                _context.UserDashboardRel.Update(entity);
                _context.SaveChanges();
            }
        }
    }
}