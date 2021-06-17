using System;
using System.ComponentModel.DataAnnotations.Schema;
using Viv2.API.Core.ProtoEntities;

#nullable enable

namespace Viv2.API.Infrastructure.DataStore.EfNpgSql.Entities
{
    public class EnvDataSample : IEnvDataSample
    {
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
        public Pet? RealOccupant { get; set; }

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