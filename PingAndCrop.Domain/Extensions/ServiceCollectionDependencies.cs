using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PingAndCrop.Data;
using PingAndCrop.Domain.Interfaces;
using PingAndCrop.Domain.Services;

namespace PingAndCrop.Domain.Extensions
{
    public static class ServiceCollectionDependencies
    {
        /// <summary>
        /// Method for adding the dependencies needed
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddInjectedDependencies(this IServiceCollection services)
        {
            services.AddSingleton<IQueueService, QueueService>();
            services.AddSingleton<ITableService, TableService>();
            services.AddScoped<IManagementBaseService, ManagementService>();
            services.AddScoped<IEntityService, EntityService>();
            services.AddHostedService<TimedProcessService>();
            return services;
        }

        public static IServiceCollection AddEfSupport(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(connectionString));
            var serviceProvider = services.BuildServiceProvider();
            using var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var context = serviceScope.ServiceProvider.GetRequiredService<DataContext>();
            context.Database.EnsureCreated();
            return services;
        }
    }
}
