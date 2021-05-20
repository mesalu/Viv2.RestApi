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
        private readonly IUserStore _userStore;
        private readonly IPetStore _store;
        private readonly IEntityFactory _entityFactory;

        public AddPetUseCase(IPetStore store, IEntityFactory entityFactory, IUserStore userStore)
        {
            _userStore = userStore;
            _store = store;
            _entityFactory = entityFactory;
        }
        public async Task<bool> Handle(CreatePetRequest message, IOutboundPort<NewEntityResponse<int>> outputPort)
        {
            var user = await _userStore.GetUserById(message.User);
            if (user == null) return false;
            
            // Compose a new Pet instance:
            var pet = _entityFactory.GetPetBuilder()
                .SetName(message.Name)
                .SetSpecies(await _store.GetSpeciesById(message.SpeciesType))
                .SetMorph(message.Morph)
                .SetOwner(user)
                .Build();

            var id = await _store.Create(pet);
            var response = new NewEntityResponse<int> { Id = id };
            outputPort.Handle(response);
            return true;
        }
    }
}