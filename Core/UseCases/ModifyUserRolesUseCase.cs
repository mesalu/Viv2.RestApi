using System.Threading.Tasks;
using Viv2.API.Core.Adapters;
using Viv2.API.Core.Constants;
using Viv2.API.Core.Dto.Request;
using Viv2.API.Core.Dto.Response;
using Viv2.API.Core.Interfaces;
using Viv2.API.Core.Interfaces.UseCases;

namespace Viv2.API.Core.UseCases
{
    public class ModifyUserRolesUseCase : IModifyUserRolesUseCase
    {
        private readonly IUserStore _userStore;
        
        public ModifyUserRolesUseCase(IUserStore userStore)
        {
            _userStore = userStore;
        }
        
        public async Task<bool> Handle(ModifyUserRolesRequest message, IOutboundPort<BlankResponse> outputPort)
        {
            // fetch the user instance.
            var user = await _userStore.GetUserById(message.UserId);
            if (user == null) return false;

            switch (message.Mode)
            {
                case ModifyUserRolesRequest.ModificationMode.Add:
                    await _userStore.AddToRoles(user, message.DesiredRoles);
                    return true;
                case ModifyUserRolesRequest.ModificationMode.Set:
                    await _userStore.RemoveRolesFromUser(user);
                    await _userStore.AddToRoles(user, message.DesiredRoles);
                    return true;
                default:
                    return false;
            }
        }
    }
}