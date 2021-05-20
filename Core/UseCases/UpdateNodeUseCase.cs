using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Viv2.API.Core.Adapters;
using Viv2.API.Core.Dto.Request;
using Viv2.API.Core.Dto.Response;
using Viv2.API.Core.Interfaces;
using Viv2.API.Core.Interfaces.UseCases;
using Viv2.API.Core.ProtoEntities;

namespace Viv2.API.Core.UseCases
{
    public class UpdateNodeUseCase : IUpdateNodeUseCase
    {
        private readonly IUserStore _userStore;
        private readonly IControllerStore _controllerStore;
        private readonly IEnvironmentStore _environmentStore;
        private readonly IEntityFactory _entityFactory;
        
        public UpdateNodeUseCase(IUserStore userStore,
            IControllerStore controllerStore,
            IEnvironmentStore environmentStore,
            IEntityFactory entityFactory)
        {
            _userStore = userStore;
            _controllerStore = controllerStore;
            _environmentStore = environmentStore;
            _entityFactory = entityFactory;
        }
        
        public async Task<bool> Handle(NodeUpdateRequest message, IOutboundPort<GenericDataResponse<IController>> outputPort)
        {
            var user = await _userStore.GetUserById(message.UserId);
            if (user == null) return false; // specified user doesn't exist, do nothing on their behalf.
            
            var controller = await _controllerStore.GetById(message.ControllerId);

            if (message.Operation == NodeUpdateRequest.Mode.Create)
            {
                // if the controller exists - then it was owned by another user
                // if they haven't relinquished it then we don't want to re-author its owner.
                if (controller?.Owner != null)
                    if (controller.Owner != user) return false;
                
                
                // add controller (or a new instance) to the user:
                controller ??= _entityFactory.GetControllerBuilder()
                    .SetId(message.ControllerId)
                    .Build();

                await _userStore.AddAssociationToController(user, controller);
            }
            else if (message.Operation == NodeUpdateRequest.Mode.PeerUpdate)
            {
                if (controller == null) return false;
                
                // call through to RegisterEnvironmentUseCase?
                foreach (Guid envId in message.PeerIds)
                {
                    // try loading the environment, update its association
                    var env = await _environmentStore.GetById(envId);
                    
                    // Behavior here is TBD. Should environment devices just be 'whoever has posession it'
                    // if so, should existing data associated to that environment be dumped?
                    // if not, hos should we permit giving them out? etc.
                    // Point is, I have some system-level planning to do here, but I need *something*
                    // here for the time being:
                    if (env == null) continue;
                    if (env.Controller != controller)
                        await _controllerStore.ReParentEnvironment(controller, env);
                    
                    // for now: not creating new Env entities in this use case. Expect it to be
                    // handled separately
                }
            }

            var response = new GenericDataResponse<IController>
            {
                Result = new[] {controller}
            };
            
            outputPort.Handle(response);
            return true;
        }
    }
}