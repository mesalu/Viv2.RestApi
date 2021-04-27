using System;
using System.ComponentModel.DataAnnotations.Schema;
using Viv2.API.Core.ProtoEntities;

namespace Viv2.API.Infrastructure.DataStore.EfNpgSql.Entities
{
    public class Pet : IPet
    {
        public int Id { get; set; }
        
        // What EF needs (a mapping to concrete classes in the same collection of entities.)
        public Species RealSpecies { get; set; }
        
        // what Core needs, but EF can't see.
        [NotMapped]
        public ISpecies Species 
        { get => RealSpecies;
            set
            {
                if (value is Species species) RealSpecies = species;
            } 
        }
        
        public string Name { get; set; }
        public string Morph { get; set; }
        public DateTime? HatchDate { get; set; }
    }
}