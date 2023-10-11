using Application.Commands.Users;
using Application.Queries.Users;
using Authentication.Services;
using Domain.Dto;
using Domain.Dto.Users;
using Domain.Entities;
using Infrastructure.UnitOfWork;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PasswordHashExample.WebAPI.Resources;

namespace store_nexus.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IMediator _mediator;
        private readonly IUserService _userService;

        public UsersController(
            ILogger<UsersController> logger,
            IMediator mediator,
            IUserService userService
        )
        {
            _logger = logger;
            _mediator = mediator;
            _userService = userService;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _mediator.Send(new GetAllUsers.Query());

            return Ok(response.data);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllManagersOptions()
        {
            var result = await _mediator.Send(new GetAllManagers.Query());

            return Ok(result.response);
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var response = await _mediator.Send(new GetById.Query(id));

            return Ok(response.userData);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Add([FromBody] UserToAddDto user)
        {
            var response = await _mediator.Send(new AddUser.Command(user));
            if (response == Guid.Empty)
            {
                return BadRequest("Email address is already in use");
            }

            return Ok(response);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetUserRoles()
        {
            var response = await _mediator.Send(new GetUserRoles.Query());

            return Ok(response.data);
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> Edit([FromBody] UsersDto user)
        {
            var response = await _mediator.Send(new EditUser.Command(user));

            if (response)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(new { message = "Failed to update user" });
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordModel)
        {
            var response = await _mediator.Send(new ChangePassword.Command(changePasswordModel));
            
            return Ok(response);

        }



        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginResource loginModel)
        {
            try
            {
                var loginResult = await _userService.Login(loginModel, CancellationToken.None);

                return Ok(loginResult);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterResource registerModel)
        {
            try
            {
                var result = await _userService.Register(registerModel, CancellationToken.None);

                return Created(nameof(Register), result.Id);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
