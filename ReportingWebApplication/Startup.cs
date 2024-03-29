using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using szakdoga.BusinessLogic;
using szakdoga.Data;
using szakdoga.Models;
using szakdoga.Models.Dtos;
using szakdoga.Models.Dtos.DashboardDtos;
using szakdoga.Models.Dtos.ReportDtos;
using szakdoga.Models.Repositories;
using szakdoga.Models.RepositoryInterfaces;
using szakdoga.Others;

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
            services.AddSingleton(_configurationRoot);
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(_configurationRoot.GetConnectionString("DefaultConnection")));
            services.AddTransient<DbInitializer>();

            services.AddScoped<IQueryRepository, QueryRepository>();
            services.AddScoped<IReportRepository, ReportRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IDashboardRepository, DashboardRepository>();
            services.AddScoped<IReportDashboardRelRepository, ReportDashboardRelRepository>();
            services.AddScoped<IReportUserRelRepository, ReportUserRelRepository>();
            services.AddScoped<IUserDashboardRelRepository, UserDashboardRelRepository>();
            services.AddScoped<IUserJwtMapRepository, UserJwtMapRepository>();

            services.AddScoped<DashboardManager>();
            services.AddScoped<QueryManager>();
            services.AddScoped<ReportManager>();
            services.AddScoped<ReportUserRelManager>();
            services.AddScoped<DashboardUserRelManager>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                   .AddJwtBearer(options =>
                   {
                       options.TokenValidationParameters =
                            new TokenValidationParameters
                            {
                                ValidateIssuer = true,
                                ValidateAudience = true,
                                ValidateLifetime = true,
                                ValidateIssuerSigningKey = true,

                                ValidIssuer = _configurationRoot["Tokens:Issuer"],
                                ValidAudience = _configurationRoot["Tokens:Audience"],
                                IssuerSigningKey =
                                 JwtSecurityKey.Create(_configurationRoot["Tokens:Key"])
                            };
                       options.Events = new JwtBearerEvents
                       {
                           OnAuthenticationFailed = context =>
                           {
                               Debug.WriteLine("OnAuthenticationFailed: " +
                                   context.Exception.Message);
                               return Task.CompletedTask;
                           },
                           OnTokenValidated = context =>
                           {
                               Debug.WriteLine("OnTokenValidated: " +
                                   context.SecurityToken);
                               return Task.CompletedTask;
                           }
                       };
                   });

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                  builder => builder.AllowAnyOrigin()
                                    .AllowAnyMethod()
                                    .AllowAnyHeader()
                                    .AllowCredentials());
            });

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, DbInitializer dbInitializer, AppDbContext context)
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

            //ensuredcreate will not update the tables with modifies!!!
            //EF_History not inclueded!!!
            if (context.Database.EnsureCreated() && bool.Parse(_configurationRoot["PoppulateWithDemoData"]) == true)
            {
                dbInitializer.Seed();
            }
            app.UseCors("CorsPolicy");
            app.UseAuthentication();

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

            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Dashboard, DashboardDto>();
                cfg.CreateMap<Dashboard, DashboardForAllDto>();
                cfg.CreateMap<Report, ReportDto>();
                cfg.CreateMap<Report, ReportForAllDto>().ForMember(dest => dest.HasStyle, opt => opt.MapFrom(src => !String.IsNullOrEmpty(src.Style)));
                cfg.CreateMap<User, UserDto>();
                cfg.CreateMap<Query, QueryDto>();
            });
        }
    }
}
