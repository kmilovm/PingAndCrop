using FluentValidation;

namespace PingAndCrop.Objects.Requests
{
    public class PacRequest
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string UserId { get; set; }
        public required string RequestedUrl { get; set; }
    }

    public class PacRequestValidator : AbstractValidator<PacRequest>
    {
        /// <summary>
        /// Public constructor
        /// </summary>
        public PacRequestValidator()
        {
            RuleFor(req => req.RequestedUrl).NotNull().NotEmpty().WithMessage("Requested url property is needed to start the process!");
        }
    }
}
