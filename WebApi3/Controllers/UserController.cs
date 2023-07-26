using Authentication.Services;
using Domain.Entities;
using Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PasswordHashExample.WebAPI.Resources;

namespace store_nexus.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;

        public UserController(
            IUnitOfWork unitOfWork, 
            IUserService userService
        )
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _unitOfWork.User.All();

            return Ok(users);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser()
        {
            var userToAdd = new User
            {
                Name = "Robert Ababei",
                Email = "robert.ababei9@gmail.com"
            };

            await _unitOfWork.User.AddAsync(userToAdd);
            await _unitOfWork.User.SaveChangesAsync();

            return Ok(userToAdd);
        }

        // TODO: To be changed to POST method ... how tf to send the password in query ... wtf is wrong with you
        [AllowAnonymous]
        [HttpGet]
        [Route("login")]
        public async Task<IActionResult> Login([FromQuery] LoginResource loginModel)
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
