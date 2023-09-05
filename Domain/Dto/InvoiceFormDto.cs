using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dto
{
    public class InvoiceFormDto
    {

        public InvoiceFormDto()
        {
            Items = new List<ItemType>();
        }


        public string CreatedDate { get; set; }
        public string DueDate { get; set; }
        public string InvoiceNo { get; set; }
        public BillTo BillTo { get; set; }
        public BillFrom BillFrom { get; set; }
        public IEnumerable<ItemType> Items{ get; set; }
        public string Notes{ get; set; }
        public int Tax { get; set; }
        public int Discount { get; set; }
        public decimal TaxSubtotal { get; set; }
        public decimal DiscountSubtotal { get; set; }
        public int Subtotal { get; set; }
        public int Total { get; set; }
    }


    public class BillTo
    {
        public string To { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
    }

    public class BillFrom
    {
        public string From { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
    }

    public class ItemType
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Qty { get; set; }
        public int Price { get; set; }
    }

}
