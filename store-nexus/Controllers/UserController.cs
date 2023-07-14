using Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace store_nexus.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;

        public UserController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        [HttpGet]
        public async  Task<IActionResult> GetAll()
        {
            var users = await _unitOfWork.User.All();

            return Ok(users);
        }
    }
}
