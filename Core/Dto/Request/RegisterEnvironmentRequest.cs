using System;
using Viv2.API.Core.Dto.Response;
using Viv2.API.Core.Interfaces;

namespace Viv2.API.Core.Dto.Request
{
    public class RegisterEnvironmentRequest : IUseCaseRequest<NewEntityResponse<Guid>>
    {
        public enum Mode
        {
            /// <summary>
            /// Create only if not present, otherwise ensure an association with the current user
            /// Using this mode will only error if the environment is already registered and not associated
            /// to the specified user. 
            /// </summary>
            Touch, 
            
            /// <summary>
            /// Create the entry, erroring if it already exists.
            /// </summary>
            Create
        }
        
        public Mode CreateMode { get; set; }
        
        /// <summary>
        /// Guid of the user to which this environment is being registered.
        /// </summary>
        public Guid Owner { get; set; }
        
        /// <summary>
        /// The hardware level Guid of the Environment being registered.
        /// </summary>
        public Guid MfgId { get; set; }
        
        /// <summary>
        /// User-given description of the environment.
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Generic model specific information about the environment.
        /// (which MCU is in use, its version, enclosure information, etc.)
        /// </summary>
        public string Model { get; set; }
    }
}