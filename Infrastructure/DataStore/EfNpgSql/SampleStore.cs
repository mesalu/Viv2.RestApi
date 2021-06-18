using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Viv2.API.Core.Adapters;
using Viv2.API.Core.ProtoEntities;
using Viv2.API.Infrastructure.DataStore.EfNpgSql.Contexts;

namespace Viv2.API.Infrastructure.DataStore.EfNpgSql
{
    public class SampleStore : ISampleStore
    {
        private readonly DataContext _context;

        public SampleStore(DataContext context)
        {
            _context = context;
        }
        
        public async Task<IList<IEnvDataSample>> GetRangeByUser([NotNull] IUser user, DateTime start, DateTime end)
        {
            return await _context.Users
                .Include(u => u.BackedEnvironments)
                .ThenInclude(env => env.Samples)
                .SelectMany(u => u.BackedEnvironments)
                .SelectMany(env => env.Samples)
                .Select(concrete => concrete as IEnvDataSample)
                .ToListAsync();
        }

        public async Task<IList<IEnvDataSample>> GetRangeByEnv([NotNull] IEnvironment env, DateTime start, DateTime end)
        {
            return await _context.EnvDataSamples
                .Include(sample => sample.RealEnvironment)
                .Where(sample => sample.RealEnvironment != null && sample.RealEnvironment.Id == env.Id)
                .Where(sample => sample.Captured >= start && sample.Captured < end)
                .Select(concrete => concrete as IEnvDataSample)
                .ToListAsync();
        }

        public async Task<IList<IEnvDataSample>> GetRangeByPet([NotNull] IPet pet, DateTime start, DateTime end)
        {
            return await _context.EnvDataSamples
                .Include(sample => sample.RealOccupant)
                .Where(sample => sample.RealOccupant.Id == pet.Id)
                .Where(sample => sample.Captured >= start && sample.Captured < end)
                .Select(concrete => concrete as IEnvDataSample)
                .ToListAsync();
        }
    }
}