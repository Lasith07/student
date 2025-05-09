using System.ComponentModel.DataAnnotations;

namespace DemoAPI.Models
{
    public class UserModel
    {

        [Key]
        public long userid { get; set; } 
        public string username { get; set; }
        public string password { get; set; } 
    }
}
