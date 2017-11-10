namespace szakdoga.Models
{
    public interface IQueryRepository : IBaseRepositoryInterface<Query>
    {
        Query Get(string GUID);
    }
}