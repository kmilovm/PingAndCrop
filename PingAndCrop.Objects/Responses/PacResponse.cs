namespace PingAndCrop.Objects.Responses
{
    public record PacResponse
    {
        public string? Error { get; set; }
        public string? RawResponse { get; set; }

        public string? CroppedResponse => RawResponse?[..Math.Min(1000, RawResponse.Length)];
    }
}
