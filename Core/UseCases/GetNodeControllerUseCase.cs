using System.Threading.Tasks;
using Viv2.API.Core.Adapters;
using Viv2.API.Core.Dto.Request;
using Viv2.API.Core.Dto.Response;
using Viv2.API.Core.Interfaces;
using Viv2.API.Core.Interfaces.UseCases;
using Viv2.API.Core.ProtoEntities;

namespace Viv2.API.Core.UseCases
{
    public class GetNodeControllerUseCase : IGetNodeControllerUseCase
    {
        private readonly IUserBackingStore _userStore;

        public GetNodeControllerUseCase(IUserBackingStore userStore)
        {
            _userStore = userStore;
        }
        
        public async Task<bool> Handle(DataAccessRequest<IController> message, IOutboundPort<GenericDataResponse<IController>> outputPort)
        {
            // Load user, verify they exist.
            var user = await _userStore.GetUserById(message.UserId);
            if (user == null) return false;

            // pipe on back through the port.
            var response = new GenericDataResponse<IController>
            {
                // Load controllers (Note: userStore impls must include controller.Environments relation here)
                // associated to that user.
                Result = await _userStore.LoadControllers(user)
            };
            
            outputPort.Handle(response);
            return true;
        }
    }
}