using System;

namespace DemoAPI.Models
{
    public class LoginDetailModel
    {
        
        public long id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public long user_id { get; set; }
        public string Token { get; set; }

        public UserModel User { get; set; }

        public LoginDetailModel()
        {
        }
    }
}
