using Cronos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PingAndCrop.Domain.Interfaces;

namespace PingAndCrop.Domain.Services
{
    public class TimedProcessService(
        IConfiguration config,
        IServiceScopeFactory serviceScopeFactory,
        IHostApplicationLifetime lifetime)
        : BackgroundService
    {
        private readonly CronExpression _cronExp = CronExpression.Parse(config["CronExpression:Frequency"]);
        

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var isReady = false;
            lifetime.ApplicationStarted.Register(() => isReady = true);
            await Task.Factory.StartNew(async () =>
            {
                while (!isReady)
                {
                    await Task.Delay(1000, stoppingToken);
                }
                await ExecuteProcess(stoppingToken);
            }, stoppingToken);
        }

        private async Task ExecuteProcess(CancellationToken stoppingToken)
        {
            
            try
            {
                var queueIn = config["QueueNameIn"];
                while (!stoppingToken.IsCancellationRequested)
                {
                    var nextExec = _cronExp.GetNextOccurrence(DateTime.UtcNow, Convert.ToBoolean(config["CronExpression:EnabledAtStart"]));
                    if (!nextExec.HasValue) continue;
                    
                    using var scope = serviceScopeFactory.CreateScope();
                    var scopedProcessingService = scope.ServiceProvider.GetRequiredService<IManagementBaseService>();
                    await scopedProcessingService.GetAndProcessMessages(queueIn);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
