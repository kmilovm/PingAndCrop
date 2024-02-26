using PingAndCrop.Domain.Interfaces;
using PingAndCrop.Objects.Requests;
using PingAndCrop.Objects.Responses;

namespace PingAndCrop.Domain.Services
{
    public class PacRequestService(IHttpClientFactory httpClientFactory) : IPacRequestService
    {
        public async Task<PacResponse> ProcessRequest(PacRequest request)
        {
            using var httpClient = httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri(request.RequestedUrl);
            var response = await httpClient.GetAsync(request.RequestedUrl);
            response.EnsureSuccessStatusCode();
            return new PacResponse()
            {
                RawResponse = await response.Content.ReadAsStringAsync(),
                Error = !response.IsSuccessStatusCode ? await response.Content.ReadAsStringAsync() : string.Empty
            };
        }
    }
}
