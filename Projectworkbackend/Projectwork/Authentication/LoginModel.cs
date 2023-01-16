using System.ComponentModel.DataAnnotations;

namespace Projectwork.Authentication
{
    public class LoginModel
    {
        [Required(ErrorMessage = "UserName is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    
}
}
