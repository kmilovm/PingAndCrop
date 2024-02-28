using PingAndCrop.Objects.Models;
using PingAndCrop.Objects.Models.Requests;

namespace PingAndCrop.Objects.ViewModels
{
    public record PacResponseVm : BaseEntity
    {
        public string? Error { get; set; }
        public string? RawResponse { get; set; }

        public string? CroppedResponse { get; set; }

        public string MessageId { get; set; }
        
        public PacRequest Request { get; set; }
    }
}
