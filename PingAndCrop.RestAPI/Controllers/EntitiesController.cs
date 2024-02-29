using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PingAndCrop.Domain.Constants;
using PingAndCrop.Domain.Interfaces;
using PingAndCrop.Objects.Models.Requests;
using PingAndCrop.Objects.Models.Responses;
using PingAndCrop.Objects.ViewModels;

namespace PingAndCrop.RestAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EntitiesController(IEntityService entityService) : ControllerBase
    {

        [HttpPost("StoreRequest")]

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PacResponseVm))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<bool> Store([FromBody] PacRequest pacRequest)
        {
            return await entityService.Set(pacRequest);
        }

        [HttpGet("GetRequests")]

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PacRequest>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IEnumerable<PacRequestVm>> GetRequests()
        {
            return await entityService.Get<PacRequest, PacRequestVm>();
        }

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
