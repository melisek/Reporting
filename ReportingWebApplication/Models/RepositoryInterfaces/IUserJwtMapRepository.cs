using System;
using szakdoga.Models.Entities;

namespace szakdoga.Models.RepositoryInterfaces
{
    public interface IUserJwtMapRepository
    {
        void Add(string jwt, User user, DateTime expireTime);

        UserJwtMap GetRecordByJwt(string jwt);

        void Delete(DateTime time);
    }
}