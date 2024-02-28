using Cronos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using PingAndCrop.Domain.Interfaces;
using PingAndCrop.Objects.Models.Responses;

namespace PingAndCrop.Domain.Services
{
    public class PacTimedHostedProcessService(
        IConfiguration config,
        IQueueService queueService,
        IPacRequestService pacRequestService)
        : BackgroundService
    {
        private readonly CronExpression _cronExp = CronExpression.Parse(config["CronExpression:Frequency"]);

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var responses = new List<PacResponse>();
            try
            {
                var queueIn = config["QueueNameIn"];
                while (!stoppingToken.IsCancellationRequested)
                {
                    var nextExec = _cronExp.GetNextOccurrence(DateTime.UtcNow, Convert.ToBoolean(config["CronExpression:EnabledAtStart"]));
                    if (!nextExec.HasValue) continue;
                    
                    var messages = await queueService.GetMessagesFromQueue(queueIn!);
                    if (messages.HasValue && messages.Value.Length != 0)
                    {
                        responses.AddRange(await pacRequestService.ProcessRequests(messages.Value));
                        if (responses.Count != 0)
                        {
                            await pacRequestService.StoreResponses(responses);
                        }
                    }
                    
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
