using FluentValidation;

namespace PingAndCrop.Objects.Models.Requests
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="PingAndCrop.Objects.Models.BaseEntity" />
    /// <seealso cref="Azure.Data.Tables.ITableEntity" />
    /// <seealso cref="System.IEquatable&lt;PingAndCrop.Objects.Models.BaseEntity&gt;" />
    /// <seealso cref="System.IEquatable&lt;PingAndCrop.Objects.Models.Requests.PacRequest&gt;" />
    public record PacRequest : BaseEntity
    {

        /// <summary>Gets or sets the user identifier.</summary>
        /// <value>The user identifier.</value>
        public string UserId { get; set; }
        /// <summary>
        /// Gets or sets the requested URL.
        /// </summary>
        /// <value>
        /// The requested URL.
        /// </value>
        public required string RequestedUrl { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="FluentValidation.AbstractValidator&lt;PingAndCrop.Objects.Models.Requests.PacRequest&gt;" />
    public class PacRequestValidator : AbstractValidator<PacRequest>
    {
        /// <summary>
        /// Public constructor
        /// </summary>
        public PacRequestValidator()
        {
            RuleFor(req => req.RequestedUrl).NotNull().NotEmpty().Must(BeAValidUri).WithMessage("Requested url property is needed to start the process!");
            
        }
        /// <summary>
        /// validates if the url is valid.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns></returns>
        private bool BeAValidUri(string uri)
        {
            return Uri.TryCreate(uri, UriKind.Absolute, out _);
        }
    }
}
