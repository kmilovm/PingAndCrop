using PingAndCrop.Objects.Models;

namespace PingAndCrop.Domain.Interfaces
{
    public interface IEntityService
    {

        Task<IEnumerable<TEntVm>> Get<TEnt, TEntVm>(string userId = "")
            where TEnt : BaseEntity
            where TEntVm : BaseEntity;

        Task<IEnumerable<TEnt>> GetAll<TEnt>(string userId = "") where TEnt : BaseEntity;

        Task<bool> Set<TEnt>(TEnt request) where TEnt : BaseEntity;

        Task<bool> UnSet<TEnt>(TEnt request) where TEnt : BaseEntity;
    }
}
