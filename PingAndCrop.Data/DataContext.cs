using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PingAndCrop.Objects.Models.Requests;
using PingAndCrop.Objects.Models.Responses;

namespace PingAndCrop.Data
{
    /// <summary>DataContext for the application</summary>
    public class DataContext(DbContextOptions<DataContext> options, ILoggerFactory loggerFactory) : DbContext(options)
    {
        /// <summary>Gets or sets the responses.</summary>
        /// <value>The responses.</value>
        public DbSet<PacResponse> Responses { get; set; }
        /// <summary>Gets or sets the requests.</summary>
        /// <value>The requests.</value>
        public DbSet<PacRequest> Requests { get; set; }

        /// <summary>
        /// Override this method to configure the database (and other options) to be used for this context.
        /// This method is called for each instance of the context that is created.
        /// The base implementation does nothing.
        /// </summary>
        /// <param name="optionsBuilder">
        /// A builder used to create or modify options for this context. Databases (and other extensions)
        /// typically define extension methods on this object that allow you to configure the context.
        /// </param>
        /// <remarks>
        ///   <para>
        /// In situations where an instance of <see cref="T:Microsoft.EntityFrameworkCore.DbContextOptions">DbContextOptions</see> may or may not have been passed
        /// to the constructor, you can use <see cref="P:Microsoft.EntityFrameworkCore.DbContextOptionsBuilder.IsConfigured">IsConfigured</see> to determine if
        /// the options have already been set, and skip some or all of the logic in
        /// <see cref="M:Microsoft.EntityFrameworkCore.DbContext.OnConfiguring(Microsoft.EntityFrameworkCore.DbContextOptionsBuilder)">OnConfiguring(Microsoft.EntityFrameworkCore.DbContextOptionsBuilder)</see>.
        /// </para>
        ///   <para>
        /// See <a href="https://aka.ms/efcore-docs-dbcontext">DbContext lifetime, configuration, and initialization</a>
        /// for more information and examples.
        /// </para>
        /// </remarks>
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
