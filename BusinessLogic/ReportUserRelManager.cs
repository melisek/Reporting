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

        public bool Create(CreateReportUserDto reportUserRel)
        {
            if (!IsExistUserAndReport(out User user, out Report report, reportUserRel.UserGUID, reportUserRel.ReportGUID))
                return false;

            if (IsExistRel(user.Id, report.Id) != null)//létezik a jogosultság
                return false;

            _reportUserRelRepository.Add(new ReportUserRel { Report = report, User = user, AuthoryLayer = reportUserRel.Permission });
            return true;
        }

        public bool DelteReportUserRel(DeleteReportUserDto reportUserRel)
        {
            if (!IsExistUserAndReport(out User user, out Report report, reportUserRel.UserGUID, reportUserRel.ReportGUID))
                return false;

            var origRel = IsExistRel(user.Id, report.Id);

            if (origRel == null)
                return false;

            return _reportUserRelRepository.Remove(origRel.Id);
        }

        public bool UpdateReportUserRel(UpdateReportUserDto reportUserRel)
        {
            if (!IsExistUserAndReport(out User user, out Report report, reportUserRel.UserGUID, reportUserRel.ReportGUID))
                return false;

            var origRel = IsExistRel(user.Id, report.Id);

            if (origRel == null)
                return false;

            origRel.AuthoryLayer = reportUserRel.Permission;
            _reportUserRelRepository.Update(origRel);
            return true;
        }

        private bool IsExistUserAndReport(out User user, out Report report, string userGUID, string reportGUID)
        {
            report = _reportRepository.Get(reportGUID);
            user = _userRepository.Get(userGUID);

            if (user == null || report == null)
                return false;
            else return true;
        }

        private ReportUserRel IsExistRel(int userId, int reportID)
        {
            return _reportUserRelRepository.Get(reportID, userId);
        }
    }
}

