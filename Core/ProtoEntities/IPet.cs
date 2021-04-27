using System;

namespace Viv2.API.Core.ProtoEntities
{
    public interface IPet
    {
        public int Id { get; set; }
        public ISpecies Species { get; set; }
        public string Name { get; set; }
        public string Morph { get; set; }
        public DateTime? HatchDate { get; set; }
    }
}