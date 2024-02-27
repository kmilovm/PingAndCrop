using Azure.Storage.Queues.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PingAndCrop.Domain.Constants;
using PingAndCrop.Domain.Interfaces;
using PingAndCrop.Objects.Requests;
using PingAndCrop.Objects.Responses;

namespace PingAndCrop.Domain.Services
{
    public class PacRequestService : IPacRequestService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IQueueService _queueService;
        private readonly string _queueIn;
        private readonly string _queueOut;

        public PacRequestService(IHttpClientFactory httpClientFactory, IConfiguration configuration, IQueueService queueService)
        {
            _httpClientFactory = httpClientFactory;
            _queueService = queueService;
            var config = configuration ?? throw new ArgumentException(StringMessages.NoConfigFound);
            _queueIn = config["QueueNameIn"] ?? throw new ArgumentException(StringMessages.NoQueueFoundAtConfig);
            _queueOut = config["QueueNameOut"] ?? throw new ArgumentException(StringMessages.NoQueueFoundAtConfig);
        }

        public async Task StoreResponses(IList<PacResponse> responses)
        {
            foreach (var pacResponse in responses)
            {
                await _queueService.EnqueueMessage(_queueOut, pacResponse);
            }
        }

        public async Task<IEnumerable<PacResponse>> ProcessRequests(QueueMessage[] messages)
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
                    Request = request
                };
                pacResponses.Add(pacResponse);
                await _queueService.DequeueMessage(_queueIn, requestsQueueMessage);
            }
            return pacResponses;
        }
    }
}
