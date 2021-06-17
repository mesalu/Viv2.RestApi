using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Viv2.API.Core.ProtoEntities;

#nullable enable

namespace Viv2.API.Infrastructure.DataStore.EfNpgSql.Entities
{
    public class EnvDataSample : IEnvDataSample
    {
        private Pet? _realOccupant;

        public EnvDataSample()
        {
            _realOccupant = null;
        }
        public EnvDataSample(ILazyLoader lazyLoader)
        {
            LazyLoader = lazyLoader;
        }
        private ILazyLoader? LazyLoader { get; set; }

        
        public int Id { get; set; }

        // What Core accesses
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
        
        // what EF needs.
        public Environment? RealEnvironment { get; set; }
        
        // what Core needs
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

        // what EF needs
        public Pet? RealOccupant { get => LazyLoader?.Load(this, ref _realOccupant); set => _realOccupant = value; }

        private DateTime? _captured;
        public DateTime? Captured
        {
            get
            {
                if (_captured.HasValue && _captured.Value.Kind == DateTimeKind.Unspecified)
                    // Part of Core's spec to Infrastructure providers is that all date time values are UTC.
                    // NpgSql is somewhat timezone aware but it appears that postgres is still treating datetime
                    // objects naively, so correct for that here.
                    return DateTime.SpecifyKind(_captured.Value, DateTimeKind.Utc);
                
                return _captured;
            }
            set => _captured = value;
        }
        
        public double HotGlass { get; set; }
        public double HotMat { get; set; }
        public double MidGlass { get; set; }
        public double ColdGlass { get; set; }
        public double ColdMat { get; set; }
    }
}