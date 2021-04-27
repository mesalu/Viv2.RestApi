using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Viv2.API.Core.ProtoEntities;
using Viv2.API.Core.Services;
using Viv2.API.Infrastructure.DataStore.EfNpgSql.Contexts;
using Viv2.API.Infrastructure.DataStore.EfNpgSql.Entities;

namespace Viv2.API.Infrastructure.DataStore.EfNpgSql
{
    public class PetStore : IPetBackingStore
    {
        private readonly DataContext _context;

        public PetStore(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ISpecies>> GetSpeciesInfo() =>
            await _context.Species.ToListAsync(); 
        
        
        public async Task<ISpecies> GetSpeciesById(int id) => await _context.Species.FirstAsync(s => s.Id == id);

        public async Task<int> Create(IPet pet)
        {
            var concrete = pet as Pet;
            if (concrete == null) throw new ArgumentException("Mismatched infrastructure components");
            var result = await _context.Pets.AddAsync(concrete);
            return result.Entity.Id;
        }
    }
}
