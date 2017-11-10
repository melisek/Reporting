using AutoMapper;
using System;
using szakdoga.Models;
using szakdoga.Models.Dtos.DashboardDto;

namespace szakdoga.BusinessLogic
{
    public class DashboardManager : IDisposable
    {
        private IDashboardRepository _dashboardRepository;

        public DashboardManager(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        public void Dispose()
        {
            _dashboardRepository = null;
        }

        public DashboardDto GetDashBoardStyle(string dashboardGUID)
        {
            var db = _dashboardRepository.Get(dashboardGUID);
            if (db == null)
                return null;
            else
                return Mapper.Map<DashboardDto>(db);

        }
    }
}
