using Infrastructure.Repositories.Contracts;
using Domain.Dto;
using Domain.Entities;
using MediatR;
using Common.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands.Invoices
{
    public static class CreateInvoice
    {
        // Command
        public record Command(InvoiceFormDto invoiceForm) : IRequest<Guid>;

        // Handler
        public class Handler : IRequestHandler<Command, Guid>
        {
            protected IInvoiceRepository _invoiceRepository { get; set; }
            protected IInvoiceItemRepository _invoiceItemRepository{ get; set; }

            public Handler(IInvoiceRepository invoiceRepository, IInvoiceItemRepository invoiceItemRepository)
            {
                _invoiceRepository = invoiceRepository;
                _invoiceItemRepository = invoiceItemRepository;
            }


            public async Task<Guid> Handle(Command request, CancellationToken cancellationToken)
            {
                int invoiceNo = 1;
                var lastInveoice = await _invoiceRepository
                    .GetAllQueryable()
                    .OrderByDescending(x => x.RecordId)
                    .FirstOrDefaultAsync();

                if (lastInveoice != null)
                {
                    invoiceNo = lastInveoice.RecordId + 1;
                }

                DateTime dueDate = DateTime.Parse(request.invoiceForm.DueDate);


                Invoice invoice = new Invoice
                {
                    CompanyId = Guid.Parse("E65423FD-811C-4492-B578-23F7353CD1D9"), // To retrieve it here on the server based on the loggedin user
                    DueDate = DateOnly.FromDateTime(dueDate),
                    InvoiceNo = InvoiceHelper.GenerateInvoiceNumber(invoiceNo),
                    BillTo = request.invoiceForm.BillTo.To,
                    BillToAddress = request.invoiceForm.BillTo.Address,
                    BillToEmail = request.invoiceForm.BillTo.Email,
                    BillFrom = request.invoiceForm.BillFrom.From,
                    BillFromAddress = request.invoiceForm.BillFrom.Address,
                    BillFromEmail = request.invoiceForm.BillFrom.Email,
                    Notes = request.invoiceForm.Notes,
                    Tax = request.invoiceForm.Tax,
                    Discount = request.invoiceForm.Discount,
                    Total = request.invoiceForm.Total
                };

                await _invoiceRepository.AddAsync(invoice);
                await _invoiceRepository.SaveChangesAsync();

                foreach (var item in request.invoiceForm.Items)
                {
                    var invoiceItem = new InvoiceItem
                    {
                        InvoiceId = invoice.Id,
                        Title = item.Name,
                        Description = item.Description,
                        Qty = item.Qty,
                        Price = item.Price
                    };

                    await _invoiceItemRepository.AddAsync(invoiceItem);
                }

                await _invoiceItemRepository.SaveChangesAsync();


                return invoice.Id;
            }
        }

    }
}
