using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Viv2.API.Infrastructure.DataStore.EfNpgSql.Entities;

using Environment = Viv2.API.Infrastructure.DataStore.EfNpgSql.Entities.Environment;

/*
 * Design-time notes:
 * - DataContextDesignFactory exists for use with `dotnet ef` toolsets.
 * - The migrations directory does not match the default case, use '-o', '--output-dir' to specify
 *      the correct path
 * - As specified by DataContextDesignFactory, use the named environment variable to make invoking the dotnet ef tools
 *      simpler.
 */

namespace Viv2.API.Infrastructure.DataStore.EfNpgSql.Contexts
{
    public partial class DataContext : IdentityDbContext<User>
    {
        //public virtual DbSet<Controller> Controllers { get; set; }
        public virtual DbSet<Environment> Environments { get; set; }
        public virtual DbSet<EnvDataSample> EnvDataSamples { get; set; }
        public virtual DbSet<Pet> Pets { get; set; }
        public virtual DbSet<Species> Species { get; set; }

        public DataContext(DbContextOptions<DataContext> options)
            : base(options) { }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasAnnotation("Relational:Collation", "en_US.UTF-8");

            // Ignore Core-compat properties.
            modelBuilder.Entity<User>()
                .Ignore(u => u.Pets)
                .Ignore(u => u.Environments)
                .Ignore(u => u.Guid) // use Id instead
                .HasKey(u => u.Id);

            modelBuilder.Entity<User>()
                .HasMany<Pet>(u => u.BackedPets)
                .WithOne(p => p.RealCareTaker);

            modelBuilder.Entity<Environment>()
                .Ignore(e => e.Controller)
                .Ignore(e => e.Inhabitant)
                .Ignore(e => e.EnvDataSamples)
                .Ignore(e => e.Users);

            modelBuilder.Entity<Pet>()
                .Ignore(p => p.Species)
                .Ignore(p => p.CareTaker);

            modelBuilder.Entity<Species>()
                .Ignore(s => s.Pets);

            modelBuilder.Entity<Controller>()
                .Ignore(c => c.Environments);

            // Add default values for "captured" columns. (e.g. if not provided, default to now)
            modelBuilder.Entity<EnvDataSample>()
                .Ignore(sample => sample.Occupant)
                .Ignore(sample => sample.Environment)
                .Property(e => e.Captured)
                .HasDefaultValueSql("(NOW() AT TIME ZONE 'utc')");

            // Ensure navigation property EnvDataSample.Occupant, which may not be caught implicitly.
            //modelBuilder.Entity<EnvDataSample>()
            //    .HasOne(eds => eds.Occupant);
        }
    }
}
