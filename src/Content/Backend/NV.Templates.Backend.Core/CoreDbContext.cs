using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace NV.Templates.Backend.Core
{
    /// <summary>
    /// The <see cref="DbContext"/> for the application.
    /// </summary>
    internal class CoreDbContext : DbContext
    {
        public CoreDbContext(DbContextOptions<CoreDbContext> options)
            : base(options)
        {
        }

        /// <inheritdoc />
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            // Disable client evaluation.
            optionsBuilder
                .ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning));
        }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Enable automatic loading of IEntityTypeConfiguration and IQueryTypeConfiguration in this assembly.
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CoreDbContext).Assembly);
        }
    }
}
