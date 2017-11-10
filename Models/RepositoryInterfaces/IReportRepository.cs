namespace szakdoga.Models
{
    public interface IReportRepository : IBaseRepositoryInterface<Report>
    {
        Report Get(string GUID);

        Query GetQuery(string QueryGUID);

        bool Remove(string ReportGUID);
    }
}