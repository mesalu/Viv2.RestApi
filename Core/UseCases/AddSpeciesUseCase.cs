using System;
using System.Threading.Tasks;
using Viv2.API.Core.Adapters;
using Viv2.API.Core.Dto.Request;
using Viv2.API.Core.Dto.Response;
using Viv2.API.Core.Interfaces;
using Viv2.API.Core.Interfaces.UseCases;

namespace Viv2.API.Core.UseCases
{
    public class AddSpeciesUseCase : IAddSpeciesUseCase
    {
        private readonly IEntityFactory _entityFactory;
        private readonly IPetBackingStore _petStore;

        public AddSpeciesUseCase(IEntityFactory factory, IPetBackingStore petStore)
        {
            _entityFactory = factory;
            _petStore = petStore;
        }
        public async Task<bool> Handle(CreateSpeciesRequest message, IOutboundPort<NewEntityResponse<int>> outputPort)
        {
            var species = _entityFactory.GetSpeciesBuilder()
                .SetName(message.Name)
                .SetScientificName(message.ScientificName)
                .SetLatitude(message.Latitude)
                .SetLongitude(message.Longitude)
                .Build();
            
            // validate some stuff:
            if (string.IsNullOrWhiteSpace(species.Name)
                || string.IsNullOrEmpty(species.ScientificName)
                || Math.Abs(species.DefaultLatitude) > 90.0
                || Math.Abs(species.DefaultLongitude) > 180.0) return false;

            var response = new NewEntityResponse<int>
            {
                Id = await _petStore.Create(species)
            };
            
            outputPort.Handle(response);
            return true;
        }
    }
}