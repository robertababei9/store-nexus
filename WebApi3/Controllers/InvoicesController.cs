using Application.Commands.Invoices;
using Application.Queries.Invoices;
using Common.Helpers;
using Domain.Dto;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoicesController : ControllerBase
    {
        private readonly ILogger<InvoicesController> _logger;
        private readonly IMediator _mediator;

        public InvoicesController(
            ILogger<InvoicesController> logger,
            IMediator mediator
        )
        {
            _logger = logger;
            _mediator = mediator;
        }




        [HttpGet("{id}")]
        public async Task<IActionResult> GetAll(Guid id)
        {
            var response = await _mediator.Send(new GetById.Query(id));

            if (response.invoiceData != null)
            {
                return Ok(response.invoiceData);
            }

            return BadRequest();
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetPdf(Guid id)
        {
            var response = await _mediator.Send(new GetPdf.Query(id));

            var resultPdf = File(response.pdfBytes, "application/pdf", "pdf_test.pdf");

            return Ok(resultPdf);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _mediator.Send(new GetAllInvoices.Query());

            return Ok(response.data);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Add([FromBody] InvoiceFormDto invoiceForm)
        {
            var response = await _mediator.Send(new CreateInvoice.Command(invoiceForm));

            return Ok(response);
        }
    }
}
