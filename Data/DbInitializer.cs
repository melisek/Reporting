using Microsoft.AspNetCore.Builder;

namespace szakdoga.Data
{
    public class DbInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            //AppDbContext context = applicationBuilder.ApplicationServices.GetRequiredService<AppDbContext>();

            //if (!context.User.Any())
            //{
            //    context.AddRange(
            //        new User { Name = "Admin", Password = "admin", EmailAddress = "asd@asd.hu" }
            //        );
            //}
        }
    }
}