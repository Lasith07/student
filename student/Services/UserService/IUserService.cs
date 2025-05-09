using DemoAPI.DTOs;
using DemoAPI.DTOs.Requests;
using DemoAPI.DTOs.Responses;

namespace DemoAPI.Services.UserService
{
    public interface IUserService
    {
        BaseResponse Authenticate(AuthenticateRequest request);
        BaseResponse Register(RegisterRequest registerRequest);
        BaseResponse GetUserById(long userId);
        
    }
}
