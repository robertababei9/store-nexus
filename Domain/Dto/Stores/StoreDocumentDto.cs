using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dto.Stores
{
    public class StoreDocumentDto
    {
        public string Name { get; set; }
        public string UploadedBy { get; set; }
        public string UploadedAt { get; set; }
        public string Uri { get; set; }
    }
}
