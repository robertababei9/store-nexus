using Application.Commands.Stores;
using Application.Commands.Users;
using Application.Queries.Stores;
using Application.Services.FileService;
using Domain.Dto.Stores;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class SettingsController : ControllerBase
    {
        private readonly ILogger<InvoicesController> _logger;
        private readonly IMediator _mediator;

        public SettingsController(
            ILogger<InvoicesController> logger,
            IMediator mediator
        )
        {
            _logger = logger;
            _mediator = mediator;
        }

        #region ROLES
        [AllowAnonymous]
        [HttpGet("[action]/{roleId}")]
        public async Task<IActionResult> GetRolePermissions(Guid roleId)
        {
            var result = await _mediator.Send(new GetRolePermissions.Query(roleId));

            return Ok(result.response);
        }

        #endregion
    }
}
