using Microsoft.AspNetCore.Http;

namespace Domain.Dto.Stores
{
    public class UploadFileDto
    {
        public Guid StoreId { get; set; }
        public IFormFile Blob { get; set; }
    }
}
