using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Viv2.API.Core.Entities;
using Viv2.API.Core.Services;
using Viv2.API.Infrastructure.DataStore.EfNpgSql.Contexts;
using Environment = Viv2.API.Core.Entities.Environment;

namespace Viv2.API.Infrastructure.DataStore.EfNpgSql
{
    public class EnvironmentStore : IEnvironmentBackingStore
    {
        private readonly DataContext _context;

        public EnvironmentStore(DataContext context)
        {
            _context = context;
        }
        
        public async Task<Environment> GetById(Guid id)
        {
            return await _context.Environments.FirstOrDefaultAsync(env => env.Id == id);
        }

        public async Task Update([NotNull] Environment environment)
        {
            _context.Environments.Update(environment);
            await _context.SaveChangesAsync();
        }

        public async Task<ICollection<EnvDataSample>> LoadSamplesFor([NotNull] Environment env, bool force = false)
        {
            // EnvDataSamples should never be null (an empty hash set is made in the default ctor), so instead
            // of checking for null-ness, we should check for contents.
            // TODO: double check logic for 'legitimately empty' scenarios.
            if (env.EnvDataSamples.Count > 0 && !force) return env.EnvDataSamples;

            await _context.Entry(env).Collection(e => e.EnvDataSamples).LoadAsync();
            return env.EnvDataSamples;
        }

        public async Task<ICollection<User>> LoadUsersFor([NotNull] Environment env, bool force = false)
        {
            if (env.Users.Count > 0 && !force) return env.Users;
            
            // forced or not yet loaded:
            await _context.Entry(env).Collection(e => e.Users).LoadAsync();
            return env.Users;
        }

        public async Task<Controller> LoadControllerFor([NotNull] Environment env, bool force = false)
        {
            if (env.Controller != null && !force) return env.Controller;
            
            // forced or not yet loaded:
            await _context.Entry(env).Reference(e => e.Controller).LoadAsync();
            return env.Controller;
        }
    }
}
