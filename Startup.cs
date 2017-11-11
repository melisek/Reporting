using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using szakdoga.Data;
using szakdoga.Models;
using szakdoga.Models.Dtos.DashboardDtos;
using szakdoga.Models.Repositories;

namespace szakdoga
{
    public class Startup
    {
        private IConfigurationRoot _configurationRoot;

        public Startup(IHostingEnvironment hostingEnvironment)
        {
            _configurationRoot = new ConfigurationBuilder()
                          .SetBasePath(hostingEnvironment.ContentRootPath)
                          .AddJsonFile("appsettings.json")
                          .Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(_configurationRoot.GetConnectionString("DefaultConnection")));

            services.AddScoped<IQueryRepository, QueryRepository>();
            services.AddScoped<IReportRepository, ReportRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IDashboardRepository, DashboardRepository>();
            services.AddScoped<IReportDashboardRelRepository, ReportDashboardRelRepository>();
            services.AddScoped<IReportUserRelRepository, ReportUserRelRepository>();
            services.AddScoped<IUserDashboardRelRepository, UserDashboardRelRepository>();


            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
                 {
                     routes.MapRoute(
                         name: "default",
                        template: "{controller=Home}/{action=Index}/{id?}");

                     routes.MapSpaFallbackRoute(
                         name: "spa-fallback",
                         defaults: new { controller = "Home", action = "Index" });
                 });
            //új db-migration elõtt kikapcsolni, mivel futtatásnál már próbál beírni a nem létezõ táblákba
            DbInitializer.Seed(app);

            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Dashboard, DashboardDto>();
                cfg.CreateMap<Report, ReportDto>();
            });
        }
    }
}
