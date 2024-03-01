using Newtonsoft.Json;
using PingAndCrop.Domain.Interfaces;
using PingAndCrop.Objects.Models.Requests;
using PingAndCrop.Objects.Models.Responses;

namespace PingAndCrop.Domain.Services
{
    /// <summary>Service created for handling the requests of the TimedHostedService</summary>
    public class ManagementService(IHttpClientFactory httpClientFactory, IEntityService entityService)
        : IManagementBaseService
    {
        /// <inheritdoc />
        public async Task GetAndProcessMessages(string? queueIn)
        {
            var responses = await entityService.GetAll<PacRequest>();
            await ProcessRequests(responses);
        }

        private async Task ProcessRequests(IEnumerable<PacRequest> pacRequests)
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
                    Url = request.RequestedUrl,
                    RawResponse = await ReadStreamAndConvert(response),
                    Message = JsonConvert.SerializeObject(request),
                    Timestamp = DateTimeOffset.UtcNow,
                    PartitionKey = Guid.NewGuid().ToString(),
                    RowKey = Guid.NewGuid().ToString(),
                    Error = !response.IsSuccessStatusCode ? await response.Content.ReadAsStringAsync() : string.Empty,
                };
                pacResponses.Add(pacResponse);
            }

            await RemoveRequests(pacRequests);
            await StoreResponses(pacResponses);
        }

        private static async Task<string> ReadStreamAndConvert(HttpResponseMessage response)
        {
            var result = await response.Content.ReadAsStringAsync();
            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(result);
            var headNode = doc.DocumentNode.SelectSingleNode("/html/head").InnerHtml;
            return headNode;
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
