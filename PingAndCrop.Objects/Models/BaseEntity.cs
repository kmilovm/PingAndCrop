using Azure;
using Azure.Data.Tables;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace PingAndCrop.Objects.Models
{
    /// <summary>Record for provide a base class with common properties</summary>
    public record BaseEntity : ITableEntity
    {
        /// <summary>Gets or sets the identifier.</summary>
        /// <value>The identifier.</value>
        public Guid Id { get; set; } = Guid.NewGuid();
        /// <summary>Gets or sets the partition key.</summary>
        /// <value>The partition key.</value>
        public string PartitionKey { get; set; }
        /// <summary>Gets or sets the row key.</summary>
        /// <value>The row key.</value>
        public string RowKey { get; set; }
        /// <summary>Gets or sets the timestamp.</summary>
        /// <value>The timestamp.</value>
        public DateTimeOffset? Timestamp { get; set; } = DateTimeOffset.UtcNow;
        /// <summary>Gets or sets the e tag.</summary>
        /// <value>The e tag.</value>
        [NotMapped]
        [JsonIgnore]
        public ETag ETag { get; set; }
    }
}
