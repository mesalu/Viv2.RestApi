using Viv2.API.Core.Dto.Response;
using Viv2.API.Core.Interfaces;
using Viv2.API.Core.ProtoEntities;

namespace Viv2.API.Core.Dto.Request
{
    public class CreateSpeciesRequest : IUseCaseRequest<NewEntityResponse<int>>
    {
        public string Name { get; set; }
        public string ScientificName { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}