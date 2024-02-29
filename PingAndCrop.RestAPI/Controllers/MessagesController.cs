using AutoMapper;
using Azure.Storage.Queues.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PingAndCrop.Domain.Constants;
using PingAndCrop.Domain.Interfaces;
using PingAndCrop.Objects.Models.Requests;
using PingAndCrop.Objects.Models.Responses;
using PingAndCrop.Objects.ViewModels;

namespace PingAndCrop.RestAPI.Controllers
{
    /// <summary>Provide a controller for handing messages at Azure Queue</summary>
    [ApiController]
    [Route("api/[controller]")]
    public class MessagesController(ILogger<MessagesController> logger, IConfiguration config, IQueueService queueService, IMapper mapper) : ControllerBase
    {
        public IMapper Mapper { get; } = mapper ?? throw new ArgumentException(StringMessages.NoMapper);

        /// <summary>Enqueues the specified pac request.</summary>
        /// <param name="pacRequest">The pac request.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <exception cref="Microsoft.AspNetCore.Server.IIS.BadHttpRequestException"></exception>
        [HttpPost("EnqueueRequestMessage")]

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Azure.Response<SendReceipt>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<Azure.Response<SendReceipt>> Enqueue([FromBody] PacRequest pacRequest)
        {
            logger.LogInformation(string.Format(StringMessages.InitiatingRequest, pacRequest.RequestedUrl, DateTime.Now.ToLongTimeString()));
            var queueName = config["QueueNameIn"];
            var answer = await queueService.Set(queueName!, pacRequest);
            logger.LogInformation(string.Format(StringMessages.FinalizingRequest, pacRequest.RequestedUrl, DateTime.Now.ToLongTimeString()));

            if (answer.HasValue) return answer;
            logger.LogInformation(string.Format(StringMessages.ErrorProcessingRequest, pacRequest.RequestedUrl));
            throw new BadHttpRequestException(StringMessages.ErrorProcessingRequest);

        }

        /// <summary>Gets the request messages.</summary>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <exception cref="System.ArgumentException"></exception>
        [HttpGet("GetRequestMessages")]

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<PacRequest>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IEnumerable<PacRequest>> GetRequestMessages()
        {
            var queueName = config["QueueNameIn"] ?? throw new ArgumentException(StringMessages.NoQueueFoundAtConfig);
            var requests = new List<PacRequest>();
            var messages = await queueService.Get(queueName);
            if (messages.HasValue && messages.Value.Length != 0)
            {
                requests.AddRange(messages.Value.Select(message => JsonConvert.DeserializeObject<PacRequest>(message.MessageText))!);
            }
            logger.LogInformation(string.Format(StringMessages.NoMessagesAtQueue, queueName));
            return requests;
        }

        /// <summary>Gets the response messages.</summary>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <exception cref="System.ArgumentException"></exception>
        [HttpGet("GetResponseMessages")]

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<PacResponseVm>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IEnumerable<PacResponseVm>> GetResponseMessages()
        {
            var queueName = config["QueueNameOut"] ?? throw new ArgumentException(StringMessages.NoQueueFoundAtConfig);
            var requests = new List<PacResponse>();

            var messages = await queueService.Get(queueName);
            if (messages.HasValue && messages.Value.Length != 0)
            {
                foreach (var queueMessage in messages.Value)
                {
                    var serializedMessage = JsonConvert.DeserializeObject<PacResponse>(queueMessage.MessageText);
                    if (serializedMessage == null) continue;
                    serializedMessage.Message = JsonConvert.SerializeObject(queueMessage);
                    requests.Add(serializedMessage);
                }
            }
            var responsesVm = Mapper!.Map<List<PacResponse>, List<PacResponseVm>>(requests);
            return responsesVm;
        }
    }
}
