using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class InvoiceItem : BaseEntity<Guid>
    {
        public Guid InvoiceId{ get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public int Qty { get; set; }
        public int Price { get; set; }

        [JsonIgnore]
        public virtual Invoice Invoice { get; set; }
    }
}
