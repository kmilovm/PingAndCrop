using AutoFixture;
using Moq;
using PingAndCrop.Domain.Services;
using PingAndCrop.Objects;

namespace PingAndCrop.Tests
{
    public class RequestServiceTests(RequestService requestService)
    {   

        [Theory]
        [InlineData("http://www.google.com")]
        public async Task RequestServiceSuccess(string url)
        {
            var request = new Request
            {
                RequestedUrl = url
            };
            var answer = await requestService.ProcessRequest(request);
            Assert.NotNull(answer.RawResponse);
            Assert.NotEmpty(answer.RawResponse);
        }
    }
}