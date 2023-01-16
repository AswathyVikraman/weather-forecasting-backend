//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.IdentityModel.Tokens;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;
//using Projectwork.Controllers.Authentication;
//using Projectwork.Authentication;
//using Microsoft.EntityFrameworkCore;
//using Projectwork.Model;

//namespace Projectwork.Controllers
//{
//  [Route("api/[controller]")]
//  [ApiController]
//  public class AuthenticateController : ControllerBase
//  {
//    private readonly UserManager<ApplicationUser> userManager;
//    private readonly RoleManager<IdentityRole> roleManager;
//    private readonly IConfiguration _configuration;
//    private readonly ApplicationDbContext _applicationDbContext;

//    public AuthenticateController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, ApplicationDbContext applicationDbContext)
//    {
//      this.userManager = userManager;
//      this.roleManager = roleManager;
//      _configuration = configuration;
//      _applicationDbContext = applicationDbContext;
//    }

//    [HttpPost]
//    [Route("login")]
//    public async Task<IActionResult> Login([FromBody] LoginModel model)
//    {
//      var user = await userManager.FindByNameAsync(model.Username);
//      if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
//      {
//        var userRoles = await userManager.GetRolesAsync(user);

//        var authClaims = new List<Claim>
//                {
//                    new Claim(ClaimTypes.Name, user.UserName),
//                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
//                };

//        foreach (var userRole in userRoles)
//        {
//          authClaims.Add(new Claim(ClaimTypes.Role, userRole));
//        }

//        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

//        var token = new JwtSecurityToken(
//            issuer: _configuration["JWT:ValidIssuer"],
//            audience: _configuration["JWT:ValidAudience"],
//            expires: DateTime.Now.AddHours(5),
//            claims: authClaims,
//            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
//            );

//                return Ok(new
//                {
//                    token = new JwtSecurityTokenHandler().WriteToken(token),
//                    Message = "Login Success"
//                    // expiration = token.ValidTo
//                });
//      }
//            //return Unauthorized();
//            return BadRequest(new { Message = "Unauthorized" });
//    }

//    [HttpPost]
//    [Route("register")]
//    public async Task<IActionResult> Register([FromBody] RegisterModel model)
//    {
//      var userExists = await userManager.FindByNameAsync(model.Username);
//      if (userExists != null)
//        return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

//      ApplicationUser user = new ApplicationUser()
//      {
//        Email = model.Email,
//        SecurityStamp = Guid.NewGuid().ToString(),
//        UserName = model.Username
//      };
//      var result = await userManager.CreateAsync(user, model.Password);
//      if (!result.Succeeded)
//        return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

//      return Ok(new Response { Status = "Success", Message = "User created successfully!" });
//    }

//    [HttpPost]
//    [Route("registerAdmin")]
//    public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
//    {
//      var userExists = await userManager.FindByNameAsync(model.Username);
//      if (userExists != null)
//        return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

//      ApplicationUser user = new ApplicationUser()
//      {
//        Email = model.Email,
//        SecurityStamp = Guid.NewGuid().ToString(),
//        UserName = model.Username
//      };
//      var result = await userManager.CreateAsync(user, model.Password);
//      if (!result.Succeeded)
//        return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

//      if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
//        await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
//      if (!await roleManager.RoleExistsAsync(UserRoles.User))
//        await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

//      if (await roleManager.RoleExistsAsync(UserRoles.Admin))
//      {
//        await userManager.AddToRoleAsync(user, UserRoles.Admin);
//      }

//      return Ok(new Response { Status = "Success", Message = "User created successfully!" });
//    }
//    //[HttpGet]
//    //[Route("weatherinfo")]
//    //public async Task<IActionResult> GetAllWeartherInfo()
//    //{
//    //  //return View();
//    //  var weatherinfo = await _applicationDbContext.WeatherInfo.ToListAsync();
//    //  return Ok(weatherinfo);
//    //}
//    //[HttpPost]
//    ////[Route("weatherinfo")]
//    //public async Task<IActionResult> AddWeartherInfo([FromBody] WeatherInfo weatherinfoRequest)
//    //{
//    //  weatherinfoRequest.Id = Guid.NewGuid();
//    //  await _applicationDbContext.WeatherInfo.AddAsync(weatherinfoRequest);
//    //  await _applicationDbContext.SaveChangesAsync();
//    //  return Ok(weatherinfoRequest);
//    //}

//    //private string CreateJwt(UserRoles userRoles)
//    //  {
//    //    var jwtTokenHandler=new JwtSecurityTokenHandler();
//    //    var key = Encoding.ASCII.GetBytes("ByYM000OLlMQG6VVVp1OH7Xzyr7gHuw1qvUC5dcGt3SNM");
//    //    var identity=new ClaimsIdentity(new Claim[]
//    //    {
//    //      new Claim(ClaimTypes.Role,userRoles.Admin)
//    //    })
//    //  }
//  }
//}
