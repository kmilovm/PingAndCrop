using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PingAndCrop.Data;
using PingAndCrop.Domain.Constants;
using PingAndCrop.Objects.Models.Requests;
using PingAndCrop.Objects.ViewModels;

namespace PingAndCrop.RestAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EntitiesController(DataContext dataContext, IMapper mapper) : ControllerBase
    {
        public IMapper Mapper { get; } = mapper ?? throw new ArgumentException(StringMessages.NoMapper);

        [HttpPost("StoreRequest")]

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PacResponseVm))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async ValueTask<EntityEntry<PacRequest>> Store([FromBody] PacRequest pacRequest)
        {
            var addResult = await dataContext.Requests.AddAsync(pacRequest);
            await dataContext.SaveChangesAsync(CancellationToken.None);
            return addResult;
        }

        [HttpGet("GetRequests")]

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PacRequest>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IEnumerable<PacRequest>> GetRequests()
        {
            return await dataContext.Requests.AsNoTracking().ToListAsync();
        }

        [HttpGet("GetResponses")]

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PacResponseVm>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IEnumerable<PacResponseVm>> GetResponses()
        {
            return await dataContext.Responses.ProjectTo<PacResponseVm>(Mapper.ConfigurationProvider).AsNoTracking().ToListAsync();
        }
    }
}
