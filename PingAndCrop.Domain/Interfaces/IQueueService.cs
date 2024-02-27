using Azure;
using Azure.Storage.Queues.Models;

namespace PingAndCrop.Domain.Interfaces
{
    public interface IQueueService
    {
        Task<Azure.Response<bool>> EnsureQueueCreation(string queueName);

        Task<Azure.Response<QueueMessage[]>> GetMessagesFromQueue(string queueName);
        
        Task<Azure.Response<SendReceipt>> EnqueueMessage<TEnt>(string queueName, TEnt request);

        Task<Response<SendReceipt>> DequeueMessage(string queueNameIn, string queueNameOut, QueueMessage queueMessage);
    }
}
