using AutoMapper;
using Azure.Storage.Queues.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PingAndCrop.Domain.Constants;
using PingAndCrop.Domain.Interfaces;
using PingAndCrop.Objects.Requests;
using PingAndCrop.Objects.Responses;
using PingAndCrop.Objects.ViewModels;

namespace PingAndCrop.RestAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OperationsController(ILogger<OperationsController> logger, IConfiguration config, IQueueService queueService, IMapper mapper) : ControllerBase
    {
        public IMapper Mapper { get; } = mapper;

        [HttpPost("EnqueueRequest")]
        
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PacResponseVm))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        
        public async Task<Azure.Response<SendReceipt>> Enqueue([FromBody] PacRequest pacRequest)
        {
            logger.LogInformation(string.Format(StringMessages.InitiatingRequest, pacRequest.RequestedUrl, DateTime.Now.ToLongTimeString()));
            var queueName = config["QueueNameIn"];
            var answer = await queueService.EnqueueMessage(queueName!, pacRequest);
            logger.LogInformation(string.Format(StringMessages.FinalizingRequest, pacRequest.RequestedUrl, DateTime.Now.ToLongTimeString()));

            if (answer.HasValue) return answer;
            logger.LogInformation(string.Format(StringMessages.ErrorProcessingRequest, pacRequest.RequestedUrl));
            throw new BadHttpRequestException(StringMessages.ErrorProcessingRequest);

        }

        [HttpPost("GetRequestMessages")]

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PacRequest>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IEnumerable<PacRequest>> GetRequestMessages()
        {
            var queueName = config["QueueNameIn"] ?? throw new ArgumentException(StringMessages.NoQueueFoundAtConfig);
            var requests = new List<PacRequest>();
            var messages = await queueService.GetMessagesFromQueue(queueName);
            if (messages.HasValue && messages.Value.Any())
            {
                requests.AddRange(messages.Value.Select(message => JsonConvert.DeserializeObject<PacRequest>(message.MessageText))!);
            }
            logger.LogInformation(string.Format(StringMessages.NoMessagesAtQueue, queueName));
            return requests;
        }

        [HttpPost("GetResponseMessages")]

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PacResponseVm>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IEnumerable<PacResponseVm>> GetResponseMessages()
        {
            var queueName = config["QueueNameOut"] ?? throw new ArgumentException(StringMessages.NoQueueFoundAtConfig);
            var requests = new List<PacResponse>();
            
            var messages = await queueService.GetMessagesFromQueue(queueName);
            if (messages.HasValue && messages.Value.Length != 0)
            {
                foreach (var queueMessage in messages.Value)
                {
                    var serializedMessage = JsonConvert.DeserializeObject<PacResponse>(queueMessage.MessageText);
                    if (serializedMessage == null) continue;
                    serializedMessage.Message = queueMessage;
                    requests.Add(serializedMessage);
                }
            }
            var responsesVm = mapper.Map<List<PacResponse>, List<PacResponseVm>>(requests);
            return responsesVm;
        }
    }
}
