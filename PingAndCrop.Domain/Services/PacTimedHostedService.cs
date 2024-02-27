using Cronos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using PingAndCrop.Domain.Interfaces;
using PingAndCrop.Objects.Requests;
using PingAndCrop.Objects.Responses;

namespace PingAndCrop.Domain.Services
{
    public class PacTimedHostedService : BackgroundService
    {
        private readonly CronExpression _cronExp;
        private readonly IConfiguration _config;
        
        private readonly IQueueService _queueService;
        private readonly IPacRequestService _pacRequestService;
        
        public PacTimedHostedService(IConfiguration config, IQueueService queueService, IPacRequestService pacRequestService)
        {
            _config = config;
            _queueService = queueService;
            _pacRequestService = pacRequestService;
            _cronExp = CronExpression.Parse(config["CronExpression"]);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var responses = new List<PacResponse>();
            while (!stoppingToken.IsCancellationRequested)
            {
                var nextExec = _cronExp.GetNextOccurrence(DateTime.UtcNow, true);
                if (!nextExec.HasValue) continue;
                var messages = await _queueService.GetMessagesFromQueue(_config["QueueName"]!);
                foreach (var msg in messages.Value)
                {
                    var pacRequest = JsonConvert.DeserializeObject<PacRequest>(msg.MessageText);
                    if (pacRequest == null) continue;
                    var response = await _pacRequestService.ProcessRequest(pacRequest);
                    responses.Add(response);
                }
                await _pacRequestService.NotifyResponses(responses);
                var valueMilliseconds = (nextExec - DateTime.Now).Value.Milliseconds;
                valueMilliseconds = valueMilliseconds > 0 ? valueMilliseconds : 1000;
                await Task.Delay(valueMilliseconds, stoppingToken);
            }
        }
    }
}
