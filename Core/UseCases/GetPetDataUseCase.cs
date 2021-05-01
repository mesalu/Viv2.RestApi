using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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
    public class GetPetDataUseCase : IGetPetDataUseCase
    {
        private readonly IUserBackingStore _userStore;
        private readonly IPetBackingStore _petStore;
        
        public GetPetDataUseCase(IPetBackingStore petStore, IUserBackingStore userStore)
        {
            _userStore = userStore;
            _petStore = petStore;
        }
        public async Task<bool> Handle(DataAccessRequest<IPet> message, IOutboundPort<GenericDataResponse<IPet>> outputPort)
        {
            var user = await _userStore.GetUserById(message.UserId);
            if (user == null) return false;

            await _userStore.LoadPets(user);
            
            // Load species data - its trivial to include.
            foreach (var pet in user.Pets)
            {
                await _petStore.LoadSpecies(pet);
            }

            var response = new GenericDataResponse<IPet>();
            
            switch (message.Strategy)
            {
                case DataAccessRequest<IPet>.AcquisitionStrategy.All:
                    response.Result = user.Pets.ToImmutableList();
                    outputPort.Handle(response);
                    return true;
                case DataAccessRequest<IPet>.AcquisitionStrategy.Range:
                    response.Result = user.Pets.Where(message.SelectionPredicate).ToImmutableList();
                    outputPort.Handle(response);
                    return true;
                case DataAccessRequest<IPet>.AcquisitionStrategy.Single:
                    response.Result = new List<IPet>(new[] {user.Pets.FirstOrDefault(message.SelectionPredicate)});
                    outputPort.Handle(response);
                    return true;
                default:
                    throw new ArgumentOutOfRangeException(nameof(message), "Unrecognized strategy");
            }
        }
    }
}