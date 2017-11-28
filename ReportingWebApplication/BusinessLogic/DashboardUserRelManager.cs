using System.Collections.Generic;
using szakdoga.Models;
using szakdoga.Models.Dtos.RelDtos.DashboardUserRelDtos;

namespace szakdoga.BusinessLogic
{
    public class DashboardUserRelManager
    {
        private IUserRepository _userRepository;
        private IDashboardRepository _dashboardRepository;
        private IUserDashboardRelRepository _userDashboardRelRepository;

        public DashboardUserRelManager(IUserRepository userRepository, IDashboardRepository dashboardRepository, IUserDashboardRelRepository userDashboardRelRepository)
        {
            _userRepository = userRepository;
            _dashboardRepository = dashboardRepository;
            _userDashboardRelRepository = userDashboardRelRepository;
        }

        public object GetDashboardUsers(string DashboardGUID)
        {
            var Dashboard = _dashboardRepository.Get(DashboardGUID);
            if (Dashboard == null)
                throw new NotFoundException("Invalid DashboardGUID.");

            List<DashboardUserDto> users = new List<DashboardUserDto>();
            foreach (var rel in _userDashboardRelRepository.GetDashboardUsers(Dashboard.Id))
            {
                users.Add(new DashboardUserDto
                {
                    UserGUID = rel.User.UserGUID,
                    Name = rel.User.Name,
                    Permission = (DashboardUserPermissions)rel.AuthoryLayer
                });
            }

            return new DashboardUsersDto
            {
                DashboardGUID = DashboardGUID,
                Users = users
            };
        }

        public bool Create(CreateDashboardUserDto DashboardUserRel)
        {
            if (!IsExistUserAndDashboard(out User user, out Dashboard Dashboard, DashboardUserRel.UserGUID, DashboardUserRel.DashboardGUID))
                return false;

            if (IsExistRel(user.Id, Dashboard.Id) != null)//létezik a jogosultság
                return false;

            _userDashboardRelRepository.Add(new UserDashboardRel { Dashboard = Dashboard, User = user, AuthoryLayer = DashboardUserRel.Permission });
            return true;
        }

        public bool DeleteDashboardUserRel(DeleteDashboardUserDto DashboardUserRel)
        {
            if (!IsExistUserAndDashboard(out User user, out Dashboard Dashboard, DashboardUserRel.UserGUID, DashboardUserRel.DashboardGUID))
                return false;

            var origRel = IsExistRel(user.Id, Dashboard.Id);

            if (origRel == null)
                throw new NotFoundException("There is no relation record with this data.");

            return _userDashboardRelRepository.Remove(origRel.Id);
        }

        public bool UpdateDashboardUserRel(UpdateDashboardUserDto DashboardUserRel)
        {
            if (!IsExistUserAndDashboard(out User user, out Dashboard Dashboard, DashboardUserRel.UserGUID, DashboardUserRel.DashboardGUID))
                return false;

            var origRel = IsExistRel(user.Id, Dashboard.Id);

            if (origRel == null)
                throw new NotFoundException("There is no relation record with this data.");

            origRel.AuthoryLayer = DashboardUserRel.Permission;
            _userDashboardRelRepository.Update(origRel);
            return true;
        }

        private bool IsExistUserAndDashboard(out User user, out Dashboard dashboard, string userGUID, string dashboardGUID)
        {
            dashboard = _dashboardRepository.Get(dashboardGUID);
            user = _userRepository.Get(userGUID);

            if (user == null)
                throw new NotFoundException("Invalid userGUID.");
            if (dashboard == null)
                throw new NotFoundException("Invalid dashboardGUID");

            return true;
        }

        private UserDashboardRel IsExistRel(int userId, int dashboardID)
        {
            return _userDashboardRelRepository.Get(dashboardID, userId);
        }
    }
}