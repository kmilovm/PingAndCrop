using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using PingAndCrop.Objects.ViewModels;

namespace PingAndCrop.Domain.Interfaces
{
    public interface ISignalRManager
    {
        Task<SignalRConnectionInfo> RequestConnectionInfo(string userId);

        Task<HttpResponseMessage> SendMessage(string userId, SignalRNotificationVm signalRNotificationVm);

    }
}
