using System.ComponentModel.DataAnnotations.Schema;
using Azure.Storage.Queues.Models;
using PingAndCrop.Objects.Models.Requests;

namespace PingAndCrop.Objects.Models.Responses
{
    public record PacResponse : BaseEntity
    {
        public string? Error { get; set; }
        public string? RawResponse { get; set; }

        public string? CroppedResponse => RawResponse?[..Math.Min(1000, RawResponse.Length)];

        public QueueMessage? Message { get; set; }

        public PacRequest Request { get; set; }
    }
}
