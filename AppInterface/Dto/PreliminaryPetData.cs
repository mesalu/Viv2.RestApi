using Viv2.API.Core.ProtoEntities;

namespace Viv2.API.AppInterface.Dto
{
    public class PreliminaryPetData
    {
        public PetDto Pet { get; set; }
        public SampleDto LatestSample { get; set; }
    }
}