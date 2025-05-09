using DemoAPI.DTOs;
using DemoAPI.DTOs.Requests;
using DemoAPI.DTOs.Responses;
using DemoAPI.Services.UserService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DemoAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] AuthenticateRequest request)
        {
            var response = _userService.Authenticate(request);
            if (response.status_code == StatusCodes.Status200OK)
            {
                return Ok(response);
            }
            return Unauthorized(response);
        }

        

        [HttpGet("{userId}")]
        public IActionResult GetUserById(long userId)
        {
            var response = _userService.GetUserById(userId);
            if (response.status_code == StatusCodes.Status200OK)
            {
                return Ok(response);
            }
            return NotFound(response);
        }
    }
}
