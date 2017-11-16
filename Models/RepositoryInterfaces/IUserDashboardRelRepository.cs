using System.Collections.Generic;

namespace szakdoga.Models
{
    public interface IUserDashboardRelRepository : IBaseRepositoryInterface<UserDashboardRel>
    {
        IEnumerable<UserDashboardRel> GetDashboardUsers(int dashboardId);

        UserDashboardRel Get(int dashboardId, int userId);
    }
}