using FluentValidation;

namespace PingAndCrop.Objects
{
    public class Request
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required string RequestedUrl { get; set; }
        public Response? Response { get; set; }
    }

    public class RequestValidator : AbstractValidator<Request>
    {
        /// <summary>
        /// Public constructor
        /// </summary>
        public RequestValidator()
        {
            RuleFor(req => req.RequestedUrl).NotNull().NotEmpty();
        }
    }
}
