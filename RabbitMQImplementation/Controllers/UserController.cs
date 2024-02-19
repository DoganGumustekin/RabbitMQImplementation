using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RabbitMQImplementation.Models;
using RabbitMQImplementation.Repository.Producers.IServices;

namespace RabbitMQImplementation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("UserRegister")]
        public IActionResult Register(UserModel param)
        {
            _userService.register(param);
            return Ok();
        }

    }
}
