﻿using System.Collections.Generic;

namespace szakdoga.Models
{
    public interface IReportUserRelRepository : IBaseRepositoryInterface<ReportUserRel>
    {
        IEnumerable<ReportUserRel> GetReportUsers(int ReportId);
    }
}