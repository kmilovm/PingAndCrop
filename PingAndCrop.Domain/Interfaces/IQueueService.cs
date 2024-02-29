using Azure;
using Azure.Storage.Queues.Models;
using PingAndCrop.Objects.Models;

namespace PingAndCrop.Domain.Interfaces
{
    /// <summary>Queue services for handling the requests to Queue</summary>
    public interface IQueueService
    {

        /// <summary>Ensures the creation of the Queue</summary>
        /// <param name="queueName">Name of the queue.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        Task<Response<bool>> EnsureCreation(string queueName);

        /// <summary>Gets the message of the specified queue.</summary>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        Task<Response<QueueMessage[]>> Get(string queueName, string userId = "");

        /// <summary>Sets the message to specified queue.</summary>
        /// <typeparam name="TEnt">The type of the ent.</typeparam>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        Task<Response<SendReceipt>> Set<TEnt>(string queueName, TEnt request) where TEnt : BaseEntity;

        /// <summary>Deletes the message from Queue</summary>
        /// <param name="queueNameIn">The queue name in.</param>
        /// <param name="queueMessage">The queue message.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        Task<Response> UnSet(string queueNameIn, QueueMessage queueMessage);
    }
}
