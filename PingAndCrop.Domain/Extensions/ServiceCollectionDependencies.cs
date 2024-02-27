using Microsoft.Extensions.DependencyInjection;
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
            services.AddSingleton<IPacRequestService, PacRequestService>();
            services.AddSingleton<IQueueService, QueueService>();
            services.AddHostedService<PacTimedHostedProcessService>();
            return services;
        }
    }
}
