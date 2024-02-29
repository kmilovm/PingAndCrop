using Azure;
using Azure.Data.Tables;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace PingAndCrop.Objects.Models
{
    public record BaseEntity : ITableEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; } = DateTimeOffset.UtcNow;
        [NotMapped]
        [JsonIgnore]
        public ETag ETag { get; set; }
    }
}
