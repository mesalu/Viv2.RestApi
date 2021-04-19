namespace Viv2.API.Core.Dto.Response
{
    public class GenericDataResponse <TDataType>
    {
        public TDataType Result { get; set; } 
    }
}