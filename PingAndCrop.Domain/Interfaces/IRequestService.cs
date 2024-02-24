using PingAndCrop.Objects;

namespace PingAndCrop.Domain.Interfaces
{
    public interface IRequestService
    {
        Task<Response> ProcessRequest(Request request);
    }
}
