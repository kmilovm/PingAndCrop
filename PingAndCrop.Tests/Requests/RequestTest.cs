using PingAndCrop.Objects.Models.Requests;

namespace PingAndCrop.Tests.Requests
{
    public class RequestTest()
    {
        [Theory]
        [Trait("Category", "UnitTest")]
        [InlineData("www.google.com")]
        
        public void NotPassedUrlValidation(string url)
        {
            // Arrange
            var fixture = new Fixture();
            var validator = new PacRequestValidator();
            var request = fixture.Create<PacRequest>();
            request.RequestedUrl = url;
            
            // Act
            var result = validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.RequestedUrl);
        }

        [Theory]
        [Trait("Category", "UnitTest")]
        [InlineData("http://www.google.com")]

        public void PassedUrlValidation(string url)
        {
            // Arrange
            var fixture = new Fixture();
            var validator = new PacRequestValidator();
            var request = fixture.Create<PacRequest>();
            request.RequestedUrl = url;

            // Act
            var result = validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.RequestedUrl);
        }
    }
}
