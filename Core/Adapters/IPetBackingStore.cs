using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Viv2.API.Core.ProtoEntities;

namespace Viv2.API.Core.Adapters
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
        Task<ICollection<ISpecies>> GetSpeciesInfo();

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
        /// <returns>ID of the new entity</returns>
        Task<int> Create([NotNull] IPet pet);

        /// <summary>
        /// Creates a new Species entity in the persistent store.
        /// </summary>
        /// <param name="species"></param>
        /// <returns>ID of the new entity</returns>
        Task<int> Create([NotNull] ISpecies species);

        /// <summary>
        /// Loads the species reference of the specified pet.
        /// </summary>
        /// <param name="pet"></param>
        /// <param name="force"></param>
        /// <returns></returns>
        Task<ISpecies> LoadSpecies([NotNull] IPet pet, bool force = false);

        /// <summary>
        /// Loads the CareTaker reference of the specified pet.
        /// </summary>
        /// <param name="pet">The pet whose caretaker should be loaded</param>
        /// <param name="force">Force a reload</param>
        /// <returns></returns>
        public Task<IUser> LoadCareTaker([NotNull] IPet pet, bool force = false);
    }
}
