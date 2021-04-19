using System;
using Viv2.API.Core.Interfaces;
using Viv2.API.Core.Dto.Response;

namespace Viv2.API.Core.Dto.Request
{
    public class DataAccessRequest<TDataType> : IUseCaseRequest<GenericDataResponse<TDataType>>
    {
        /// <summary>
        /// Specifies the user on behalf of which the data access is being requested.
        /// </summary>
        public Guid UserId { get; set; }
    }
}