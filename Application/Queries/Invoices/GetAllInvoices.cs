using Infrastructure.Repositories.Contracts;
using Domain.Dto;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.Invoices
{
    public static class GetAllInvoices
    {
        // Query
        public record Query() : IRequest<Response>;


        // Handler
        public class Handler : IRequestHandler<Query, Response>
        {
            protected IInvoiceRepository _invoiceRepository { get; set; }
            public Handler(IInvoiceRepository invoiceRepository)
            {
                _invoiceRepository =  invoiceRepository;
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = _invoiceRepository
                    .GetAllQueryable()
                        .Include(x => x.InvoiceItems)
                    .ToList()
                    .Select(x => new InvoiceDto
                    {
                        Id = x.Id,
                        InvoiceNo = x.InvoiceNo,
                        BillFrom = x.BillFrom,
                        BillTo = x.BillTo,
                        CreatedDate = x.CreatedAt.ToString("dd-MMM-yyyy"),
                        DueDate = x.DueDate.ToString("dd-MMM-yyyy"),
                        Status = "Sent",    // FUTURE: maybe it could be a int -> table of invoice statuses ( but for the moment Sent / Paid are enough )
                        Total = x.Total,
                        Tax = x.Tax,
                        Discount = x.Discount
                    })
                    .ToList();

                return new Response(result);
            }
        }



        // Response
        public record Response(IEnumerable<InvoiceDto> data);
    }
}

