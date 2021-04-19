using System.Linq;
using System.Threading.Tasks;
using Viv2.API.Core.Dto.Request;
using Viv2.API.Core.Dto.Response;
using Viv2.API.Core.Entities;
using Viv2.API.Core.Interfaces;
using Viv2.API.Core.Interfaces.UseCases;
using Viv2.API.Core.Services;

namespace Viv2.API.Core.UseCases
{
    public class AddSampleUseCase : IAddSampleUseCase
    {
        private readonly IUserBackingStore _userStore;
        private readonly IPetBackingStore _petStore;
        

        public AddSampleUseCase(IUserBackingStore userStore, IPetBackingStore petStore)
        {
            _userStore = userStore;
            _petStore = petStore;
        }
        
        public async Task<bool> Handle(NewSampleRequest message, IOutboundPort<BlankResponse> outputPort)
        {
            // Verify that the specified user maps to the sample's environment. (e.g., can this user add samples to that
            // environment?)
            var user = await _userStore.GetUserById(message.UserId);
            if (user == null) return false;

            if (!user.Environments.Select(env => env.Id).Contains(message.Sample.Environment))
                return false;

            // Add sample to the collection via data store.

            return await Task.FromResult(false);
        }
    }
}