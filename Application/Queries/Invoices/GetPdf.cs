using Infrastructure.Repositories.Contracts;
using System.IO;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Common.Helpers;

namespace Application.Queries.Invoices
{

    public static class GetPdf
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
                var invoiceFormData = await _invoiceRepository.GetInvoiceForm(request.id);

                var pdfBytes = HtmlToPdfHelper.GetPdfFromHtmlByInvoiceData(invoiceFormData);

                return new Response(pdfBytes);
            }
        }



        // Response
        public record Response(byte[] pdfBytes);
    }
}
