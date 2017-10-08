using Microsoft.EntityFrameworkCore;
using szakdoga.Models;

namespace szakdoga.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }

        /// <summary>
        /// https://docs.microsoft.com/en-us/ef/core/modeling/relational/default-values
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Report>().Property(b => b.CreationDate).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<Report>().Property(b => b.ModifyDate).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<Dashboard>().Property(b => b.CreationDate).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<Dashboard>().Property(b => b.ModifyDate).HasDefaultValueSql("getdate()");
        }

        public DbSet<Dashboard> Dashboards { get; set; }
        public DbSet<Query> Query { get; set; }
        public DbSet<ReportDashboardRel> ReportDashboardRel { get; set; }
        public DbSet<Report> Report { get; set; }
        public DbSet<ReportUserRel> ReportUserRel { get; set; }
        public DbSet<User> User { get; set; }

        public DbSet<UserDashboardRel> UserDashboardRel { get; set; }
    }
}