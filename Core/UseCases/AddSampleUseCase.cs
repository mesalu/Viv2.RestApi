using System;
using System.Linq;
using System.Threading.Tasks;
using Viv2.API.Core.Dto.Request;
using Viv2.API.Core.Dto.Response;
using Viv2.API.Core.Interfaces;
using Viv2.API.Core.Interfaces.UseCases;
using Viv2.API.Core.ProtoEntities;
using Viv2.API.Core.Services;

namespace Viv2.API.Core.UseCases
{
    public class AddSampleUseCase : IAddSampleUseCase
    {
        private readonly IUserBackingStore _userStore;
        private readonly IEnvironmentBackingStore _envStore;
        private readonly IEntityFactory _entityFactory;
        
        public AddSampleUseCase(IUserBackingStore userStore, IEnvironmentBackingStore envStore,
            IEntityFactory entityFactory)
        {
            _userStore = userStore;
            _envStore = envStore;
            _entityFactory = entityFactory;
        }
        
        public async Task<bool> Handle(NewSampleRequest message, IOutboundPort<BlankResponse> outputPort)
        {
            // Verify that the specified user maps to the sample's environment. (e.g., can this user add samples to that
            // environment?)
            var user = await _userStore.GetUserById(message.UserId);
            if (user != null) await _userStore.LoadEnvironments(user);
            
            var env = user?.Environments.FirstOrDefault(e => e.Id == message.Sample.Environment);
            if (env == null) return false; // environment not owned by user or user not found.

            // TODO: ensure the pet reference is loaded in above search.
           
            // Fill in EnvDataSample entity instance.
            var sample = _entityFactory.GetSampleBuilder()
                .AddHotGlassMeasurement(message.Sample.HotGlass ?? 0)
                .AddHotMatMeasurement(message.Sample.HotMat ?? 0)
                .AddMidGlassMeasurement(message.Sample.MidGlass ?? 0)
                .AddColdGlassMeasurement(message.Sample.ColdGlass ?? 0)
                .AddColdMatMeasurement(message.Sample.ColdMat ?? 0)
                .SetInhabitant(env.Inhabitant)
                .Build();

            await _envStore.AddSample(sample);
            return true;
        }
    }
}