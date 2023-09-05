using Infrastructure.Repositories.Contracts;
using Domain.Dto;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.Invoices
{
    public static class GetById
    {
        // Query
        public record Query(Guid id) : IRequest<Response>;

        // Handler
        public class Handler : IRequestHandler<Query, Response>
        {
            protected IInvoiceRepository _invoiceRepository { get; set; }
            public Handler(IInvoiceRepository invoiceRepository)
            {
                _invoiceRepository = invoiceRepository;
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                var invoiceDataForm = await _invoiceRepository.GetInvoiceForm(request.id);

                return new Response(invoiceDataForm);
            }
        }

        // Response
        public record Response(InvoiceFormDto invoiceData);
    }
}
