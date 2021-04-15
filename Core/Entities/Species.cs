using System.Collections.Generic;

namespace Viv2.API.Core.Entities
{
    public class Species
    {
        public Species()
        {
            Pets = new HashSet<Pet>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Pet> Pets { get; set; }
    }
}