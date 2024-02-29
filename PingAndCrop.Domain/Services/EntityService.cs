using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using PingAndCrop.Data;
using PingAndCrop.Domain.Constants;
using PingAndCrop.Domain.Interfaces;
using PingAndCrop.Objects.Models;

namespace PingAndCrop.Domain.Services
{
    public class EntityService(IMapper mapper, DataContext dataContext) : IEntityService
    {
        public IMapper Mapper { get; } = mapper ?? throw new ArgumentException(StringMessages.NoMapper);

        public async Task<IEnumerable<TEntVm>> Get<TEnt, TEntVm>(string userId = "")
            where TEnt : BaseEntity
            where TEntVm : BaseEntity
        {
            return await dataContext.Set<TEnt>().AsNoTracking().ProjectTo<TEntVm>(Mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<IEnumerable<TEnt>> GetAll<TEnt>(string userId = "")
            where TEnt : BaseEntity
        {
            return await dataContext.Set<TEnt>().AsNoTracking().ToListAsync();
        }

        public async Task<bool> Set<TEnt>(TEnt request) where TEnt : BaseEntity
        {
            await dataContext.Set<TEnt>().AddAsync(request);
            var result = await dataContext.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> UnSet<TEnt>(TEnt request) where TEnt : BaseEntity
        {
            var entityToRemove = await dataContext.Set<TEnt>().SingleOrDefaultAsync(ent => ent.Id.Equals(request.Id));
            if (entityToRemove == default) return false;
            dataContext.Entry(entityToRemove).State = EntityState.Deleted;
            var result = await dataContext.SaveChangesAsync() > 0;
            return result;
        }
    }
}
