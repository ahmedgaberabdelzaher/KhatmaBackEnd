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
            services.AddControllers().AddXmlDataContractSerializerFormatters();
           ///local
          // var ConnectionString = @"Server=DESKTOP-MCDM6RJ\MSSQLSERVER01;Database=KhatmaDB;Trusted_Connection=True;MultipleActiveResultSets=true";
            ///Smarter
         var ConnectionString = "Data Source = SQL5053.site4now.net; Initial Catalog = DB_A62FD0_KhatmaDB; User Id = DB_A62FD0_KhatmaDB_admin;Password = gaber789421; MultipleActiveResultSets=True";

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
            app.UseHangfireDashboard();
            app.UseHangfireServer();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            IServiceProvider servicesProvider = _services.BuildServiceProvider();
            IHangFireJobService hngfirSrvc =servicesProvider.GetRequiredService<IHangFireJobService>();
           RecurringJob.AddOrUpdate(() => hngfirSrvc.UpdateKhatmaCountHangfire(), Cron.MinuteInterval(2));

        }
    }
}
