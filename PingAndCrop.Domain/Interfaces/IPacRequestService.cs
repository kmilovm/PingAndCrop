using Azure.Storage.Queues.Models;
using PingAndCrop.Objects.Responses;

namespace PingAndCrop.Domain.Interfaces
{
    public interface IPacRequestService
    {
        Task<IEnumerable<PacResponse>> ProcessRequests(QueueMessage[] messages);
        Task StoreResponses(IList<PacResponse> responses);
    }
}
