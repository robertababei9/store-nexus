using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dto
{
    public class InvoiceDto
    {
        public string InvoiceNo { get; set; }
        public string BillFrom { get; set; }
        public string BillTo { get; set; }
        public string CreatedDate { get; set; }
        public string DueDate{ get; set; }
        public string Status { get; set; }
        public int Total { get; set; }
        public int Tax { get; set; }
        public int Discount { get; set; }

    }
}
