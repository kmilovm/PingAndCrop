using Azure;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PingAndCrop.Domain.Interfaces;
using PingAndCrop.Objects.Models;

namespace PingAndCrop.Domain.Services
{
    /// <summary>Service made for handling the messages from Azure Queue</summary>
    public class QueueService(IConfiguration config) : IQueueService
    {
        /// <summary>Gets or sets the azure storage connection string.</summary>
        /// <value>The azure storage connection string.</value>
        public string AzureStorageConnectionString { get; set; } = config["AzureStorageEndPoint"] ?? throw new InvalidOperationException();

        /// <summary>Ensures the creation of the Queue</summary>
        /// <param name="queueName">Name of the queue.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<Response<bool>> EnsureCreation(string queueName)
        {
            var queueClient = new QueueClient(AzureStorageConnectionString, queueName);
            await queueClient.CreateIfNotExistsAsync();
            return await queueClient.ExistsAsync();
        }

        /// <summary>Gets the specified message from Queue</summary>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<Response<QueueMessage[]>> Get(string queueName, string userId = "")
        {
            await EnsureCreation(queueName);
            var queueClient = new QueueClient(AzureStorageConnectionString, queueName);
            var maxItemsPerRun = Convert.ToInt32(config["MaxItemsPerRun"]);
            return await queueClient.ReceiveMessagesAsync(maxItemsPerRun);
        }

        /// <summary>Sets the specified message on queue</summary>
        /// <typeparam name="TEnt">The type of the ent.</typeparam>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<Response<SendReceipt>> Set<TEnt>(string queueName, TEnt request) where TEnt : BaseEntity
        {
            await EnsureCreation(queueName);
            var queueClient = new QueueClient(AzureStorageConnectionString, queueName);
            var response = await queueClient.SendMessageAsync(JsonConvert.SerializeObject(request));
            return response;
        }

        /// <summary>Deletes a message from Queue</summary>
        /// <param name="queueNameIn">The queue name in.</param>
        /// <param name="queueMessage">The queue message.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<Response> UnSet(string queueNameIn, QueueMessage queueMessage)
        {
            var queueClient = new QueueClient(AzureStorageConnectionString, queueNameIn);
            var responseOut = await queueClient.DeleteMessageAsync(queueMessage.MessageId, queueMessage.PopReceipt);
            return responseOut;
        }
    }
}
