using System.Collections.Generic;
using szakdoga.Models;
using szakdoga.Models.Dtos.RelDtos;
using szakdoga.Models.Dtos.RelDtos.RepUserDtos;

namespace szakdoga.BusinessLogic
{
    public class ReportUserRelManager
    {
        private readonly IUserRepository _userRepository;
        private readonly IReportUserRelRepository _reportUserRelRepository;
        private readonly IReportRepository _reportRepository;

        public ReportUserRelManager(IUserRepository userRepository, IReportUserRelRepository reportUserRelRepository, IReportRepository reportRepository)
        {
            _userRepository = userRepository;
            _reportUserRelRepository = reportUserRelRepository;
            _reportRepository = reportRepository;
        }

        public object GetReportUsers(string reportGUID)
        {
            var report = _reportRepository.Get(reportGUID);
            if (report == null)
                return null;

            List<ReportUserDto> users = new List<ReportUserDto>();
            foreach (var rel in _reportUserRelRepository.GetReportUsers(report.Id))
            {
                users.Add(new ReportUserDto
                {
                    UserGUID = rel.User.UserGUID,
                    Name = rel.User.Name,
                    Permission = (RepotUserPermissions)rel.AuthoryLayer
                });
            }

            return new ReportUsersDto
            {
                ReportGUID = reportGUID,
                Users = users
            };
        }
    }
}

