using System;
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
    /// <summary>
    /// Main implementation for ISampleAcquisitionUseCase.
    /// </summary>
    public class SampleAcquisitionUseCase : ISampleAcquisitionUseCase
    {
        private readonly ISampleStore _sampleStore;
        private readonly IUserStore _userStore;

        public SampleAcquisitionUseCase(ISampleStore sampleStore, IUserStore userStore)
        {
            _sampleStore = sampleStore;
            _userStore = userStore;
        }
        
        public async Task<bool> Handle(SampleAccessRequest message, IOutboundPort<GenericDataResponse<IEnvDataSample>> outputPort)
        {
            var response = new GenericDataResponse<IEnvDataSample>();
            var user = await _userStore.GetUserById(message.UserId);
            switch (message.Selector)
            {
                case SampleAccessRequest.SelectionCriteria.User:
                    if (user == null) return false;

                    response.Result = await _sampleStore.GetRangeByUser(user, message.RangeStart, message.RangeEnd);
                    break;
                    
                case SampleAccessRequest.SelectionCriteria.Environment:
                    // ensure the user has access to see this environment's data:
                    await _userStore.LoadEnvironments(user);
                    var env = user.Environments.FirstOrDefault(e => e.Id == message.EnvId);
                    if (env == null) return false; // env doesn't exist or user can't access it.

                    response.Result = await _sampleStore.GetRangeByEnv(env, message.RangeStart, message.RangeEnd);
                    break;
                    
                case SampleAccessRequest.SelectionCriteria.Pet:
                    // ensure the user owns the pet in question:
                    await _userStore.LoadPets(user);
                    var pet = user.Pets.FirstOrDefault(p => p.Id == message.PetId);
                    if (pet == null) return false; // pet doesn't exist or user can't access its data.

                    response.Result = await _sampleStore.GetRangeByPet(pet, message.RangeStart, message.RangeEnd);
                    break;
                    
                default:
                    throw new ArgumentException("Invalid selection mode", nameof(message));
            }
            
            outputPort.Handle(response);
            return true;
        }
    }
}