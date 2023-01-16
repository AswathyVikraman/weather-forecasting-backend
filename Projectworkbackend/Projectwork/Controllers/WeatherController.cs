using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projectwork.Authentication;
using Projectwork.Model;
using System.Linq;

namespace Projectwork.Controllers
{
  //[Authorize(Roles =UserRoles.Admin)]
  [Authorize]
  [ApiController]
  [Route("api/[Controller]")]
  public class WeatherController : Controller
  {

    private readonly ApplicationDbContext _applicationDbContext;

    public WeatherController(ApplicationDbContext applicationDbContext)
    {
      _applicationDbContext = applicationDbContext;
    }
    [HttpGet("get_info")]
    public async Task<IActionResult> GetAllWeartherInfo()
    {
      //return View();
      var weatherinfo = await _applicationDbContext.WeatherInfo.ToListAsync();
      //return Ok(weatherinfo);
      return Ok(new
      {
        StatusCode = 200,
        Details = weatherinfo
      });
    }
    //[HttpGet("get_info/city")]
    //public async Task<IActionResult> GetAllWeartherInfo(string city)
    //{
    //  var weatherinfo = _applicationDbContext.WeatherInfo.FindAsync(city);
    //  if (weatherinfo == null)
    //  {
    //    return NotFound(new
    //    {
    //      StatusCode = 404,
    //      Message = "Information not found"
    //    });
    //  }
    //  else
    //  {
    //    return Ok(new
    //    {
    //      StatusCode = 200,
    //      Details = weatherinfo
    //    });
    //  }
    //}



    [HttpGet("get_info/city")]
    public async Task<IActionResult> GetWeatherByCity(string city)
    {
      var weather = await _applicationDbContext.WeatherInfo.FirstOrDefaultAsync(x => x.City == city);
      if (weather == null)
      {
        return NotFound();
      }
      return Ok(weather);
    }




    [HttpPost("Add_Weatherinfo")]
    public IActionResult DeleteWeather([FromBody] WeatherInfo weatherInfoobj)
    {
      if (weatherInfoobj == null)
      {
        return BadRequest();
      }
      else
      {
        _applicationDbContext.WeatherInfo.Add(weatherInfoobj);
        _applicationDbContext.SaveChanges();
        return Ok(new
        {
          StatusCode = 200,
          Message = "Information Added Successfully"
        });
      }
    }
    [HttpPut("Update info")]
    public IActionResult UpdateWeather([FromBody] WeatherInfo weatherInfoobj)
    {
      if (weatherInfoobj == null)
      {
        return BadRequest();
      }
      else
      {
        var user = _applicationDbContext.WeatherInfo.AsNoTracking().FirstOrDefault(x => x.City == weatherInfoobj.City);
        if (user == null)
        {
          return NotFound(new
          {
            StatusCode = 404,
            Message = "Information not found"
          });
        }
        else
        {
          _applicationDbContext.Entry(weatherInfoobj).State = EntityState.Modified;
          _applicationDbContext.SaveChanges();
          return Ok(new
          {
            StatusCode = 200,
            Message = "Information Updated Successfully"
          });
        }

      }
    }
    //[HttpDelete("delete info/{city}")]
    //public IActionResult DeleteWeather(string city, [FromBody] WeatherInfo weatherInfoobj)
    //{

    //    var user = _applicationDbContext.WeatherInfo.AsNoTracking().FirstOrDefault(x => x.City == weatherInfoobj.City);
    //    if (user == null)
    //    {
    //        return NotFound(new
    //        {
    //            StatusCode = 404,
    //            Message = "Information not found"
    //        });
    //    }
    //    else
    //    {
    //        _applicationDbContext.Remove(user);
    //        _applicationDbContext.SaveChanges();
    //        return Ok(new
    //        {
    //            StatusCode = 200,
    //            Message = "Information deleted Successfully"
    //        });
    //    }
    //}
    [HttpDelete]
    [Route("{id:int}")]
    public async Task<IActionResult> DeleteWeather([FromRoute] int id)
    {

      var contact = await _applicationDbContext.WeatherInfo.FindAsync(id);
      if (contact != null)

      {
        _applicationDbContext.Remove(contact);
        await _applicationDbContext.SaveChangesAsync();
        return Ok(contact);

      }
      return NotFound();
    }
  }
}
