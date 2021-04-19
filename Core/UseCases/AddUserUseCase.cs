using System;
using System.Threading.Tasks;
using Viv2.API.Core.Dto.Request;
using Viv2.API.Core.Dto.Response;
using Viv2.API.Core.Entities;
using Viv2.API.Core.Interfaces;
using Viv2.API.Core.Interfaces.UseCases;
using Viv2.API.Core.Services;

namespace Viv2.API.Core.UseCases
{
    public class AddUserUseCase : IAddUserUseCase
    {
        private readonly IUserBackingStore _userBackingStore;

        public AddUserUseCase(IUserBackingStore backingStore)
        {
            _userBackingStore = backingStore;
        }
        
        public async Task<bool> Handle(CreateUserRequest message, IOutboundPort<NewUserResponse> outputPort)
        {
            // ensure username, email, are unused.
            if (await _userBackingStore.GetUserByName(message.UserName) != null) return false;
            
            // create a new entity instance.
            User user = new User
            {
                Name = message.UserName,
                Email = message.Email
            };

            var guid = await _userBackingStore.CreateUser(user, message.Password);
            
            // use backing store to persist.
            NewUserResponse response = new NewUserResponse();
            response.UserId = (guid == Guid.Empty) ? null : guid.ToString();
            
            outputPort.Handle(response);
            return true;
        }
    }
}