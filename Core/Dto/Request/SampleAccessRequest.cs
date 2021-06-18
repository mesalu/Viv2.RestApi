using System;
using Viv2.API.Core.Dto.Response;
using Viv2.API.Core.Interfaces;
using Viv2.API.Core.ProtoEntities;

namespace Viv2.API.Core.Dto.Request
{
    public class SampleAccessRequest : IUseCaseRequest<GenericDataResponse<IEnvDataSample>>
    {
        /// <summary>
        /// Describes what criteria by which samples should be discovered.
        /// </summary>
        public enum SelectionCriteria
        {
            Pet, Environment, User
        }

        /// <summary>
        /// Specifies the entity type that should be used to filter for samples. 
        /// </summary>
        public SelectionCriteria Selector { get; init; }
            
        /// <summary>
        /// The user making the request.
        /// Also used if SelectionCriteria is set to User.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Used when Selector is set to Env.
        /// </summary>
        public Guid EnvId { get; set; }
        
        /// <summary>
        /// Used when Selector is set to Pet.
        /// The ID of the pet whose samples should be loaded.
        /// </summary>
        public int PetId { get; set; }
        
        /// <summary>
        /// Inclusive start time for the window in which samples are being located.
        /// </summary>
        public DateTime RangeStart { get; set; }
        
        /// <summary>
        /// Exclusive end time for the window in which samples are being located.
        /// </summary>
        public DateTime RangeEnd { get; set; }
    }
}