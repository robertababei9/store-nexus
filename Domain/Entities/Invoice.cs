using Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Invoice : BaseEntity<Guid>
    {
        public Guid CompanyId { get; set; }
        public string InvoiceNo { get; set; }
        public string BillTo { get; set; }
        public string BillToEmail { get; set; }
        public string BillToAddress { get; set; }
        public string BillFrom{ get; set; }
        public string BillFromEmail { get; set; }
        public string BillFromAddress { get; set; }

        public int Tax { get; set; }
        public int Discount { get; set; }
        public int Total { get; set; }

        public DateOnly DueDate{ get; set; }
        public string? Notes { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RecordId { get; set; }   // auto-increment


        public virtual ICollection<InvoiceItem> InvoiceItems { get; set; }
        public virtual Company Company{ get; set; }
    }
}
