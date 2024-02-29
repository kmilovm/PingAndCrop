using Microsoft.AspNetCore.Mvc;
using PingAndCrop.Domain.Interfaces;
using PingAndCrop.Objects.Models.Requests;
using PingAndCrop.Objects.Models.Responses;
using PingAndCrop.Objects.ViewModels;

namespace PingAndCrop.RestAPI.Controllers
{
    /// <summary>Controller for handing entities at DB</summary>
    [ApiController]
    [Route("api/[controller]")]
    public class EntitiesController(IEntityService entityService) : ControllerBase
    {

        /// <summary>Stores the specified pac request.</summary>
        /// <param name="pacRequest">The pac request.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        [HttpPost("StoreRequest")]

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PacResponseVm))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<bool> Store([FromBody] PacRequest pacRequest)
        {
            return await entityService.Set(pacRequest);
        }

        /// <summary>Gets the requests.</summary>
        /// <returns>
        ///   <br />
        /// </returns>
        [HttpGet("GetRequests")]

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PacRequest>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IEnumerable<PacRequestVm>> GetRequests()
        {
            return await entityService.Get<PacRequest, PacRequestVm>();
        }

        /// <summary>Gets the responses.</summary>
        /// <returns>
        ///   <br />
        /// </returns>
        [HttpGet("GetResponses")]

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PacResponseVm>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IEnumerable<PacResponseVm>> GetResponses()
        {
            return await entityService.Get<PacResponse, PacResponseVm>();
        }
    }
}
