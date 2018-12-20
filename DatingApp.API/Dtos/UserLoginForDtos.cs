using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.Dtos
{
    public class UserLoginForDtos
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}