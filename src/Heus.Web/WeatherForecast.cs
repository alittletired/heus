using System;
using System.ComponentModel.DataAnnotations;

namespace Heus.Web
{
    public enum EntityState
    {
        [Display(Name = "没有设置")] NotSet,
        [Display(Name = "禁用")] Disabled,
        [Display(Name = "启用")] Enable
    }

    public class WeatherForecast
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int) (TemperatureC / 0.5556);
        public EntityState EntityState { get; set; }
        public string Summary { get; set; }
    }
}