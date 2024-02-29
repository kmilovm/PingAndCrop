using Azure;
using Azure.Data.Tables;
using Azure.Storage.Queues.Models;
using PingAndCrop.Objects.Models;

namespace PingAndCrop.Domain.Interfaces
{
    public interface ITableService
    {

        Task<TableClient> EnsureCreation(string tableName);

        Task<AsyncPageable<TEnt>> Get<TEnt>(string tableName, string userId = "") where TEnt : BaseEntity;

        Task<Response> Set<TEnt>(string tableName, TEnt request) where TEnt : BaseEntity;

        Task<Response> UnSet(string tableNameIn, QueueMessage queueMessage);
    }
}
