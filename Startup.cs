using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Hangfire;
using KhatmaBackEnd.DBContext;
using KhatmaBackEnd.Managers.Classes;
using KhatmaBackEnd.Managers.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Cronos;
using Hangfire.Dashboard;
using System.IO;
using Microsoft.Extensions.FileProviders;

namespace KhatmaBackEnd
{
    public class Startup
    {
        IServiceCollection _services;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            _services = services;
            services.AddControllers();
            //.AddXmlDataContractSerializerFormatters();
            ///local
            // var ConnectionString = @"Server=DESKTOP-MCDM6RJ\MSSQLSERVER01;Database=KhatmaDB;Trusted_Connection=True;MultipleActiveResultSets=true";
            ///Smarter//sql5063.site4now.net ahmed
            //   var ConnectionString = "Data Source =SQL5063.site4now.net;Initial Catalog=DB_A62FD0_KhatmaDB;User Id=DB_A62FD0_KhatmaDB_admin;Password = gaber789421; MultipleActiveResultSets=True";
         //   var ConnectionString = "Data Source =SQL5097.site4now.net;Initial Catalog=DB_A6945F_KhatmaDB;User Id=DB_A6945F_KhatmaDB_admin;Password =doaa11711; MultipleActiveResultSets=True";
         //  var ConnectionString = "Data Source =SQL5053.site4now.net;Initial Catalog=DB_A6BD77_ahmedgaber1994;User Id=DB_A6BD77_ahmedgaber1994_admin;Password = gaber789421; MultipleActiveResultSets=True";
            IConfigurationRoot configuration = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json")
              .Build();
            var ConnectionString = configuration["KhatmaDBConnection"];

            services.AddDbContext<KhatmaContext>(op => {
                    op .UseLazyLoadingProxies()
                    .ConfigureWarnings(warnings => warnings.Ignore(CoreEventId.DetachedLazyLoadingWarning))
                .UseSqlServer(ConnectionString);
            });

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddScoped<IUserManager, UserManager>();
            services.AddScoped<IGroupManager, GroupManager>();
            services.AddSingleton<INotificationManager, NotificationManager>();
            services.AddHangfire(x => x.UseSqlServerStorage(ConnectionString));
            services.AddTransient(typeof(IHangFireJobService), typeof(HangFireJobService));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //HangFire
            //       app.UseHangfireDashboard();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
        Path.Combine(env.ContentRootPath, "Images")),
                RequestPath = "/Images"
            });
            app.UseAuthorization();
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] { new MyAuthorizationFilter() },
               
            });
            app.UseHangfireServer();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            IServiceProvider servicesProvider = _services.BuildServiceProvider();
            IHangFireJobService hngfirSrvc =servicesProvider.GetRequiredService<IHangFireJobService>();
            IHangFireJobService hngfirSrvc2 =servicesProvider.GetRequiredService<IHangFireJobService>();
            var jobId2 = BackgroundJob.Schedule(
() => hngfirSrvc2.NotifyUnreadedUsers(),
TimeSpan.FromSeconds(1));
            /*  var jobId2 = BackgroundJob.Schedule(
  () => hngfirSrvc2.UpdateKhatmaCountHangfire(),
  TimeSpan.FromSeconds(1));
              var jobId = BackgroundJob.Schedule(
      () => hngfirSrvc2.NotifyUnreadedUsers(),
      TimeSpan.FromSeconds(2));*/

           // RecurringJob.AddOrUpdate(() => hngfirSrvc2.NotifyUnreadedUsers(), Cron.Hourly);
          RecurringJob.AddOrUpdate(() => hngfirSrvc.UpdateKhatmaCountHangfire(), Cron.Daily(2));
       //  RecurringJob(() => hngfirSrvc2.NotifyUnreadedUsers(), "0 0 */4 * **");



        }
    }

    public class MyAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();

            // Allow all authenticated users to see the Dashboard (potentially dangerous).
            // return httpContext.User.Identity.IsAuthenticated;
            return true;
        }
    }
}
