using Azure.Storage.Queues.Models;
using PingAndCrop.Objects.Requests;

namespace PingAndCrop.Domain.Interfaces
{
    public interface IQueueService
    {
        Task<Azure.Response<bool>> EnsureQueueCreation(string queueName);

        Task<Azure.Response<QueueMessage[]>> GetMessagesFromQueue(string queueName);
        
        Task<Azure.Response<SendReceipt>> EnqueueMessage(string queueName, PacRequest request);
        
    }
}
