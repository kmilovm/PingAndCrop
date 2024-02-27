using PingAndCrop.Domain.Interfaces;
using PingAndCrop.Objects.Requests;
using PingAndCrop.Objects.Responses;
using PingAndCrop.Objects.ViewModels;

namespace PingAndCrop.Domain.Services
{
    public class PacRequestService : IPacRequestService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ISignalRManager _signalRManager;

        public PacRequestService(IHttpClientFactory httpClientFactory, ISignalRManager signalRManager)
        {
            _httpClientFactory = httpClientFactory;
            _signalRManager = signalRManager;
        }

        public async Task NotifyResponses(IList<PacResponse> responses)
        {
            foreach (var pacResponse in responses)
            {
                await _signalRManager.SendMessage(pacResponse.UserId, new SignalRNotificationVm()
                {
                    response = pacResponse
                });
            }
        }

        public async Task<PacResponse> ProcessRequest(PacRequest request)
        {
            using var httpClient = _httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri(request.RequestedUrl);
            var response = await httpClient.GetAsync(request.RequestedUrl);
            response.EnsureSuccessStatusCode();
            return new PacResponse()
            {
                RawResponse = await response.Content.ReadAsStringAsync(),
                Error = !response.IsSuccessStatusCode ? await response.Content.ReadAsStringAsync() : string.Empty,
                Request = request
            };
        }
    }
}
