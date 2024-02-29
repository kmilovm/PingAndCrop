namespace PingAndCrop.Domain.Interfaces
{
    /// <summary>Base service for management service</summary>
    public interface IManagementBaseService
    {
        /// <summary>Gets the and process messages.</summary>
        /// <param name="queueIn">The queue in.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        Task GetAndProcessMessages(string? queueIn);
    }
}
