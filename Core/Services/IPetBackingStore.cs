using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Viv2.API.Core.ProtoEntities;

namespace Viv2.API.Core.Services
{
    /// <summary>
    /// An interface for describing all pet-data related operations that Core will depend on.
    /// </summary>
    public interface IPetBackingStore
    {
        /// <summary>
        /// Load all configured species out of persistent store.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<ISpecies>> GetSpeciesInfo();

        /// <summary>
        /// Loads and returns the species with the given type, null if no such entry exists.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ISpecies> GetSpeciesById(int id);
        
        /// <summary>
        /// Creates a new Pet entity in the persistent store.
        /// </summary>
        /// <param name="pet"></param>
        /// <returns></returns>
        Task<int> Create([NotNull] IPet pet);
    }
}
