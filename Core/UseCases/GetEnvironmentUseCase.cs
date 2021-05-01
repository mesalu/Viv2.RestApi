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
        private readonly IUserBackingStore _userStore;

        public GetEnvironmentUseCase(IUserBackingStore userStore)
        {
            _userStore = userStore;
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