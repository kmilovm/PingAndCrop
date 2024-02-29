namespace PingAndCrop.Domain.Interfaces
{
    public interface IManagementBaseService
    {
        Task GetAndProcessMessages(string? queueIn);
    }
}
