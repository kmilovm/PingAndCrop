using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PingAndCrop.Domain.Constants;
using PingAndCrop.Domain.Interfaces;
using PingAndCrop.Objects.Models.Requests;
using PingAndCrop.Objects.Models.Responses;

namespace PingAndCrop.RestAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TableController(ILogger<MessagesController> logger, IConfiguration config, ITableService tableService, IMapper mapper) : ControllerBase
    {
        [HttpGet("GetRecordMessages")]

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<PacRequest>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IEnumerable<PacResponse>> GetRecordMessages([FromQuery] string userId)
        {
            var queueName = config["QueueNameIn"] ?? throw new ArgumentException(StringMessages.NoQueueFoundAtConfig);
            var requests = new List<PacResponse>();
            var messages = tableService.Get<PacResponse>(queueName, userId).Result.AsPages();
            await foreach (var page in messages)
            {
                requests.AddRange(page.Values);
            }
            logger.LogInformation(string.Format(StringMessages.NoMessagesAtQueue, queueName));
            return requests;
        }
    }
}
