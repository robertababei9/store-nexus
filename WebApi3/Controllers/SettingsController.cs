using Application.Queries.Stores;
using Authorization.Attributes;
using Domain.Entities;
using Infrastructure.Email;
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

        private readonly IMailService _mailService;

        public SettingsController(
            ILogger<InvoicesController> logger,
            IMediator mediator,
            IMailService mailService
        )
        {
            _logger = logger;
            _mediator = mediator;
            _mailService = mailService;
        }


        #region ROLES
        [PermissionsAuthorize(nameof(RolePermissions.Settings))]
        [HttpGet("[action]/{roleId}")]
        public async Task<IActionResult> GetRolePermissions(Guid roleId)
        {
            var result = await _mediator.Send(new GetRolePermissions.Query(roleId));

            return Ok(result.response);
        }

        #endregion
    }
}
