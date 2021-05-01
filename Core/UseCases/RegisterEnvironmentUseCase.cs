using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Viv2.API.Core.Adapters;
using Viv2.API.Core.Dto.Request;
using Viv2.API.Core.Dto.Response;
using Viv2.API.Core.Exceptions;
using Viv2.API.Core.Interfaces;
using Viv2.API.Core.Interfaces.UseCases;
using Viv2.API.Core.ProtoEntities;

namespace Viv2.API.Core.UseCases
{
    public class RegisterEnvironmentUseCase : IRegisterEnvironmentUseCase
    {
        private readonly IEnvironmentBackingStore _envStore;
        private readonly IUserBackingStore _userStore;
        private readonly IEntityFactory _entityFactory;

        public RegisterEnvironmentUseCase(IEnvironmentBackingStore envStore, 
            IUserBackingStore userStore,
            IEntityFactory entityFactory)
        {
            _envStore = envStore;
            _userStore = userStore;
            _entityFactory = entityFactory;
        }
        
        public async Task<bool> Handle(RegisterEnvironmentRequest message, IOutboundPort<NewEntityResponse<Guid>> outputPort)
        {
            var response = new NewEntityResponse<Guid>();
            var user = await _userStore.GetUserById(message.Owner);
            if (user != null) await _userStore.LoadEnvironments(user);
            
            switch (message.CreateMode)
            {
                case RegisterEnvironmentRequest.Mode.Touch: 
                    // Check if user already has it:
                    var env = user?.Environments?.FirstOrDefault(e => e.Id == message.MfgId);
                    
                    // check if we have this environment already - and that its associated to user
                    // if not, we'll fall through to Create logic where we'll attempt creating it - erroring if already
                    // present.
                    if (env != null)
                    {
                        // This next line should be impossible - its just here to keep linting happy.
                        if (env.Id == null) throw new Exception("Malformed input on this entity");

                        // we already have it and its associated to user.
                        // should be same as message.MfgId
                        response.Id = env.Id.Value;
                        break;
                    }
                    else if (user == null) return false; // nothing to do, this is a mis-use.
                    else
                        // Apparently we gotta explicitly mimic fallthrough...
                        goto case RegisterEnvironmentRequest.Mode.Create;

                case RegisterEnvironmentRequest.Mode.Create:
                    // Environments need an initial user:
                    if (user == null) return false;
                    
                    // check if it exists (blind firing a create call
                    // may be a pretty hard crash.)
                    var searchResult = await _envStore.GetById(message.MfgId);
                    if (searchResult != null) return false;

                    // Compose a new entry entity
                    var envEntity = _entityFactory.GetEnvironmentBuilder()
                        .SetId(message.MfgId)
                        .SetDescription(message.Description)
                        .SetModelInfo(message.Model)
                        .Build();
                    
                    response.Id = await _envStore.Create(envEntity);
                    
                    // Associate to user:
                    await _userStore.AddAssociationToEnv(user, envEntity);
                    break;       
    
                default:
                    throw new ArgumentOutOfRangeException(nameof(message));
            }
            
            outputPort.Handle(response);
            return true;
        }
    }
}