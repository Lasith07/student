using System.Security.Claims;
using System.Text;
using DemoAPI.DTOs.Responses;
using DemoAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using DemoAPI.DTOs.Requests;

namespace DemoAPI.Helpers.Utils
{
    public static class JwtUtils
    {
        private static string secret = "3hO4Lash4CzZfk0Ga6yQhd48208RGTAu";

        public static string GenerateJwtToken(UserModel user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);

            var claims = new List<Claim>
            {
                new Claim("user_id", user.userid.ToString()),
                new Claim("username", user.username),
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var jwtToken = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(jwtToken);
        }

        public static BaseResponse Authenticate(AuthenticateRequest request)
        {
            try
            {
                using var dbContext = new ApplicationDbContext();

                var user = dbContext.User.FirstOrDefault(u => u.username == request.Username);
                if (user == null)
                {
                    return new BaseResponse(StatusCodes.Status401Unauthorized, new MessageDTO("Invalid Username or Password"));
                }

                string md5Password = Supports.GenerateMD5(request.Password);
                if (user.password != md5Password)
                {
                    return new BaseResponse(StatusCodes.Status401Unauthorized, new MessageDTO("Invalid Username or Password"));
                }

                string jwt = GenerateJwtToken(user);

                var loginDetail = dbContext.LoginDetails.FirstOrDefault(ld => ld.user_id == user.userid);

                if (loginDetail == null)
                {
                    loginDetail = new LoginDetailModel
                    {
                        user_id = user.userid,
                        Token = jwt
                    };
                    dbContext.LoginDetails.Add(loginDetail);
                }
                else
                {
                    loginDetail.Token = jwt;
                }

                dbContext.SaveChanges();

                return new BaseResponse(StatusCodes.Status200OK, new { token = jwt });
            }
            catch (Exception ex)
            {
                return new BaseResponse(StatusCodes.Status500InternalServerError, new MessageDTO(ex.Message));
            }
        }

        public static bool ValidateJwtToken(string jwt)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(secret);

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };

                tokenHandler.ValidateToken(jwt, validationParameters, out SecurityToken validatedToken);

                var validatedJWT = (JwtSecurityToken)validatedToken;

                long userId = long.Parse(validatedJWT.Claims.First(claim => claim.Type == "user_id").Value);

                using var dbContext = new ApplicationDbContext();
                return dbContext.User.Any(u => u.userid == userId);
            }
            catch
            {
                return false;
            }
        }
    }
}
