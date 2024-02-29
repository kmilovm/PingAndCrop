using PingAndCrop.Objects.Models;

namespace PingAndCrop.Objects.ViewModels
{
    /// <summary>
    /// Request entity version light
    /// </summary>
    /// <seealso cref="PingAndCrop.Objects.Models.BaseEntity" />
    /// <seealso cref="Azure.Data.Tables.ITableEntity" />
    /// <seealso cref="System.IEquatable&lt;PingAndCrop.Objects.Models.BaseEntity&gt;" />
    /// <seealso cref="System.IEquatable&lt;PingAndCrop.Objects.ViewModels.PacRequestVm&gt;" />
    public record PacRequestVm : BaseEntity
    {
        /// <summary>Gets or sets the user identifier.</summary>
        /// <value>The user identifier.</value>
        public string UserId { get; set; }
        /// <summary>
        /// Gets or sets the requested URL.
        /// </summary>
        /// <value>
        /// The requested URL.
        /// </value>
        public required string RequestedUrl { get; set; }
    }
}
