using PingAndCrop.Domain.Interfaces;
using PingAndCrop.Domain.Services;

namespace PingAndCrop.RestAPI.Extensions
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
            return services;
        }
    }
}
