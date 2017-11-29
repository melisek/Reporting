using System;
using szakdoga.Models.Entities;

namespace szakdoga.Models.RepositoryInterfaces
{
    public interface IUserJwtMapRepository
    {
        void AddUserJwtMapRecord(string jwt, User user, DateTime expireTime);

        UserJwtMap GetRecordByJwt(string jwt);

        void RemoveRecordBefore(DateTime time);
    }
}