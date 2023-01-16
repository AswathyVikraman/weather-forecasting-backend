using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Projectwork.Authentication;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace Projectwork.Controllers
{
  [ApiController]
  [Route("api/[Controller]")]
  public class UserController : Controller
  {
    private readonly ApplicationDbContext _applicationDbContext;
    public UserController(ApplicationDbContext _applicationDbContext1)
    {
      _applicationDbContext = _applicationDbContext1;
    }
    [HttpPost("authenticate")]
    public async Task<IActionResult> Authenticate([FromBody] User userObj)
    {
      if (userObj == null)
        return BadRequest();
      var user = await _applicationDbContext.User
                .FirstOrDefaultAsync(x => x.Username == userObj.Username && x.Password == userObj.Password);
      if (user == null)
      {
        return NotFound(new { Message = "user not found!!!!!!" });
      }
      user.Token = CreateJwtToken(user);

      return Ok(new
      {
        Token = user.Token,
        Message = "login success!!!!!!!"
      });
    }
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] User userObj)
    {
      if (userObj == null)
        return BadRequest();
      //check username


      if (await CheckUsenameExist(userObj.Username))
      {
        return BadRequest(new { Message = "username already exist!!!!!!!!!!" });
      }

      //check email

      if (await CheckEmailExist(userObj.Email))
      {
        return BadRequest(new { Message = "email already exist!!!!!!!!!!" });
      }

      //check password srength

      var pass = CheckPasswordStrength(userObj.Password);
      if (!string.IsNullOrEmpty(pass))
        return BadRequest(new { Message = pass.ToString() });

      await _applicationDbContext.User.AddAsync(userObj);
      await _applicationDbContext.SaveChangesAsync();
      return Ok(new
      {
        Message = "user registered!!!!!!!!"
      });
    }
    private Task<bool> CheckUsenameExist(string username)

        => _applicationDbContext.User.AnyAsync(x => x.Username == username);
    private Task<bool> CheckEmailExist(string email)

        => _applicationDbContext.User.AnyAsync(x => x.Email == email);
    private string CheckPasswordStrength(string password)
    {
      StringBuilder sb = new StringBuilder();
      if (password.Length < 8)
        sb.Append("minimum password length should be 8" + Environment.NewLine);
      if (!(Regex.IsMatch(password, "[a-z]") && Regex.IsMatch(password, "[A-Z]") && Regex.IsMatch(password, "[0-9]")))
        sb.Append("password should be alphanumeric!!!!!!!!!" + Environment.NewLine);
      if (!Regex.IsMatch(password, "[<,>,@,!,#,$,%,^,&,*,(,),_,+,\\[,\\],{,},?,:,;,|,',\\,.,/,~,`,-,=,]"))
        sb.Append("password should contain special character!!!!!!!!!" + Environment.NewLine);
      return sb.ToString();

    }
    private string CreateJwtToken(User user)
    {
      var jwtTokenHandler = new JwtSecurityTokenHandler();
      var key = Encoding.ASCII.GetBytes("veryverysecret.....");
      var identity = new ClaimsIdentity(new Claim[]
      {
                new Claim(ClaimTypes.Role,user.Role),
                new Claim(ClaimTypes.Name,$"{user.Username}")
      });
      var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = identity,
        Expires = DateTime.Now.AddDays(1),
        SigningCredentials = credentials
      };
      var token = jwtTokenHandler.CreateToken(tokenDescriptor);
      return jwtTokenHandler.WriteToken(token);
    }
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<User>> GetAllUser()
    {
      return Ok(await _applicationDbContext.User.ToListAsync());
    }
  }
}
