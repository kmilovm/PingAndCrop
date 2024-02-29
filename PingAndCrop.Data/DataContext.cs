using Microsoft.EntityFrameworkCore;
using PingAndCrop.Objects.Models.Requests;
using PingAndCrop.Objects.Models.Responses;

namespace PingAndCrop.Data
{
    public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
    {
        public DbSet<PacResponse> Responses { get; set; }
        public DbSet<PacRequest> Requests { get; set; }
    }
}
