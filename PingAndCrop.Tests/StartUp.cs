using Microsoft.Extensions.DependencyInjection;
using PingAndCrop.Domain.Extensions;

namespace PingAndCrop.Tests
{
    public class StartUp
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddInjectedDependencies();
        }
    }
}
