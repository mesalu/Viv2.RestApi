using System;
using System.Threading.Tasks;
using Viv2.API.Core.Dto.Request;
using Viv2.API.Core.Dto.Response;
using Viv2.API.Core.Interfaces;
using Viv2.API.Core.Interfaces.UseCases;
using Viv2.API.Core.Services;

namespace Viv2.API.Core.UseCases
{
    public class AddUserUseCase : IAddUserUseCase
    {
        private readonly IUserBackingStore _userBackingStore;
        private readonly IEntityFactory _entityFactory;

        public AddUserUseCase(IUserBackingStore backingStore, IEntityFactory entityFactory)
        {
            _userBackingStore = backingStore;
            _entityFactory = entityFactory;
        }
        
        public async Task<bool> Handle(CreateUserRequest message, IOutboundPort<NewUserResponse> outputPort)
        {
            // ensure username, email, are unused.
            if (await _userBackingStore.GetUserByName(message.UserName) != null) return false;
            
            // create a new entity instance.
            var user = _entityFactory.GetUserBuilder()
                .AddName(message.UserName)
                .AddEmail(message.Email)
                .Build();
            
            var guid = await _userBackingStore.CreateUser(user, message.Password);
            
            // use backing store to persist.
            NewUserResponse response = new NewUserResponse();
            response.Id = (guid == Guid.Empty) ? null : guid.ToString();
            
            outputPort.Handle(response);
            return true;
        }
    }
}