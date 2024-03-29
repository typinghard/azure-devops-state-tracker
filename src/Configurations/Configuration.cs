using AzureDevopsTracker.Adapters;
using AzureDevopsTracker.Data;
using AzureDevopsTracker.Data.Context;
using AzureDevopsTracker.Integrations;
using AzureDevopsTracker.Interfaces;
using AzureDevopsTracker.Interfaces.Internals;
using AzureDevopsTracker.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace AzureDevopsTracker.Configurations
{
    public static class Configuration
    {
        public static IServiceCollection AddAzureDevopsTracker(this IServiceCollection services,
                                                               DataBaseConfig configurations,
                                                               MessageConfig messageConfig = null)
        {
            services.AddDbContext<AzureDevopsTrackerContext>(options =>
                    options.UseSqlServer(DataBaseConfig.ConnectionsString));

            // Crie o escopo de serviços e aplique as migrações pendentes
            using (var serviceScope = services.BuildServiceProvider().CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<AzureDevopsTrackerContext>();
                // Verifique se há migrações pendentes
                var pendingMigrations = dbContext.Database.GetPendingMigrations();

                if (pendingMigrations.Any())
                {
                    // Aplicar migrações pendentes
                    dbContext.Database.Migrate();
                }
            }

            services.AddMessageIntegrations();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<AzureDevopsTrackerContext>();
            services.AddScoped<IWorkItemAdapter, WorkItemAdapter>();
            services.AddScoped<IWorkItemRepository, WorkItemRepository>();

            services.AddScoped<IAzureDevopsTrackerService, AzureDevopsTrackerService>();

            services.AddScoped<IChangeLogService, ChangeLogService>();
            services.AddScoped<IChangeLogItemRepository, ChangeLogItemRepository>();
            services.AddScoped<IChangeLogRepository, ChangeLogRepository>();

            return services;
        }

        private static IServiceCollection AddMessageIntegrations(this IServiceCollection services)
        {
            switch (MessageConfig.Messenger)
            {
                case EMessengers.DISCORD:
                    services.AddScoped<MessageIntegration, DiscordIntegration>();
                    break;
                case EMessengers.MICROSOFT_TEAMS:
                    services.AddScoped<MessageIntegration, MicrosoftTeamsIntegration>();
                    break;
                default:
                    services.AddScoped<MessageIntegration, FakeIntegration>();
                    break;
            }
            return services;
        }

        public static IApplicationBuilder UseAzureDevopsTracker(this IApplicationBuilder app, IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetService<AzureDevopsTrackerContext>();
            context.Database.Migrate();

            return app;
        }
    }
}