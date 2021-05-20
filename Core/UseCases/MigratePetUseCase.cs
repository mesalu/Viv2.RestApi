using System.Linq;
using System.Threading.Tasks;
using Viv2.API.Core.Adapters;
using Viv2.API.Core.Dto.Request;
using Viv2.API.Core.Dto.Response;
using Viv2.API.Core.Interfaces;
using Viv2.API.Core.Interfaces.UseCases;

namespace Viv2.API.Core.UseCases
{
    public class MigratePetUseCase : IMigratePetUseCase
    {
        private readonly IUserStore _userStore;
        private readonly IPetStore _petStore;

        public MigratePetUseCase(IUserStore userStore, IPetStore petStore)
        {
            _userStore = userStore;
            _petStore = petStore;
        }

        public async Task<bool> Handle(MigratePetRequest message, IOutboundPort<BlankResponse> outputPort)
        {
            // load user - verify they have access to both env and pet specified. 
            var user = await _userStore.GetUserById(message.UserId);

            if (user == null) return false;

            await _userStore.LoadEnvironments(user);
            await _userStore.LoadPets(user);

            var canProceed =
                user.Pets.Select(p => p.Id).Contains(message.PetId)
                && user.Environments.Select(e => e.Id).Contains(message.EnvId);

            if (!canProceed) return false;
            
            var pet = user.Pets.First(p => p.Id == message.PetId);
            var targetEnv = user.Environments.First(env => env.Id == message.EnvId);
            await _petStore.MigratePet(pet, targetEnv);
            
            return true;
        }
    }
}