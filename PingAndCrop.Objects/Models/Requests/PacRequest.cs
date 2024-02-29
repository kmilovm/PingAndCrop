using FluentValidation;

namespace PingAndCrop.Objects.Models.Requests
{
    public record PacRequest : BaseEntity
    {

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
            RuleFor(req => req.RequestedUrl).NotNull().NotEmpty().Must(BeAValidUri).WithMessage("Requested url property is needed to start the process!");
            
        }
        private bool BeAValidUri(string uri)
        {
            return Uri.TryCreate(uri, UriKind.Absolute, out _);
        }
    }
}
