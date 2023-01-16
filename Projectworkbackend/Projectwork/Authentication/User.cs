using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Projectwork.Authentication
{
  public class User
  {
    [Key]
    public int Id { get; set; }
    //[Required(ErrorMessage = "UserName is required")]
    public string Username { get; set; }

    //[Required(ErrorMessage = "Email is required")]
    //[RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Please enter the valid Email")]
    public string Email { get; set; }

    //[Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }
    //[Required(ErrorMessage = "Please enter ConfirmPassword")]
    //[Display(Name = "confirm password ")]
    //[Compare("Password", ErrorMessage = ("confirm password can't be dismismatched!"))]
    public string ConfirmPassword { get; set; }
    public string Token { get; set; }
    public string Role { get; set; }

  }
}
