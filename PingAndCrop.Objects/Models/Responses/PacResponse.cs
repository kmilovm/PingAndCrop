namespace PingAndCrop.Objects.Models.Responses
{
    public record PacResponse : BaseEntity
    {
        public string? Error { get; set; }
        public string? RawResponse { get; set; }

        public string? CroppedResponse => RawResponse?[..Math.Min(1000, RawResponse.Length)];

        public string? Message { get; set; }
    }
}
