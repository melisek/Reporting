namespace szakdoga
{
    public enum DashboardUserPermissions
    {
        Invalid = 0,
        CanWatch = 1,
        CanModify = 2
    }

    public enum ReportUserPermissions
    {
        Invalid = 0,
        CanWatch = 1,
        CanModify = 2
    }

    public enum Direction
    {
        Asc = 0,
        Desc = 1
    }

    public enum Aggregation
    {
        SUM = 0,
        AVG = 1,
        MIN = 2,
        MAX = 3,
        COUNT = 4
    }
}