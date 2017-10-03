using Microsoft.EntityFrameworkCore;
using szakdoga.Models;

namespace szakdoga.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }

        public DbSet<Dashboard> Dashboards { get; set; }
        public DbSet<Query> Query { get; set; }
        public DbSet<ReportDashboardRel> RiporDashboardRel { get; set; }
        public DbSet<Report> Riport { get; set; }
        public DbSet<ReportUserRel> RiportUserRel { get; set; }
        public DbSet<User> User { get; set; }
    }
}