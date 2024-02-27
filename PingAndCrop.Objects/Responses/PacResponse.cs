using PingAndCrop.Objects.Requests;

namespace PingAndCrop.Objects.Responses
{
    public record PacResponse
    {
        public string UserId { get; set; }
        public string? Error { get; set; }
        public string? RawResponse { get; set; }

        public string? CroppedResponse => RawResponse?[..Math.Min(1000, RawResponse.Length)];
        
        public PacRequest Request { get; set; }
    }
}
