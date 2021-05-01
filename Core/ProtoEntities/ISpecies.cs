using System.Collections.Generic;

namespace Viv2.API.Core.ProtoEntities
{
    public interface ISpecies
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ScientificName { get; set; }
        
        /// <summary>
        /// The latitude at which members of this species should
        /// use by default.
        /// </summary>
        public double DefaultLatitude { get; set; }
        
        /// <summary>
        /// The longitude at which members of this species should
        /// use by default.
        /// </summary>
        public double DefaultLongitude { get; set; }
        public ICollection<IPet> Pets { get; }
    }
}