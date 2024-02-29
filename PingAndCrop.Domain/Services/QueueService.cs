using Azure;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PingAndCrop.Domain.Interfaces;
using PingAndCrop.Objects.Models;

namespace PingAndCrop.Domain.Services
{
    public class QueueService(IConfiguration config) : IQueueService
    {
        public string AzureStorageConnectionString { get; set; } = config["AzureStorageEndPoint"] ?? throw new InvalidOperationException();

        public async Task<Response<bool>> EnsureCreation(string queueName)
        {
            var queueClient = new QueueClient(AzureStorageConnectionString, queueName);
            await queueClient.CreateIfNotExistsAsync();
            return await queueClient.ExistsAsync();
        }

        public async Task<Response<QueueMessage[]>> Get(string queueName, string userId = "")
        {
            await EnsureCreation(queueName);
            var queueClient = new QueueClient(AzureStorageConnectionString, queueName);
            var maxItemsPerRun = Convert.ToInt32(config["MaxItemsPerRun"]);
            return await queueClient.ReceiveMessagesAsync(maxItemsPerRun);
        }

        public async Task<Response<SendReceipt>> Set<TEnt>(string queueName, TEnt request) where TEnt : BaseEntity
        {
            await EnsureCreation(queueName);
            var queueClient = new QueueClient(AzureStorageConnectionString, queueName);
            var response =  await queueClient.SendMessageAsync(JsonConvert.SerializeObject(request));
            return response;
        }

        public async Task<Response> UnSet(string queueNameIn, QueueMessage queueMessage)
        {
            var queueClient = new QueueClient(AzureStorageConnectionString, queueNameIn);
            var responseOut = await queueClient.DeleteMessageAsync(queueMessage.MessageId, queueMessage.PopReceipt);
            return responseOut;
        }
    }
}
