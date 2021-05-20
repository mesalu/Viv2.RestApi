using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Viv2.API.Core.Adapters;
using Viv2.API.Core.Dto.Request;
using Viv2.API.Core.Dto.Response;
using Viv2.API.Core.Interfaces;
using Viv2.API.Core.Interfaces.UseCases;
using Viv2.API.Core.ProtoEntities;

namespace Viv2.API.Core.UseCases
{
    public class GetEnvironmentUseCase : IGetEnvironmentsUseCase
    {
        private readonly IUserStore _userStore;
        private readonly IEnvironmentStore _envStore;

        public GetEnvironmentUseCase(IUserStore userStore, IEnvironmentStore envStore)
        {
            _userStore = userStore;
            _envStore = envStore;
        }
            
        public async Task<bool> Handle(DataAccessRequest<IEnvironment> message, IOutboundPort<GenericDataResponse<IEnvironment>> outputPort)
        {
            switch (message.Strategy)
            {
                case DataAccessRequest<IEnvironment>.AcquisitionStrategy.All:
                    // Acquire all associated environments, null-cascade throughout
                    var user = await _userStore.GetUserById(message.UserId);
                    if (user != null) await _userStore.LoadEnvironments(user);
                    var envs = user?.Environments;

                    foreach (var env in envs)
                    {
                        await _envStore.LoadControllerFor(env);
                        await _envStore.LoadPetFor(env);
                    }
                    
                    // Compose a response.
                    var response = new GenericDataResponse<IEnvironment>
                    {
                        Result = envs?.ToList()
                    };
                    
                    outputPort.Handle(response);
                    return (envs != null);
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(message.Strategy));
            }
        }
    }
}