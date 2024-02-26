using PingAndCrop.Domain.Services;
using PingAndCrop.Objects.Requests;

namespace PingAndCrop.Tests
{
    public class RequestServiceTests(PacRequestService requestService)
    {   

        [Theory]
        [InlineData("http://www.google.com")]
        public async Task RequestServiceSuccess(string url)
        {
            var request = new PacRequest
            {
                RequestedUrl = url
            };
            var answer = await requestService.ProcessRequest(request);
            Assert.NotNull(answer.RawResponse);
            Assert.NotEmpty(answer.RawResponse);
        }
    }
}