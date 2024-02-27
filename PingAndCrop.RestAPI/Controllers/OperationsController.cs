using Azure.Storage.Queues.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PingAndCrop.Domain.Constants;
using PingAndCrop.Domain.Interfaces;
using PingAndCrop.Objects.Requests;
using PingAndCrop.Objects.Responses;

namespace PingAndCrop.RestAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OperationsController(ILogger<OperationsController> logger, IConfiguration config, IQueueService queueService) : ControllerBase
    {
        [HttpPost("EnqueueRequest")]
        
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PacResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        
        public async Task<Azure.Response<SendReceipt>> Enqueue([FromBody] PacRequest pacRequest)
        {
            logger.LogInformation(string.Format(StringMessages.InitiatingRequest, pacRequest.RequestedUrl, DateTime.Now.ToLongTimeString()));
            var queueName = config["QueueName"];
            var answer = await queueService.EnqueueMessage(queueName, pacRequest);
            logger.LogInformation(string.Format(StringMessages.FinalizingRequest, pacRequest.RequestedUrl, DateTime.Now.ToLongTimeString()));

            if (answer.HasValue) return answer;
            logger.LogInformation(string.Format(StringMessages.ErrorProcessingRequest, pacRequest.RequestedUrl));
            throw new BadHttpRequestException(StringMessages.ErrorProcessingRequest);

        }

        [HttpPost("GetMessages")]

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PacResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<List<PacRequest>> GetMessages()
        {
            var requests = new List<PacRequest>();
            var queueName = config["QueueName"];
            var messages = await queueService.GetMessagesFromQueue(queueName);
            if (messages.HasValue)
            {
                requests.AddRange(messages.Value.Select(message => JsonConvert.DeserializeObject<PacRequest>(message.MessageText))!);
            }
            logger.LogInformation(string.Format(StringMessages.NoMessagesAtQueue, queueName));
            return requests;
        }
    }
}
