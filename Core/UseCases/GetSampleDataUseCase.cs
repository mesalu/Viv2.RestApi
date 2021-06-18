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
    [Obsolete("Use SampleAcquisitionUseCase instead.")]
    public class GetSampleDataUseCase : IGetSamplesUseCase
    {
        private readonly IPetStore _petStore;
        private readonly IUserStore _userStore;
        private readonly IEnvironmentStore _environmentStore;

        public GetSampleDataUseCase(IPetStore petStore, IUserStore userStore,
            IEnvironmentStore environmentStore)
        {
            _petStore = petStore;
            _userStore = userStore;
            _environmentStore = environmentStore;
        }
        
        public async Task<bool> Handle(DataAccessRequest<IEnvDataSample> message, IOutboundPort<GenericDataResponse<IEnvDataSample>> outputPort)
        {
            // verify the user exists:
            var user = await _userStore.GetUserById(message.UserId);
            if (user == null) return false;

            await _userStore.LoadEnvironments(user);

            var response = new GenericDataResponse<IEnvDataSample>();
            
            var samples = new List<IEnvDataSample>();
            foreach (var env in user.Environments)
            {
                await _environmentStore.LoadSamplesFor(env);
                
                switch (message.Strategy)
                {
                    case DataAccessRequest<IEnvDataSample>.AcquisitionStrategy.All:
                        samples.AddRange(env.EnvDataSamples);
                        break;
                    case DataAccessRequest<IEnvDataSample>.AcquisitionStrategy.Range:
                        samples.AddRange(env.EnvDataSamples.Where(message.SelectionPredicate));
                        break;
                    case DataAccessRequest<IEnvDataSample>.AcquisitionStrategy.Single:
                        // this one is a bit different, if we find a match amongst any environments,
                        // shortcut a return after processing it.
                        var sample = env.EnvDataSamples.FirstOrDefault(message.SelectionPredicate);
                        if (sample != null)
                        {
                            response.Result = new[] {sample};
                            outputPort.Handle(response);
                            return true;
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(message), "Strategy not recognized.");
                }
            }

            // in the event of strategy being single, samples will be empty, which is a suitable response.
            response.Result = samples;
            outputPort.Handle(response);
            return true;
        }
    }
}