using Azure;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PingAndCrop.Domain.Interfaces;
using PingAndCrop.Objects.Requests;

namespace PingAndCrop.Domain.Services
{
    public class QueueService : IQueueService
    {
        public string AzureStorageConnectionString { get; set; }

        public QueueService(IConfiguration config)
        {
            AzureStorageConnectionString = config["AzureStorageEndPoint"] ?? throw new InvalidOperationException();
        }

        public async Task<Response<bool>> EnsureQueueCreation(string queueName)
        {
            var queueClient = new QueueClient(AzureStorageConnectionString, queueName);
            await queueClient.CreateIfNotExistsAsync();
            return await queueClient.ExistsAsync();
        }

        public async Task<Response<QueueMessage[]>> GetMessagesFromQueue(string queueName)
        {
            await EnsureQueueCreation(queueName);
            var queueClient = new QueueClient(AzureStorageConnectionString, queueName);
            return await queueClient.ReceiveMessagesAsync(32);
        }

        public async Task<Response<SendReceipt>> EnqueueMessage<TEnt>(string queueName, TEnt request)
        {
            await EnsureQueueCreation(queueName);
            var queueClient = new QueueClient(AzureStorageConnectionString, queueName);
            var response =  await queueClient.SendMessageAsync(JsonConvert.SerializeObject(request));
            return response;
        }

        public async Task<Response<SendReceipt>> DequeueMessage(string queueNameIn, string queueNameOut, QueueMessage queueMessage)
        {
            var queueClient = new QueueClient(AzureStorageConnectionString, queueNameIn);
            var responseOut = await queueClient.DeleteMessageAsync(queueMessage.MessageId, queueMessage.PopReceipt);
            var responseIn = await EnqueueMessage(queueNameOut, JsonConvert.DeserializeObject<PacRequest>(queueMessage.MessageText)!);
            return responseIn;
        }
    }
}
