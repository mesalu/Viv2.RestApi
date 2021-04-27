using System.Threading.Tasks;
using Viv2.API.Core.Dto.Request;
using Viv2.API.Core.Dto.Response;
using Viv2.API.Core.Interfaces;
using Viv2.API.Core.Interfaces.UseCases;
using Viv2.API.Core.ProtoEntities;
using Viv2.API.Core.Services;

namespace Viv2.API.Core.UseCases
{
    public class AddPetUseCase : IAddPetUseCase
    {
        private readonly IPetBackingStore _backingStore;
        private readonly IEntityFactory _entityFactory;

        public AddPetUseCase(IPetBackingStore backingStore, IEntityFactory entityFactory)
        {
            _backingStore = backingStore;
            _entityFactory = entityFactory;
        }
        public async Task<bool> Handle(NewPetRequest message, IOutboundPort<NewEntityResponse<string>> outputPort)
        {
            // Compose a new Pet instance:
            var pet = _entityFactory.GetPetBuilder()
                .SetName(message.Name)
                .SetSpecies(await _backingStore.GetSpeciesById(message.SpeciesType))
                .SetMorph(message.Morph)
                .Build();

            await _backingStore.Create(pet);
            return true;
        }
    }
}