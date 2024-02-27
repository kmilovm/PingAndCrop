using Azure.Storage.Queues.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PingAndCrop.Domain.Interfaces;
using PingAndCrop.Objects.Requests;
using PingAndCrop.Objects.Responses;

namespace PingAndCrop.Domain.Services
{
    public class PacRequestService : IPacRequestService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _config;
        private readonly IQueueService _queueService;
        
        public PacRequestService(IHttpClientFactory httpClientFactory, IConfiguration config, IQueueService queueService)
        {
            _httpClientFactory = httpClientFactory;
            _config = config;
            _queueService = queueService;
        }

        public async Task StoreResponses(IList<PacResponse> responses)
        {
            var queueIn = _config["QueueNameIn"]; 
            var queueOut = _config["QueueNameOut"];
            foreach (var pacResponse in responses)
            {
                await _queueService.DequeueMessage(queueIn!, queueOut!, pacResponse.Message!);
                await _queueService.EnqueueMessage(queueOut, pacResponse);
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
                httpClient.BaseAddress = new Uri(request!.RequestedUrl);
                var response = await httpClient.GetAsync(request.RequestedUrl);
                response.EnsureSuccessStatusCode();
                var pacResponse = new PacResponse()
                {
                    Message = requestsQueueMessage,
                    RawResponse = await response.Content.ReadAsStringAsync(),
                    Error = !response.IsSuccessStatusCode ? await response.Content.ReadAsStringAsync() : string.Empty,
                    Request = request
                };
                pacResponses.Add(pacResponse);
            }
            return pacResponses;
        }
    }
}
