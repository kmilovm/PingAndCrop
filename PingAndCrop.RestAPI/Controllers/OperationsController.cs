using Microsoft.AspNetCore.Mvc;
using PingAndCrop.Domain.Constants;
using PingAndCrop.Domain.Interfaces;
using PingAndCrop.Objects.Requests;
using PingAndCrop.Objects.Responses;

namespace PingAndCrop.RestAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OperationsController(ILogger<OperationsController> logger, IPacRequestService requestService) : ControllerBase
    {
        [HttpPost("EnqueueRequest")]
        
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PacResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        
        public async Task<PacResponse> Enqueue([FromBody] PacRequest pacRequest)
        {
            logger.LogInformation(string.Format(StringMessages.InitiatingRequest, pacRequest.RequestedUrl, DateTime.Now.ToLongTimeString()));
            var answer = await requestService.ProcessRequest(pacRequest);
            logger.LogInformation(string.Format(StringMessages.FinalizingRequest, pacRequest.RequestedUrl, DateTime.Now.ToLongTimeString()));
            return answer;
        }
    }
}
