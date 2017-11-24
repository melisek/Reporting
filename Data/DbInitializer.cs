using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace szakdoga.Data
{
    public class DbInitializer
    {
        private AppDbContext _context;
        private IConfigurationRoot _cfg;
        private readonly ILogger<DbInitializer> _logger;

        public DbInitializer(AppDbContext context, IConfigurationRoot cfg, ILogger<DbInitializer> logger)
        {
            _context = context;
            _cfg = cfg;
            _logger = logger;
        }

        public void Seed()
        {
            CleanAllTables(_context);

            _context.SaveChanges();

            string sourceConn = _cfg.GetConnectionString("DefaultConnection");
            using (SqlConnection conn = new SqlConnection(sourceConn))
            {
                ExecuteBatchNonQuery(conn);
            }
        }


        private void CleanAllTables(AppDbContext context)
        {
            context.ReportDashboardRel.RemoveRange(context.ReportDashboardRel.ToList());
            context.ReportUserRel.RemoveRange(context.ReportUserRel.ToList());
            context.UserDashboardRel.RemoveRange(context.UserDashboardRel.ToList());
            context.Dashboards.RemoveRange(context.Dashboards.ToList());
            context.Report.RemoveRange(context.Report.ToList());
            context.UserJwtMap.RemoveRange(context.UserJwtMap.ToList());
            context.User.RemoveRange(context.User.ToList());
            context.Query.RemoveRange(context.Query.ToList());
            context.SaveChanges();
        }

        private void ExecuteBatchNonQuery(SqlConnection conn)
        {
            string sqlBatch = string.Empty;
            var lines = File.ReadAllLines(@"Data\SeedSQL.txt");
            SqlCommand cmd = new SqlCommand(string.Empty, conn);
            conn.Open();
            try
            {
                foreach (string line in lines)
                {
                    if (line.ToUpperInvariant().Trim() == "GO")
                    {
                        cmd.CommandText = sqlBatch;
                        cmd.ExecuteNonQuery();
                        sqlBatch = string.Empty;
                    }
                    else
                    {
                        sqlBatch += line + "\n";
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
    }
}