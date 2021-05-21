namespace Viv2.API.Core.ProtoEntities
{
    public interface IBlobRecord
    {
        public long Id { get; set; }
        
        /// <summary>
        /// Basic classifier for where teh blob is, analogous to containers in azure blob storage. 
        /// </summary>
        public string Category { get; set; } 
        
        /// <summary>
        /// The name of the blob within the specified category, must be unique to the category.
        /// </summary>
        public string BlobName { get; set; }
        
        public string MimeType { get; set; }
    }
}