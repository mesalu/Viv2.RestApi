using System;

#nullable enable

namespace Viv2.API.Core.ProtoEntities
{
    public interface IPet
    {
        public int Id { get; set; }
        public ISpecies Species { get; set; }
        public IUser CareTaker { get; }
        public IEnvironment? Enclosure { get; }
        public string Name { get; set; }
        public string Morph { get; set; }
        public DateTime? HatchDate { get; set; }
    }
}