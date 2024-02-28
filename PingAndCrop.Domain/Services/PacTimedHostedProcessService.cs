using Cronos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using PingAndCrop.Domain.Interfaces;
using PingAndCrop.Objects.Models.Responses;

namespace PingAndCrop.Domain.Services
{
    public class PacTimedHostedProcessService : BackgroundService
    {
        private readonly CronExpression _cronExp;
        private readonly IConfiguration _config;
        
        private readonly IQueueService _queueService;
        private readonly IPacRequestService _pacRequestService;
        
        public PacTimedHostedProcessService(IConfiguration config, IQueueService queueService, IPacRequestService pacRequestService)
        {
            _config = config;
            _queueService = queueService;
            _pacRequestService = pacRequestService;
            _cronExp = CronExpression.Parse(config["CronExpression"]);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var responses = new List<PacResponse>();
            try
            {
                var queueIn = _config["QueueNameIn"];
                while (!stoppingToken.IsCancellationRequested)
                {
                    var nextExec = _cronExp.GetNextOccurrence(DateTime.UtcNow, true);
                    if (!nextExec.HasValue) continue;
                    
                    var messages = await _queueService.GetMessagesFromQueue(queueIn!);
                    if (messages.HasValue && messages.Value.Any())
                    {
                        responses.AddRange(await _pacRequestService.ProcessRequests(messages.Value));
                        if (responses.Any())
                        {
                            await _pacRequestService.StoreResponses(responses);
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
