using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PingAndCrop.Objects.Models.Requests;
using PingAndCrop.Objects.Models.Responses;

namespace PingAndCrop.Data
{
    public class DataContext(DbContextOptions<DataContext> options, ILoggerFactory loggerFactory) : DbContext(options)
    {
        public DbSet<PacResponse> Responses { get; set; }
        public DbSet<PacRequest> Requests { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
#if DEBUG
            optionsBuilder.UseLoggerFactory(loggerFactory)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors();
#endif
            base.OnConfiguring(optionsBuilder);
        }
    }
}
