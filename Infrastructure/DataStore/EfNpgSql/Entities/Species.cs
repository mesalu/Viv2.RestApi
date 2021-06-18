using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Viv2.API.Core.ProtoEntities;

namespace Viv2.API.Infrastructure.DataStore.EfNpgSql.Entities
{
    public class Species : ISpecies
    {
        public Species()
        {
            BackedPets = new HashSet<Pet>();
        }
        // Concrete class properties that EF uses for relations:
        public int Id { get; set; }
        public string Name { get; set; }
        public string ScientificName { get; set; }
        public double DefaultLatitude { get; set; }
        public double DefaultLongitude { get; set; }
        public ICollection<Pet> BackedPets { get; set; }
        
        // Abstraction fulfillment:
        [NotMapped]
        public ICollection<IPet> Pets => BackedPets.Select(bp => bp as IPet).ToImmutableList();
    }
}