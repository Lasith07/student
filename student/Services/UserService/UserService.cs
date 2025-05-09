using DemoAPI.DTOs;
using DemoAPI.DTOs.Responses;
using DemoAPI.Models;
using DemoAPI.Helpers.Utils;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using DemoAPI.DTOs.Requests;

namespace DemoAPI.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _dbContext;

        public MessageDTO User { get; private set; }

        public UserService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public BaseResponse Authenticate(AuthenticateRequest request)
        {
            try
            {
                var user = _dbContext.User.FirstOrDefault(u => u.username == request.Username);
                if (user == null)
                {
                    return new BaseResponse(StatusCodes.Status401Unauthorized, new MessageDTO("Invalid Username or Password"));
                }

                string md5Password = Supports.GenerateMD5(request.Password);
                if (user.password != md5Password)
                {
                    return new BaseResponse(StatusCodes.Status401Unauthorized, new MessageDTO("Invalid Username or Password"));
                }

               
                string jwt = JwtUtils.GenerateJwtToken(user);

                
                return new BaseResponse(StatusCodes.Status200OK, new MessageDTO(jwt));
            }
            catch (Exception ex)
            {
                var inner = ex.InnerException?.Message ?? "No inner exception";
                return new BaseResponse(StatusCodes.Status500InternalServerError, new MessageDTO($"Error: {ex.Message}. Inner: {inner}"));
            }
        }


        public BaseResponse Register(RegisterRequest registerRequest)
        {
            try
            {
                var existingUser = _dbContext.User.FirstOrDefault(u => u.username == registerRequest.Username);
                if (existingUser != null)
                {
                    return new BaseResponse(StatusCodes.Status400BadRequest, new MessageDTO("Username already exists"));
                }

                string hashedPassword = Supports.GenerateMD5(registerRequest.Password);

                var newUser = new UserModel
                {
                    username = registerRequest.Username,
                    password = hashedPassword,
                };

                _dbContext.User.Add(newUser);
                _dbContext.SaveChanges();

                return new BaseResponse(StatusCodes.Status201Created, new MessageDTO("User registered successfully"));
            }
            catch (Exception ex)
            {
                return new BaseResponse(StatusCodes.Status500InternalServerError, new MessageDTO(ex.Message));
            }
        }

        

        public object Authenticate(LoginRequest loginRequest)
        {
            throw new NotImplementedException();
        }

        public BaseResponse GetUserById(long userId)
        {
            throw new NotImplementedException();
        }
    }
}
