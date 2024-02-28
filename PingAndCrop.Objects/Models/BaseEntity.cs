namespace PingAndCrop.Objects.Models
{
    public record BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
    }
}
