using Viv2.API.Core.ProtoEntities;

namespace Viv2.API.Infrastructure.DataStore.EfNpgSql.Entities
{
    public class BlobRecord : IBlobRecord
    {
        public long Id { get; set; }
        public string Category { get; set; }
        public string BlobName { get; set; }
        public string MimeType { get; set; }
    }
}