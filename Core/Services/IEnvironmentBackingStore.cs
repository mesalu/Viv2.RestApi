using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Viv2.API.Core.ProtoEntities;

namespace Viv2.API.Core.Services
{
    /// <summary>
    /// Store interface for interacting with environment data.
    /// </summary>
    public interface IEnvironmentBackingStore
    {
        /// <summary>
        /// Fetch an instance from the persisted store by id.
        ///
        /// Note: Not all backing implementations ensure that all relationships are loaded.
        ///       Use the appropriate "LoadPropertyFor()" methods to ensure property relationships
        ///       are loaded.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IEnvironment> GetById(System.Guid id);

        /// <summary>
        /// Creates a new Environment as defined by `environment`
        /// </summary>
        /// <param name="environment">An environment instance to create a new entry with</param>
        /// <returns>The Guid of the new Environment entry.</returns>
        Task<Guid> Create([NotNull] IEnvironment environment);
        
        /// <summary>
        /// Persists the current state of `environment` to the store.
        /// </summary>
        /// <param name="environment"></param>
        /// <returns></returns>
        Task Update([NotNull] IEnvironment environment);

        /// <summary>
        /// Loads, and assigns back to `env` if applicable, the set of EnvDataSamples
        /// associated to `env`. Returning the sample set.
        ///
        /// If the property appears to be loaded already, then this method may shortcut and return the currently loaded
        /// property.
        ///
        /// To ensure up-to-date data, set force to true.
        /// </summary>
        /// <returns></returns>
        Task<ICollection<IEnvDataSample>> LoadSamplesFor([NotNull] IEnvironment env, bool force = false);
        
        /// <summary>
        /// Loads, and assigns back to `env` if applicable, the set of Users
        /// associated to `env`. Returning the sample set.
        ///
        /// If the property appears to be loaded already, then this method may shortcut and return the currently loaded
        /// property.
        ///
        /// To ensure up-to-date data, set force to true.
        /// </summary>
        /// <returns></returns>
        Task<ICollection<IUser>> LoadUsersFor([NotNull] IEnvironment env, bool force = false);

        /// <summary>
        /// Loads, and assigns back to `env` if applicable, node controller set as the 'owner' of the environment.
        ///
        /// If the property appears to be loaded already, then this method may shortcut and return the currently loaded
        /// property.
        ///
        /// To ensure up-to-date data, set force to true.
        /// </summary>
        /// <returns></returns>
        Task<IController> LoadControllerFor([NotNull] IEnvironment env, bool force = false);

        /// <summary>
        /// Adds the given sample to the backing data store.
        /// Does not ensure that current handles on Environment objects will be updated appropriately.
        /// </summary>
        /// <param name="sample"></param>
        /// <returns></returns>
        Task AddSample([NotNull] IEnvDataSample sample);
    }
}