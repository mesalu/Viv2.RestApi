﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Viv2.API.Core.Adapters;
using Viv2.API.Core.ProtoEntities;
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

        public async Task<ICollection<ISpecies>> GetSpeciesInfo() =>
            await _context.Species.ToArrayAsync(); 
        
        public async Task<ISpecies> GetSpeciesById(int id) => await _context.Species.FirstAsync(s => s.Id == id);

        public async Task<int> Create([NotNull] IPet pet)
        {
            var concrete = pet as Pet;
            if (concrete == null) throw new ArgumentException("Mismatched infrastructure components");
            var result = await _context.Pets.AddAsync(concrete);
            await _context.SaveChangesAsync();
            return result.Entity.Id;
        }

        public async Task<int> Create([NotNull] ISpecies species)
        {
            var concrete = species as Species;
            if (concrete == null) throw new ArgumentException("Mismatched infrastructure components");
            var result = await _context.Species.AddAsync(concrete);
            await _context.SaveChangesAsync();
            return result.Entity.Id;
        }

        public async Task<ISpecies> LoadSpecies([NotNull] IPet pet, bool force = false)
        {
            if (!force && pet.Species != null) return pet.Species;
            
            var concrete = pet as Pet;
            if (concrete == null) throw new ArgumentException("Mismatched infrastructure components");

            await _context.Entry(concrete).Reference(p => p.RealSpecies).LoadAsync();
            
            return pet.Species;
        }
        
        public async Task<IUser> LoadCareTaker([NotNull] IPet pet, bool force = false)
        {
            if (!force && pet.CareTaker != null) return pet.CareTaker;
            
            var concrete = pet as Pet;
            if (concrete == null) throw new ArgumentException("Mismatched infrastructure components");

            await _context.Entry(concrete).Reference(p => p.RealCareTaker).LoadAsync();
            
            return pet.CareTaker;
        }
    }
}
