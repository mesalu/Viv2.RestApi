using System;
using Viv2.API.Core.Interfaces;
using Viv2.API.Core.Dto.Response;

namespace Viv2.API.Core.Dto.Request
{
    public class DataAccessRequest<TDataType> : IUseCaseRequest<GenericDataResponse<TDataType>>
    {
        public enum AcquisitionStrategy
        {
            /// <summary>
            /// Specifies requesting all elements of the requested data - if it is a collection
            /// </summary>
            All,
        }
        
        /// <summary>
        /// Specifies the user on behalf of which the data access is being requested.
        /// </summary>
        public Guid UserId { get; set; }
        
        /// <summary>
        /// The strategy to use in accessing data. Currently only uses the `All` mode.
        /// </summary>
        public AcquisitionStrategy Strategy { get; set; }
    }
}