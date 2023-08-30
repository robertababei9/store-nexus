using Application.Commands.Invoices;
using Application.Queries.Invoices;
using Domain.Dto;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
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
