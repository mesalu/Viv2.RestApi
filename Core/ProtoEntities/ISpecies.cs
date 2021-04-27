using System.Collections.Generic;

namespace Viv2.API.Core.ProtoEntities
{
    public interface ISpecies
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<IPet> Pets { get; }
    }
}