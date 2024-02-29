using Azure;
using Azure.Storage.Queues.Models;
using PingAndCrop.Objects.Models;

namespace PingAndCrop.Domain.Interfaces
{
    public interface IQueueService
    {

        Task<Response<bool>> EnsureCreation(string queueName);

        Task<Response<QueueMessage[]>> Get(string queueName, string userId = "");

        Task<Response<SendReceipt>> Set<TEnt>(string queueName, TEnt request) where TEnt : BaseEntity;

        Task<Response> UnSet(string queueNameIn, QueueMessage queueMessage);
    }
}
