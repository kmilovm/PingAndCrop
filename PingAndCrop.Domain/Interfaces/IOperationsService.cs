using Azure.Storage.Queues.Models;

namespace PingAndCrop.Domain.Interfaces
{
    public interface IOperationsService<TResB, TResQ, TResR, TRes>
    {
        Task<TResB> EnsureCreation(string queueName);

        Task<TResQ> Get(string queueName, string userId = "");

        Task<TResR> Set<TEnt>(string queueName, TEnt request) where TEnt : class;

        Task<TRes> UnSet(string queueNameIn, QueueMessage queueMessage);
    }
}
