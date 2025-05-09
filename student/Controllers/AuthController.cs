using Microsoft.AspNetCore.Mvc;
using DemoAPI.DTOs.Requests;
using DemoAPI.DTOs.Responses;
using DemoAPI.Services.UserService;
using DemoAPI.Helpers.Utils;
using DemoAPI.Models;
using DemoAPI.DTOs;

namespace DemoAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

       
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequest registerRequest)
        {
            if (registerRequest == null)
            {
                return BadRequest(new BaseResponse(StatusCodes.Status400BadRequest, new MessageDTO("Invalid request data")));
            }

           
            var result = _userService.Register(registerRequest);

          
            return Ok(result);  
        }

        
    }
}
