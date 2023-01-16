using System.ComponentModel.DataAnnotations;

namespace Projectwork.Model
{
  public class WeatherInfo
  {
    [Key]
    public int Id { get; set; }
    public string City { get; set; }
    public DateTime Datetime { get; set; }
    public string Category { get; set; }
    public string TemperatureC{ get; set; }


    //public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

  }
}
