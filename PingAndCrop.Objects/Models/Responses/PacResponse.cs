namespace PingAndCrop.Objects.Models.Responses
{
    /// <summary>Object for storing the response obtained from the processing</summary>
    public record PacResponse : BaseEntity
    {
        /// <summary>Gets or sets the error.</summary>
        /// <value>The error.</value>
        public string? Error { get; set; }
        /// <summary>Gets or sets the raw response.</summary>
        /// <value>The raw response.</value>
        public string? RawResponse { get; set; }

        /// <summary>Gets the cropped response.</summary>
        /// <value>The cropped response.</value>
        public string? CroppedResponse => RawResponse?[..Math.Min(1000, RawResponse.Length)];

        /// <summary>Gets or sets the message.</summary>
        /// <value>The message.</value>
        public string? Message { get; set; }
    }
}
