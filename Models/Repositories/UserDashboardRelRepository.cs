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

        public IEnumerable<UserDashboardRel> GetAll()
        {
            return _context.UserDashboardRel.ToList();
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
