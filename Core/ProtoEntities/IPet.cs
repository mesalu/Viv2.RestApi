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
        public string Name { get; }
        public string Morph { get; }
        public DateTime? HatchDate { get; }
        public IBlobRecord? ProfileImage { get; }
        public IEnvDataSample? LatestSample { get; }
    }
}