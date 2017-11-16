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

        public void AddUserJwtMapRecord(string jwt, User user, DateTime expireTime)
        {
            //delete all records with same jwt(probably will not happen, just in case)
            List<UserJwtMap> mapRecords = _context.UserJwtMap.Where(record => record.Jwt == jwt).ToList();
            foreach (UserJwtMap record in mapRecords)
            {
                _context.UserJwtMap.Remove(record);
                _context.UserJwtMap.Remove(record);
            }

            // add the new record
            UserJwtMap newTask = new UserJwtMap
            {
                Jwt = jwt,
                User = user,
                ExpireTime = expireTime
            };
            _context.Add(newTask);
            _context.SaveChanges();
        }

        public UserJwtMap GetRecordByJwt(string jwt)
        {
            return _context.UserJwtMap.FirstOrDefault(record => record.Jwt == jwt);
        }

        public void RemoveRecordBefore(DateTime time)
        {
            List<UserJwtMap> listToRemove = _context.UserJwtMap.Where(record => record.ExpireTime < time).ToList();
            foreach (UserJwtMap record in listToRemove)
            {
                _context.UserJwtMap.Remove(record);
            }
            _context.SaveChanges();
        }
    }
}