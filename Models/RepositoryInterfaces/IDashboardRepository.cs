using System.Collections.Generic;

namespace szakdoga.Models
{
    public interface IDashboardRepository
    {
        IEnumerable<Dashboard> Dashboards { get; }
        Dashboard GetDashBoardById(int dashboardId);
    }
    public interface IReportDashboardRepository
    {
        IEnumerable<ReportDashboardRel> ReportDashboardRels { get; }
        ReportDashboardRel GetReportDashboardRelById(int ReportDashboardRelId);
    }
    public interface IReportRepository
    {
        IEnumerable<Report> Reports { get; }
        Report GetReportById(int reportId);
    }
    public interface IReportUserRelRepository
    {
        IEnumerable<ReportUserRel> ReportUserRels { get; }
        ReportUserRel GetReportUserRelById(int reportUserRelId);
    }
    public interface IUserRepository
    {
        IEnumerable<User> Users { get; }
        User GetUserById(int UserId);
    }
}
