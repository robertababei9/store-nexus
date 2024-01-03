using Application.Commands.Company;
using Application.Queries.Company;
using Domain.Dto.CompanyDto;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class CompanyController : ControllerBase
    {
        private readonly ILogger<InvoicesController> _logger;
        private readonly IMediator _mediator;

        public CompanyController(
            ILogger<InvoicesController> logger,
            IMediator mediator
        )
        {
            _logger = logger;
            _mediator = mediator;
        }

        #region GET
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var response = await _mediator.Send(new GetById.Query(id));

            return Ok(response.response);
        }
        #endregion


        #region POST
        [HttpPost("[action]")]
        public async Task<IActionResult> Create([FromBody] CreateCompanyDto createCompanyDto)
        {
            var response = await _mediator.Send(new CreateCompany.Command(createCompanyDto));

            return Ok(response);
        }
        #endregion
    }
}
