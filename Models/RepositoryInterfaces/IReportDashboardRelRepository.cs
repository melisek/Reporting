using System.Collections.Generic;

namespace szakdoga.Models
{
    public interface IReportDashboardRelRepository : IBaseRepositoryInterface<ReportDashboardRel>
    {
        IEnumerable<ReportDashboardRel> GetDashboardReports(int dashId);
    }
}