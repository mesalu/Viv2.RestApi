using System;
using System.Linq.Expressions;
using Viv2.API.Core.Interfaces;
using Viv2.API.Core.Dto.Response;

namespace Viv2.API.Core.Dto.Request
{
    /// <summary>
    /// Describes a request for data from Core.
    /// The response is provided on a GenericDataResponse instance.
    /// Response data is always contained in an ICollection with generic parameter
    /// TDataType.
    /// </summary>
    /// <typeparam name="TDataType"></typeparam>
    public class DataAccessRequest<TDataType> : IUseCaseRequest<GenericDataResponse<TDataType>>
    {
        public enum AcquisitionStrategy
        {
            /// <summary>
            /// Specifies requesting all elements of the requested data - if it is a collection
            /// </summary>
            All, Single, Range
        }
        
        /// <summary>
        /// Specifies the user on behalf of which the data access is being requested.
        /// </summary>
        public Guid UserId { get; set; }
        
        /// <summary>
        /// The strategy to use in accessing data. Currently only uses the `All` mode.
        /// </summary>
        public AcquisitionStrategy Strategy { get; set; }
        
        /// <summary>
        /// Specifies how to acquire data.
        /// In the event that Strategy is `Single`, then the result will be
        /// akin to a call to 'SomeCollection.FirstOrDefault(this.SelectionPredicate)`
        /// If the strategy is 'Range' then the result will be akin to a call
        /// to `SomeCollection.Select(this.SelectionPredicate)
        /// </summary>
        public Func<TDataType, bool> SelectionPredicate { get; init; }
        
    }
}