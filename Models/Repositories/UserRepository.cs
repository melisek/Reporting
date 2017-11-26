using System.Collections.Generic;
using System.Linq;
using szakdoga.Data;

namespace szakdoga.Models.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(User entity)
        {
            if (entity != null)
            {
                _context.User.Add(entity);
                _context.SaveChanges();
            }
        }

        public User Get(int id)
        {
            return _context.User.SingleOrDefault(x => x.Id == id);
        }

        public User Get(string userGUID)
        {
            return _context.User.SingleOrDefault(x => x.UserGUID == userGUID);
        }

        public IEnumerable<User> GetAll()
        {
            return _context.User.ToList();
        }

        public User GetByEmailAdd(string emailAdd)
        {
            return _context.User.SingleOrDefault(x => x.EmailAddress.Equals(emailAdd));
        }

        public bool Remove(int id)
        {
            var entity = Get(id);
            if (entity != null)
            {
                //_context.User.Remove(entity);
                entity.Deleted = true;
                _context.Update(entity);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public void Update(User entity)
        {
            if (entity != null)
            {
                _context.User.Update(entity);
                _context.SaveChanges();
            }
        }
    }
}