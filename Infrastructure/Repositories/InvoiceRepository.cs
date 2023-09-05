using Domain.Dto;
using Domain.Entities;
using Infrastructure.GenericRepository;
using Infrastructure.Persistence;
using Infrastructure.Repositories.Contracts;
using Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class InvoiceRepository : GenericRepository<Invoice>, IInvoiceRepository
    {
        public InvoiceRepository(IUnitOfWork uow, ApplicationContext context) : base(uow, context)
        {

        }

        public async Task<InvoiceFormDto> GetInvoiceForm(Guid invoiceId)
        {
            var result = await GetAllQueryable()
                            .Include(x => x.InvoiceItems)
                    .   Where(x => x.Id == invoiceId)
                        .FirstOrDefaultAsync();

            if (result == null)
            {
                return new InvoiceFormDto();
            }

            decimal subtotal = 0;
            foreach (var item in result.InvoiceItems)
            {
                subtotal += (item.Qty * item.Price);
            }
            decimal taxFromSubtotal = (result.Tax / (decimal)100) * subtotal;
            decimal discountFromSubtotal = (result.Discount / (decimal)100) * subtotal;

            InvoiceFormDto invoiceData = new InvoiceFormDto
            {
                CreatedDate = result.CreatedAt.ToString("dd-MMM-yyyy"),
                DueDate = result.DueDate.ToString("dd-MMM-yyyy"),
                InvoiceNo = result.InvoiceNo,
                BillTo = new BillTo
                {
                    To = result.BillTo,
                    Address = result.BillToAddress,
                    Email = result.BillToEmail,
                },
                BillFrom = new BillFrom
                {
                    From = result.BillFrom,
                    Address = result.BillFromAddress,
                    Email = result.BillFromEmail,
                },
                Items = result.InvoiceItems.Select(y => new ItemType
                {
                    Name = y.Title,
                    Description = y.Description,
                    Qty = y.Qty,
                    Price = y.Price
                }),
                Notes = result.Notes,
                Tax = result.Tax,
                Discount = result.Discount,
                TaxSubtotal = taxFromSubtotal,
                DiscountSubtotal = discountFromSubtotal,
                Subtotal = (int)subtotal,
                Total = result.Total,
            };

            return invoiceData;
        }
    }
}
