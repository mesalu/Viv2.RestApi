using System;
using System.ComponentModel.DataAnnotations.Schema;
using Viv2.API.Core.ProtoEntities;

#nullable enable

namespace Viv2.API.Infrastructure.DataStore.EfNpgSql.Entities
{
    public class EnvDataSample : IEnvDataSample
    {
        private ILazyLoader LazyLoader;
        private Pet? _realOccupant;
        public EnvDataSample() {}

        public EnvDataSample(ILazyLoader lazyLoader)
        {
            LazyLoader = lazyLoader;
        }
        
        private DateTime? _captured;
        
        // Properties discoverable by EF Core that utilize concrete classes to define relations.
        public long Id { get; set; }
        public DateTime? Captured
        {
            // NOTE: this property has been specially configured in DataContext to be used when materializing
            //       otherwise we wouldn't be able to rely on the setter to achieve the desired effect. 
            get => _captured;
            set
            {
                // Convert unspecified / naive date time instances to UTC.
                if (value.HasValue && value.Value.Kind == DateTimeKind.Unspecified)
                    _captured = DateTime.SpecifyKind(value.Value, DateTimeKind.Utc);
            }
        }
        public Environment? RealEnvironment { get; set; }
        public Pet? RealOccupant 
        { 
            get => LazyLoader?.Load(this, ref _realOccupant);
            set => _realOccupant = value;
        }
        public double HotGlass { get; set; }
        public double HotMat { get; set; }
        public double MidGlass { get; set; }
        public double ColdGlass { get; set; }
        public double ColdMat { get; set; }
        
        // All the hidden abstraction implementations (fulfillment of the Core interface that don't play
        // too nicely with EF Core.) 
        [NotMapped]
        public IEnvironment? Environment 
        { 
            get => RealEnvironment;
            set
            {
                if (value == null) RealEnvironment = null;
                else if (value is Environment env) RealEnvironment = env;
                else throw new ArgumentException("Mismatched infrastructure data store backings");
            } 
        }

        [NotMapped]
        public IPet? Occupant
        {
            get => RealOccupant;
            set
            {
                if (value == null) RealOccupant = null;
                else if (value is Pet pet) RealOccupant = pet;
                else throw new ArgumentException("Mismatched infrastructure data store backings");
            }
        }
    }
}