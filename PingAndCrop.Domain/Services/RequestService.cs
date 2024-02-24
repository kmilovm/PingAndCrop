using PingAndCrop.Domain.Interfaces;
using PingAndCrop.Objects;

namespace PingAndCrop.Domain.Services
{
    public class RequestService(HttpClient httpClient) : IRequestService
    {
        public async Task<Response> ProcessRequest(Request request)
        {
            httpClient.BaseAddress = new Uri(request.RequestedUrl);
            var response = await httpClient.GetAsync(request.RequestedUrl);
            response.EnsureSuccessStatusCode();
            return new Response()
            {
                RawResponse = await response.Content.ReadAsStringAsync(),
                Error = !response.IsSuccessStatusCode ? await response.Content.ReadAsStringAsync() : string.Empty
            };
        }
    }
}
