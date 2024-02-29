using Azure;
using Azure.Data.Tables;
using Azure.Storage.Queues.Models;
using PingAndCrop.Objects.Models;

namespace PingAndCrop.Domain.Interfaces
{
    /// <summary>Service for handling the requests to Azure Table</summary>
    public interface ITableService
    {

        /// <summary>Ensures the creation of the azure table</summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        Task<TableClient> EnsureCreation(string tableName);

        /// <summary>Gets the records from the specified azure table</summary>
        /// <typeparam name="TEnt">The type of the ent.</typeparam>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        Task<AsyncPageable<TEnt>> Get<TEnt>(string tableName, string userId = "") where TEnt : BaseEntity;

        /// <summary>Sets the record into the azure table</summary>
        /// <typeparam name="TEnt">The type of the ent.</typeparam>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        Task<Response> Set<TEnt>(string tableName, TEnt request) where TEnt : BaseEntity;

        /// <summary>Delete the record from azure table</summary>
        /// <param name="tableNameIn">The table name in.</param>
        /// <param name="queueMessage">The queue message.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        Task<Response> UnSet(string tableNameIn, QueueMessage queueMessage);
    }
}
