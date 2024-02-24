using Microsoft.Extensions.DependencyInjection;
using PingAndCrop.Domain.Interfaces;

namespace PingAndCrop.Tests
{
    public class StartUp
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IRequestService, IRequestService>();
        }
    }
}
