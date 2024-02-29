namespace PingAndCrop.Domain.Interfaces
{
    /// <summary>
    ///   <para>Interface for handing the UtcNow date manipulation</para>
    /// </summary>
    public interface IDateOffSetUtc
    {
        /// <summary>Gets the UTC now.</summary>
        /// <value>The UTC now.</value>
        DateTime UtcNow { get; }
    }
}
