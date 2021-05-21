using System;
using System.Collections.Generic;
using Viv2.API.Core.ProtoEntities;

#nullable enable

namespace Viv2.API.Core.Adapters
{
    /// <summary>
    /// Since Core expresses its required 'entities' as interfaces (making them more of a proto-entity)
    /// they can't be instantiated direclty, instead they need to be constructed by an entity
    /// with implementation knowledge of their composition.
    /// This interface declare a service for doing such.
    ///
    /// It is important to note that the constructs provided via this factory are not ensured
    /// to exist in the backing store, nor are they in any way added to the backing store during
    /// the factory's operation
    /// </summary>
    public interface IEntityFactory
    {
        // I'd obscure this one by making it internal or the like as its not externally relevant
        // but I'm not sure how to accomplish that with out running into CS0061.
        /// <summary>
        /// Common base interface for entity builders.
        /// </summary>
        /// <typeparam name="TProtoEntity"></typeparam>
        public interface IEntityBuilder <TProtoEntity>
        {
            /// <summary>
            /// Constructs the final entity object as
            /// configured by prior builder calls. 
            /// </summary>
            /// <returns>The new entity</returns>
            TProtoEntity Build();
        }
        
        public interface IUserBuilder : IEntityBuilder<IUser>
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="name"></param>
            /// <returns>this</returns>
            IUserBuilder AddName(string name);
            
            /// <summary>
            /// 
            /// </summary>
            /// <param name="email"></param>
            /// <returns>this</returns>
            IUserBuilder AddEmail(string email);
        }

        public interface IEnvBuilder : IEntityBuilder<IEnvironment>
        {
            IEnvBuilder SetId(Guid id);
            IEnvBuilder SetInhabitant(IPet? pet);
            IEnvBuilder SetModelInfo(string model);
            IEnvBuilder SetDescription(string descr);
            IEnvBuilder WithSamples(IEnumerable<IEnvDataSample> samples);
            IEnvBuilder AddSample(IEnvDataSample sample);
        }

        public interface ISampleBuilder : IEntityBuilder<IEnvDataSample>
        {
            public enum MeasurementZone
            {
                HotGlass, HotMat, MidGlass, ColdGlass, ColdMat
            }

            public ISampleBuilder AddMeasurement(MeasurementZone zone, double value);
            
            // pass through methods.
            public ISampleBuilder AddHotGlassMeasurement(double x) => AddMeasurement(MeasurementZone.HotGlass, x);
            public ISampleBuilder AddHotMatMeasurement(double x) => AddMeasurement(MeasurementZone.HotMat, x);
            public ISampleBuilder AddMidGlassMeasurement(double x) => AddMeasurement(MeasurementZone.MidGlass, x);
            public ISampleBuilder AddColdGlassMeasurement(double x) => AddMeasurement(MeasurementZone.ColdGlass, x);
            public ISampleBuilder AddColdMatMeasurement(double x) => AddMeasurement(MeasurementZone.ColdMat, x);

            public ISampleBuilder SetInhabitant(IPet? inhabitant);
            public ISampleBuilder SetEnvironment(IEnvironment environment);
        }

        public interface IPetBuilder : IEntityBuilder<IPet>
        {
            IPetBuilder SetName(string name);
            IPetBuilder SetSpecies(ISpecies species);
            IPetBuilder SetMorph(string morph);
            IPetBuilder SetHatchDate(DateTime date);
            IPetBuilder SetOwner(IUser user);
        }

        public interface ISpeciesBuilder : IEntityBuilder<ISpecies>
        {
            ISpeciesBuilder SetName(string name);
            ISpeciesBuilder SetScientificName(string name);
            ISpeciesBuilder SetLatitude(double lat);
            ISpeciesBuilder SetLongitude(double lng);
        }

        public interface IControllerBuilder : IEntityBuilder<IController>
        {
            IControllerBuilder SetId(Guid id);
        }

        public interface IBlobRecordBuilder : IEntityBuilder<IBlobRecord>
        {
            IBlobRecordBuilder SetCategory(string category);
            IBlobRecordBuilder SetName(string blobName);
        }
        
        /// <summary>
        /// Gets a new instance of a UserBuilder that the caller can use with impunity
        /// (Not a property for this reason)
        ///
        /// NOTE: Implementations of IUserBuilder are not required to be usable for multiple constructions.
        /// </summary>
        /// <returns></returns>
        IUserBuilder GetUserBuilder();
        
        /// <summary>
        /// Gets a new instance of IEnvBuilder that can be used with out conflict.
        /// NOTE: Builder specification does ensure re-usability after a call to `Build`
        /// </summary>
        /// <returns></returns>
        IEnvBuilder GetEnvironmentBuilder();
        
        /// <summary>
        /// Gets a new instance of IPetBuilder that can be used with out conflict.
        /// NOTE: Builder specification does ensure re-usability after a call to `Build`
        /// </summary>
        /// <returns></returns>
        IPetBuilder GetPetBuilder();

        /// <summary>
        /// Gets a new instance of ISampleBuilder that can be used with out conflict
        /// NOTE: Builder specification does not ensure re-usability after a call to `Build`
        /// </summary>
        /// <returns></returns>
        ISampleBuilder GetSampleBuilder();

        /// <summary>
        /// Gets a new instance of ISpeciesBuilder that can be used with out conflict
        /// NOTE: Builder specification does not ensure re-usability after a call to `Build`
        /// </summary>
        /// <returns></returns>
        ISpeciesBuilder GetSpeciesBuilder();

        /// <summary>
        /// Gets a new instance of IControllerBuilder that can be used with out conflict
        /// NOTE: Builder specification does not ensure re-usability after a call to `Build`
        /// </summary>
        /// <returns></returns>
        IControllerBuilder GetControllerBuilder();

        /// <summary>
        /// Gets a new instance of IBlobEntryBuilder that can be used with out conflict.
        /// NOTE: Builder specification does not ensure re-usability after a call to `Build`.
        /// </summary>
        /// <returns></returns>
        IBlobRecordBuilder GetBlobRecordBuilder();
    }
}