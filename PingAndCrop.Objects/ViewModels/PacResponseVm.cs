using PingAndCrop.Objects.Models;
using PingAndCrop.Objects.Models.Requests;

namespace PingAndCrop.Objects.ViewModels
{
    /// <summary>Version light of Response object</summary>
    public record PacResponseVm : BaseEntity
    {
        /// <summary>Gets or sets the message identifier.</summary>
        /// <value>The message identifier.</value>
        public string Message { get; set; }

        /// <summary>Gets or sets the url requested.</summary>
        /// <value>The request.</value>
        public string Url { get; set; }

        /// <summary>Gets or sets the cropped response.</summary>
        /// <value>The cropped response.</value>
        public string? CroppedResponse { get; set; }

        /// <summary>Gets or sets the raw response.</summary>
        /// <value>The raw response.</value>
        public string? RawResponse { get; set; }

        /// <summary>Gets or sets the error.</summary>
        /// <value>The error.</value>
        public string? Error { get; set; }
    }
}
