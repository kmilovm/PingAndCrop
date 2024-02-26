using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using PingAndCrop.Objects.Functions;

namespace PingAndCrop.Functions
{
    public class TimedRequest
    {
        private readonly ILogger _logger;

        public TimedRequest(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<TimedRequest>();
        }

        [Function("TimedRequest")]
        public void Run([TimerTrigger("0 */5 * * * *")] PacInfo pacTimer)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            _logger.LogInformation($"Next timer schedule at: {pacTimer.ScheduleStatus.Next}");
        }
    }
}
