using Newtonsoft.Json;
using PingAndCrop.Domain.Interfaces;
using PingAndCrop.Objects.Models.Requests;
using PingAndCrop.Objects.Models.Responses;

namespace PingAndCrop.Domain.Services
{
    public class ManagementService(IHttpClientFactory httpClientFactory, IEntityService entityService)
        : IManagementBaseService
    {
        public async Task GetAndProcessMessages(string? queueIn)
        {
            var responses = await entityService.GetAll<PacRequest>();
            await ProcessRequests(responses);
        }

        private async Task<IEnumerable<PacResponse>> ProcessRequests(IEnumerable<PacRequest> pacRequests)
        {
            var pacResponses = new List<PacResponse>();

            foreach (var request in pacRequests)
            {
                if (!Uri.IsWellFormedUriString(request!.RequestedUrl, UriKind.Absolute)) continue;
                using var httpClient = httpClientFactory.CreateClient();
                httpClient.Timeout = TimeSpan.FromSeconds(30);
                httpClient.BaseAddress = new Uri(request!.RequestedUrl);
                var response = await httpClient.GetAsync(request.RequestedUrl);
                response.EnsureSuccessStatusCode();
                var pacResponse = new PacResponse()
                {
                    RawResponse = await response.Content.ReadAsStringAsync(),
                    Error = !response.IsSuccessStatusCode ? await response.Content.ReadAsStringAsync() : string.Empty,
                    Message = JsonConvert.SerializeObject(request),
                    Timestamp = DateTimeOffset.UtcNow,
                    PartitionKey = Guid.NewGuid().ToString(),
                    RowKey = Guid.NewGuid().ToString()
                };
                pacResponses.Add(pacResponse);
            }

            await RemoveRequests(pacRequests);
            await StoreResponses(pacResponses);
            return pacResponses;
        }

        private async Task StoreResponses(IEnumerable<PacResponse> responses)
        {
            foreach (var pacResponse in responses)
            {
                await entityService.Set(pacResponse);
            }
        }

        private async Task RemoveRequests(IEnumerable<PacRequest> requests)
        {
            foreach (var pacRequest in requests)
            {
                await entityService.UnSet(pacRequest);
            }
        }
    }
}
