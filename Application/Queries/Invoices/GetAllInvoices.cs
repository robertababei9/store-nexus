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
                    .Select(x => x)
                    .ToList();

                return new Response(result);
            }
        }



        // Response
        public record Response(IEnumerable<Invoice> data);
    }
}

