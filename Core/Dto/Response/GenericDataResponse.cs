using System.Collections.Generic;

namespace Viv2.API.Core.Dto.Response
{
    public class GenericDataResponse<TDataType>
    {
        public ICollection<TDataType> Result { get; set; } 
    }
}