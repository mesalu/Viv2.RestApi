using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Viv2.API.Core.Entities;
using Viv2.API.Infrastructure.DataStore.EfNpgSql.Entities;

namespace Viv2.API.Infrastructure.DataStore.EfNpgSql.Contexts
{
    public partial class DataContext : IdentityDbContext<BackedUser>
    {
        public virtual DbSet<Controller> Controllers { get; set; }
        public virtual DbSet<EnvDataSample> EnvDataSamples { get; set; }
        public virtual DbSet<Pet> Pets { get; set; }
        public virtual DbSet<Species> Species { get; set; }

        public DataContext(DbContextOptions<DataContext> options)
            : base(options) { }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasAnnotation("Relational:Collation", "en_US.UTF-8");

            // Add default values for "captured" columns. (e.g. if not provided, default to now)
            modelBuilder.Entity<EnvDataSample>()
                .Property(e => e.Captured)
                .HasDefaultValueSql("(NOW() AT TIME ZONE 'utc')");
        }
    }
}
