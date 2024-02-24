namespace PingAndCrop.Objects
{
    public record Response
    {
        public string? Error { get; set; }
        public string? RawResponse { get; set; }

        public string? CroppedResponse => RawResponse?[..Math.Min(1000, RawResponse.Length)];
    }
}
