using System;
using System.Collections.Generic;
using System.Linq;
using szakdoga.Data;
using szakdoga.Models.Entities;
using szakdoga.Models.RepositoryInterfaces;

namespace szakdoga.Models.Repositories
{
    public class UserJwtMapRepository : IUserJwtMapRepository
    {
        private readonly AppDbContext _context;

        public UserJwtMapRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(string jwt, User user, DateTime expireTime)
        {
            List<UserJwtMap> mapRecords = _context.UserJwtMap.Where(record => record.Jwt == jwt).ToList();
            foreach (UserJwtMap record in mapRecords)
            {
                _context.UserJwtMap.Remove(record);
                _context.UserJwtMap.Remove(record);
            }

            UserJwtMap userToken = new UserJwtMap
            {
                Jwt = jwt,
                User = user,
                ExpireTime = expireTime
            };
            _context.Add(userToken);
            _context.SaveChanges();
        }

        public UserJwtMap GetRecordByJwt(string jwt)
        {
            return _context.UserJwtMap.FirstOrDefault(record => record.Jwt == jwt);
        }

        public void Delete(DateTime time)
        {
            foreach (UserJwtMap record in _context.UserJwtMap.Where(record => record.ExpireTime < time).ToList())
            {
                _context.UserJwtMap.Remove(record);
            }
            _context.SaveChanges();
        }
    }
}