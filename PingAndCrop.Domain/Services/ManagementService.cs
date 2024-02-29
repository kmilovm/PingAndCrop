using Azure.Storage.Queues.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PingAndCrop.Domain.Constants;
using PingAndCrop.Domain.Interfaces;
using PingAndCrop.Objects.Models.Requests;
using PingAndCrop.Objects.Models.Responses;

namespace PingAndCrop.Domain.Services
{
    public class ManagementService : IManagementBaseService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITableService _queueService;
        private readonly string _queueIn;
        private readonly string _queueOut;

        public ManagementService(IHttpClientFactory httpClientFactory, IConfiguration configuration, ITableService queueService)
        {
            _httpClientFactory = httpClientFactory;
            _queueService = queueService;
            var config = configuration ?? throw new ArgumentException(StringMessages.NoConfigFound);
            _queueIn = config["QueueNameIn"] ?? throw new ArgumentException(StringMessages.NoQueueFoundAtConfig);
            _queueOut = config["QueueNameOut"] ?? throw new ArgumentException(StringMessages.NoQueueFoundAtConfig);
        }

        public async Task GetAndProcessMessages(string? queueIn)
        {
            var responses = new List<PacResponse>();
            var messages = await _queueService.Get<PacResponse>(queueIn!);
            await foreach (var page in messages.AsPages())
            {
                responses.AddRange(page.Values);
            }
            await StoreResponses(responses);
        }

        private async Task<IEnumerable<PacResponse>> ProcessRequests(IEnumerable<QueueMessage> messages)
        {
            var pacResponses = new List<PacResponse>();
            using var httpClient = _httpClientFactory.CreateClient();
            foreach (var requestsQueueMessage in messages)
            {
                var request = JsonConvert.DeserializeObject<PacRequest>(requestsQueueMessage.MessageText);
                if (request == null) continue;
                if (!Uri.IsWellFormedUriString(request!.RequestedUrl, UriKind.Absolute)) continue;
                httpClient.BaseAddress = new Uri(request!.RequestedUrl);
                var response = await httpClient.GetAsync(request.RequestedUrl);
                response.EnsureSuccessStatusCode();
                var pacResponse = new PacResponse()
                {
                    RawResponse = await response.Content.ReadAsStringAsync(),
                    Error = !response.IsSuccessStatusCode ? await response.Content.ReadAsStringAsync() : string.Empty,
                    Request = request,
                    PartitionKey = Guid.NewGuid().ToString(),
                    RowKey = Guid.NewGuid().ToString()
                };
                pacResponses.Add(pacResponse);
                await _queueService.UnSet(_queueIn, requestsQueueMessage);
            }
            return pacResponses;
        }

        private async Task StoreResponses(IEnumerable<PacResponse> responses)
        {
            foreach (var pacResponse in responses)
            {
                await _queueService.Set(_queueOut, pacResponse);
            }
        }
    }
}
