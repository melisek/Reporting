using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
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

                if (!context.User.Any())
                {
                    context.AddRange(
                        new User { Name = "Admin", Password = "admin", EmailAddress = "asd@asd.hu" }
                        );
                }
                if (!context.Dashboards.Any())
                {
                    context.AddRange(
                        new Dashboard { Name = "asd", Style="asdfgasdf" }
                        );
                }
                context.SaveChanges();
            }
        }
    }
}