using Azure;
using Azure.Data.Tables;
using Azure.Storage.Queues.Models;
using Microsoft.Extensions.Configuration;
using PingAndCrop.Domain.Interfaces;
using PingAndCrop.Objects.Models;
using PingAndCrop.Objects.Models.Responses;

namespace PingAndCrop.Domain.Services
{
    public class TableService(IConfiguration config) : ITableService
    {
        public string AzureStorageConnectionString { get; set; } = config["AzureStorageEndPoint"] ?? throw new InvalidOperationException();
        
        public async Task<TableClient> EnsureCreation(string tableName)
        {
            var tableServiceClient = new TableServiceClient(AzureStorageConnectionString);
            await tableServiceClient.CreateTableIfNotExistsAsync(tableName);
            return tableServiceClient.GetTableClient(tableName);
        }

        public async Task<AsyncPageable<TEnt>> Get<TEnt>(string tableName, string userId = "") where TEnt : BaseEntity
        {
            var tableClient = await EnsureCreation(tableName);
            var results = tableClient.QueryAsync<TEnt>(string.Empty);
            return results;
        }

        public async Task<Response> Set<TEnt>(string tableName, TEnt request) where TEnt : BaseEntity
        {
            var tableClient = await EnsureCreation(tableName);
            return await tableClient.AddEntityAsync<TEnt>(request);
        }

        public Task<Response> UnSet(string tableNameIn, QueueMessage queueMessage)
        {
            throw new NotImplementedException();
        }
    }
}
