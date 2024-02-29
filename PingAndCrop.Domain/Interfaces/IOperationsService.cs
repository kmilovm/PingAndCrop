using Azure.Storage.Queues.Models;

namespace PingAndCrop.Domain.Interfaces
{
    /// <summary>Interface to ease change provider in a near future [Pending Feature]</summary>
    /// <typeparam name="TResB">The type of the resource b.</typeparam>
    /// <typeparam name="TResQ">The type of the resource q.</typeparam>
    /// <typeparam name="TResR">The type of the resource r.</typeparam>
    /// <typeparam name="TRes">The type of the resource.</typeparam>
    public interface IOperationsService<TResB, TResQ, TResR, TRes>
    {
        /// <summary>Ensures the creation of message/entity</summary>
        /// <param name="queueName">Name of the queue.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        Task<TResB> EnsureCreation(string queueName);

        /// <summary>Gets the message/entity from specified Queue/Entity</summary>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        Task<TResQ> Get(string queueName, string userId = "");

        /// <summary>Sets the specified message/entity into Queue/Entity</summary>
        /// <typeparam name="TEnt">The type of the ent.</typeparam>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        Task<TResR> Set<TEnt>(string queueName, TEnt request) where TEnt : class;

        /// <summary>Deletes the message/entity from the Queue/Entity</summary>
        /// <param name="queueNameIn">The queue name in.</param>
        /// <param name="queueMessage">The queue message.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        Task<TRes> UnSet(string queueNameIn, QueueMessage queueMessage);
    }
}
