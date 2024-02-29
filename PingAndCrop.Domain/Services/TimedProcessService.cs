using Cronos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using PingAndCrop.Domain.Interfaces;

namespace PingAndCrop.Domain.Services
{
    public class TimedProcessService(
        IConfiguration config,
        IManagementBaseService managementService,
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
                    
                    await managementService.GetAndProcessMessages(queueIn);

                    var valueMilliseconds = (nextExec - DateTime.Now).Value.Milliseconds;
                    valueMilliseconds = valueMilliseconds > 0 ? valueMilliseconds : 1000;
                    await Task.Delay(valueMilliseconds, stoppingToken);
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
