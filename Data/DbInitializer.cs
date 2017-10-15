using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using szakdoga.Models;

namespace szakdoga.Data
{
    public class DbInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            //TODO: megnézni h ezzel mért nem működik //AppDbContext context = applicationBuilder.ApplicationServices.GetRequiredService<AppDbContext>();
            using (var serviceScope = applicationBuilder.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {
                AppDbContext context = serviceScope.ServiceProvider.GetService<AppDbContext>();

                CleanAllTables(context);

                if (!context.User.Any())
                    context.User.AddRange(Users);
                if (!context.Query.Any())
                    context.Query.AddRange(Query1);

                context.SaveChanges();
            }
        }

        static List<User> Users
        {
            get
            {
                return new List<User>
                {
                    new User{ Name="Admin", Password="admin", EmailAddress="asd@asd.com", GUID="123"},
                    new User{ Name="Teszt", Password="teszt",EmailAddress="teszt@teszt.com",GUID="234"}
                };
            }
        }

        static Query Query1 { get => new Query { SQL = "select * from CustomerStockOut", Name = "Számlák", GUID = "456", NextUpdating = System.DateTime.Now, UpdatePeriod = new TimeSpan(1, 0, 0, 0) }; }



        private static void CleanAllTables(AppDbContext context)
        {
            context.ReportDashboardRel.RemoveRange(context.ReportDashboardRel.ToList());
            context.ReportUserRel.RemoveRange(context.ReportUserRel.ToList());
            context.UserDashboardRel.RemoveRange(context.UserDashboardRel.ToList());
            context.Dashboards.RemoveRange(context.Dashboards.ToList());
            context.Report.RemoveRange(context.Report.ToList());
            context.User.RemoveRange(context.User.ToList());
            context.Query.RemoveRange(context.Query.ToList());
        }
    }
}