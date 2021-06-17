using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Viv2.API.Core.Adapters;
using Viv2.API.Core.Exceptions;
using Viv2.API.Core.ProtoEntities;
using Viv2.API.Infrastructure.DataStore.EfNpgSql.Contexts;
using Viv2.API.Infrastructure.DataStore.EfNpgSql.Entities;
using Environment = Viv2.API.Infrastructure.DataStore.EfNpgSql.Entities.Environment;

namespace Viv2.API.Infrastructure.DataStore.EfNpgSql
{
    public class EnvironmentStore : IEnvironmentStore
    {
        private readonly DataContext _context;

        public EnvironmentStore(DataContext context)
        {
            _context = context;
        }

        public async Task<Guid> Create(IEnvironment environment)
        {
            // TODO: Mask out the Guid, ensure a new environment is created.
            try
            {
                var concrete = environment as Environment;
                if (concrete == null) throw new ArgumentException("Mismatched infrastructure components");
                
                var result = await _context.Environments.AddAsync(concrete);
                if (result.Entity.Id == null) throw new Exception("Failed to acquire Guid of new Entry");
                await _context.SaveChangesAsync();
                return result.Entity.Id.Value;
            }
            // TODO: Find tighter binding exception type, DbUpdateException wasn't working.
            catch (Exception)
            {
                throw new CollisionException("Collision in creating new Environment Entity");
            }
        }

        public async Task<IEnvironment> GetById(Guid id)
        {
            return await _context.Environments.FirstOrDefaultAsync(env => env.Id == id);
        }

        public async Task Update([NotNull] IEnvironment environment)
        {
            var concrete = environment as Environment;
            if (concrete == null) throw new ArgumentException("Mismatched infrastructure components");
            _context.Environments.Update(concrete);
            await _context.SaveChangesAsync();
        }

        public async Task<ICollection<IEnvDataSample>> LoadSamplesFor([NotNull] IEnvironment env, bool force = false)
        {
            // EnvDataSamples should never be null (an empty hash set is made in the default ctor), so instead
            // of checking for null-ness, we should check for contents.
            // TODO: double check logic for 'legitimately empty' scenarios.
            if (env.EnvDataSamples.Count > 0 && !force) return env.EnvDataSamples;
            
            var concrete = env as Environment;

            if (concrete == null) throw new ArgumentException("Mismatched datastore implementations");
            await _context.Entry(concrete).Collection(e => e.Samples).LoadAsync();
            return env.EnvDataSamples;
        }

        public async Task<ICollection<IUser>> LoadUsersFor([NotNull] IEnvironment env, bool force = false)
        {
            if (env.Users.Count > 0 && !force) return env.Users;
            
            // forced or not yet loaded:
            var concrete = env as Environment;
            if (concrete == null) throw new ArgumentException("Mismatched datastore implementations");
            
            await _context.Entry(concrete).Collection(e => e.BackedUsers).LoadAsync();
            return env.Users;
        }

        public async Task<IController> LoadControllerFor([NotNull] IEnvironment env, bool force = false)
        {
            if (env.Controller != null && !force) return env.Controller;
            
            // forced or not yet loaded:
            var concrete = env as Environment;
            if (concrete == null) throw new ArgumentException("Mismatched datastore implementations");
            
            await _context.Entry(concrete).Reference(e => e.RealController).LoadAsync();
            return env.Controller;
        }

        public async Task AddSample(IEnvDataSample sample)
        {
            var concrete = sample as EnvDataSample;
            if (concrete == null) throw new ArgumentException("Mismatched datastore implementations");
            await _context.EnvDataSamples.AddAsync(concrete);
            await _context.SaveChangesAsync();
        }

        public async Task<IPet> LoadPetFor([NotNull] IEnvironment env, bool force = false)
        {
            if (!force && env.Inhabitant != null) return env.Inhabitant; 
            
            var concrete = env as Environment;
            if (concrete == null) throw new ArgumentException("Mismatched datastore implementations");

            await _context.Entry(concrete).Reference(e => e.RealInhabitant).LoadAsync();
            return env.Inhabitant;
        }
    }
}
