using PingAndCrop.Objects.Models;

namespace PingAndCrop.Objects.ViewModels
{
    public record PacRequestVm : BaseEntity
    {
        public string UserId { get; set; }
        public required string RequestedUrl { get; set; }
    }
}
