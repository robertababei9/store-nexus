﻿using Application.Commands.Users;
using Application.Queries.Users;
using Authentication.Services;
using Domain.Dto;
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

        [HttpPost("[action]")]
        public async Task<IActionResult> Add([FromBody] UsersDto user)
        {
            var response = await _mediator.Send(new AddUser.Command(user));

            return Ok(response);
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



        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginResource loginModel)
        {
            try
            {
                var token = await _userService.Login(loginModel, CancellationToken.None);

                return Ok(token);
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
