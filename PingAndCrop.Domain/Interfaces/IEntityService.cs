using PingAndCrop.Objects.Models;

namespace PingAndCrop.Domain.Interfaces
{
    /// <summary>Created for handling the requests through EF</summary>
    public interface IEntityService
    {
        /// <summary>Gets the entities from database for an specific DbSet</summary>
        /// <typeparam name="TEnt">The type of the ent.</typeparam>
        /// <typeparam name="TEntVm">The type of the ent vm.</typeparam>
        /// <param name="userId">The user identifier.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        Task<IEnumerable<TEntVm>> Get<TEnt, TEntVm>(string userId = "")
            where TEnt : BaseEntity
            where TEntVm : BaseEntity;

        /// <summary>Gets All the entities from database for an specific DbSet</summary>
        /// <typeparam name="TEnt">The type of the ent.</typeparam>
        /// <param name="userId">The user identifier.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        Task<IEnumerable<TEnt>> GetAll<TEnt>(string userId = "") where TEnt : BaseEntity;

        /// <summary>Sets the specified entity to database.</summary>
        /// <typeparam name="TEnt">The type of the ent.</typeparam>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        Task<bool> Set<TEnt>(TEnt request) where TEnt : BaseEntity;

        /// <summary>Deletes an entity from the database</summary>
        /// <typeparam name="TEnt">The type of the ent.</typeparam>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        Task<bool> UnSet<TEnt>(TEnt request) where TEnt : BaseEntity;
    }
}
