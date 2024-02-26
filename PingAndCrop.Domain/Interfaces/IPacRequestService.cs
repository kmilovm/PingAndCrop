using PingAndCrop.Objects.Requests;
using PingAndCrop.Objects.Responses;

namespace PingAndCrop.Domain.Interfaces
{
    public interface IPacRequestService
    {
        Task<PacResponse> ProcessRequest(PacRequest request);
    }
}
