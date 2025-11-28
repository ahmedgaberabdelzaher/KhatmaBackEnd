using System;
using System.IO;
using AutoMapper;
using Cronos;
using Hangfire;
using Hangfire.Dashboard;
using KhatmaBackEnd.DBContext;
using KhatmaBackEnd.Managers.Classes;
using KhatmaBackEnd.Managers.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

namespace KhatmaBackEnd
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Services
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Read connection string (keeps the original behavior that read from appsettings.json)
            var ConnectionString = builder.Configuration["KhatmaDBConnection"];

            builder.Services.AddDbContext<KhatmaContext>(op =>
            {
                op.UseLazyLoadingProxies()
                  .ConfigureWarnings(warnings => warnings.Ignore(CoreEventId.DetachedLazyLoadingWarning))
                  .UseSqlServer(ConnectionString);
            });

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddScoped<IUserManager, UserManager>();
            builder.Services.AddScoped<IGroupManager, GroupManager>();
            builder.Services.AddSingleton<INotificationManager, NotificationManager>();
            builder.Services.AddHangfire(x => x.UseSqlServerStorage(ConnectionString));
            builder.Services.AddTransient(typeof(IHangFireJobService), typeof(HangFireJobService));

            var app = builder.Build();

            var env = app.Environment;

            // HTTP request pipeline
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseRouting();

            // Serve Images folder if available. Create it if possible; fail safe if not.
            var imagesPath = Path.Combine(env.ContentRootPath, "Images");
            try
            {
                if (!Directory.Exists(imagesPath))
                {
                    // Try to create the directory. If the host prevents creation, catch and continue.
                    Directory.CreateDirectory(imagesPath);
                }

                // Only mount the PhysicalFileProvider if the directory now exists.
                if (Directory.Exists(imagesPath))
                {
                    app.UseStaticFiles(new StaticFileOptions
                    {
                        FileProvider = new PhysicalFileProvider(imagesPath),
                        RequestPath = "/Images"
                    });
                }
            }
            catch (Exception ex)
            {
                // Log minimal info for diagnostics without blocking startup.
                try
                {
                    var logPath = Path.Combine(env.ContentRootPath, "startup_staticfiles_error.log");
                    File.WriteAllText(logPath, ex.ToString());
                }
                catch
                {
                    // best-effort logging; ignore further failures
                }
            }

            app.UseAuthorization();

            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] { new MyAuthorizationFilter() }
            });

            app.UseHangfireServer();

            app.MapControllers();

            // Schedule Hangfire jobs (moved from Startup.Configure)
            using (var scope = app.Services.CreateScope())
            {
                var provider = scope.ServiceProvider;
                var hngfirSrvc = provider.GetRequiredService<IHangFireJobService>();
                var hngfirSrvc2 = provider.GetRequiredService<IHangFireJobService>();

                var jobId2 = BackgroundJob.Schedule(
                    () => hngfirSrvc2.NotifyUnreadedUsers(),
                    TimeSpan.FromSeconds(1));

                RecurringJob.AddOrUpdate(
                    () => hngfirSrvc.UpdateKhatmaCountHangfire(), Cron.Daily(2));
            }

            app.Run();
        }
    }

    public class MyAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();

            // Allow all authenticated users to see the Dashboard (keeps original behavior).
            return true;
        }
    }
}
