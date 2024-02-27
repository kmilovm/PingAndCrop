using Azure;
using Azure.Storage.Queues.Models;

namespace PingAndCrop.Domain.Interfaces
{
    public interface IQueueService
    {
        Task<Response<bool>> EnsureQueueCreation(string queueName);

        Task<Response<QueueMessage[]>> GetMessagesFromQueue(string queueName);
        
        Task<Response<SendReceipt>> EnqueueMessage<TEnt>(string queueName, TEnt request);

        Task<Response> DequeueMessage(string queueNameIn, QueueMessage queueMessage);
    }
}
