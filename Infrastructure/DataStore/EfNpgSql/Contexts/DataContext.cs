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
        public virtual DbSet<Controller> Controllers { get; set; }

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
                
            // Add a shadow property for specifying the dependent side of the
            // 1 to 1 and configure it to be a foreign key
            modelBuilder.Entity<Environment>()
                .Property<int?>("InhabitantId");
            modelBuilder.Entity<Environment>()
                .HasOne<Pet>(e => e.RealInhabitant)
                .WithOne(p => p.RealEnclosure)
                .HasForeignKey<Environment>("InhabitantId");

            modelBuilder.Entity<Pet>()
                .Ignore(p => p.Species)
                .Ignore(p => p.CareTaker);
            
            // Configure the dependent side of a 1to1 using a shadow foreign key.
            modelBuilder.Entity<Pet>()
                .Property<long?>("LatestSampleId");
            modelBuilder.Entity<Pet>()
                .HasOne(p => p.LatestConcreteSample)
                .WithOne()
                .HasForeignKey<Pet>("LatestSampleId");

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
            
            // Set EF Core to utilize EnvDataSample's Captured property setter when materializing the entity
            modelBuilder.Entity<EnvDataSample>()
                .Property(eds => eds.Captured)
                .UsePropertyAccessMode(PropertyAccessMode.Property);

            // Ensure navigation property EnvDataSample.Occupant, which may not be caught implicitly.
            //modelBuilder.Entity<EnvDataSample>()
            //    .HasOne(eds => eds.Occupant);
        }
    }
}
