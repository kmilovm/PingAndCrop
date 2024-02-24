using Microsoft.AspNetCore.Mvc;
using PingAndCrop.Domain.Constants;
using PingAndCrop.Domain.Interfaces;
using PingAndCrop.Objects;

namespace PingAndCrop.RestAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OperationsController(ILogger<OperationsController> logger, IRequestService requestService) : ControllerBase
    {
        [HttpPost(Name = "EnqueueRequest")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        
        public async Task<Response> Get([FromBody] Request request)
        {
            logger.LogInformation(string.Format(StringMessages.InitiatingRequest, request.RequestedUrl, DateTime.Now.ToLongTimeString()));
            var answer = await requestService.ProcessRequest(request);
            logger.LogInformation(string.Format(StringMessages.FinalizingRequest, request.RequestedUrl, DateTime.Now.ToLongTimeString()));
            return answer;
        }
    }
}
