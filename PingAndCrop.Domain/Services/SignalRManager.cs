using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PingAndCrop.Domain.Interfaces;
using PingAndCrop.Objects.ViewModels;
using System.Net;
using System.Text;

namespace PingAndCrop.Domain.Services
{
    public class SignalRManager : ISignalRManager
    {
        private readonly IConfiguration _configuration;

        public SignalRManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<SignalRConnectionInfo> RequestConnectionInfo(string userId)
        {
            var azureNegociateUrl = $"{_configuration.GetValue<string>("AzureSignalR:Negociate")}?code={_configuration.GetValue<string>("AzureSignalR:Code")}";
            using var client = new HttpClient();
            using var request = new HttpRequestMessage(HttpMethod.Post, azureNegociateUrl);
            if (string.IsNullOrEmpty(userId)) throw new ArgumentNullException(nameof(userId));
            //request.Headers.Add("x-ms-client-principal-id", userId);
            var httpResponseMessage = await client.SendAsync(request);
            httpResponseMessage.EnsureSuccessStatusCode();
            if (!httpResponseMessage.IsSuccessStatusCode) return null;
            var result = httpResponseMessage.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<SignalRConnectionInfo>(result);
        }

        public async Task<HttpResponseMessage> SendMessage(string userId, SignalRNotificationVm signalRNotificationVm)
        {
            var requestConnectionInfo = await RequestConnectionInfo(userId);
            if (requestConnectionInfo == default) return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            var azureSendMessageUrl = $"{_configuration.GetValue<string>("AzureSignalR:SendMessage")}?code={_configuration.GetValue<string>("AzureSignalR:Code")}";
            using var client = new HttpClient();
            using var request = new HttpRequestMessage(HttpMethod.Post, azureSendMessageUrl);
            //request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", requestConnectionInfo.AccessToken);
            var signalRNotificationJson = JsonConvert.SerializeObject(signalRNotificationVm);
            request.Content = new StringContent(signalRNotificationJson, Encoding.UTF8, "application/json");
            var httpResponseMessage = await client.SendAsync(request);
            httpResponseMessage.EnsureSuccessStatusCode();
            return httpResponseMessage;
        }
    }
}
