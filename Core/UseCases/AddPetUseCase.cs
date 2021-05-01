using System.Threading.Tasks;
using Viv2.API.Core.Adapters;
using Viv2.API.Core.Dto.Request;
using Viv2.API.Core.Dto.Response;
using Viv2.API.Core.Interfaces;
using Viv2.API.Core.Interfaces.UseCases;
using Viv2.API.Core.ProtoEntities;

namespace Viv2.API.Core.UseCases
{
    public class AddPetUseCase : IAddPetUseCase
    {
        private readonly IUserBackingStore _userStore;
        private readonly IPetBackingStore _backingStore;
        private readonly IEntityFactory _entityFactory;

        public AddPetUseCase(IPetBackingStore backingStore, IEntityFactory entityFactory, IUserBackingStore userStore)
        {
            _userStore = userStore;
            _backingStore = backingStore;
            _entityFactory = entityFactory;
        }
        public async Task<bool> Handle(CreatePetRequest message, IOutboundPort<NewEntityResponse<int>> outputPort)
        {
            var user = await _userStore.GetUserById(message.User);
            if (user == null) return false;
            
            // Compose a new Pet instance:
            var pet = _entityFactory.GetPetBuilder()
                .SetName(message.Name)
                .SetSpecies(await _backingStore.GetSpeciesById(message.SpeciesType))
                .SetMorph(message.Morph)
                .SetOwner(user)
                .Build();

            var id = await _backingStore.Create(pet);
            var response = new NewEntityResponse<int> { Id = id };
            outputPort.Handle(response);
            return true;
        }
    }
}