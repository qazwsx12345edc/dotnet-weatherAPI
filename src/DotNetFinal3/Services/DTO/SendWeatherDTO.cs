using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO
{
    public class SendWeatherDTO
    {
        public string WeatherKind { get; set; }
        public uint Temp { get; set; }
        public int TempHigh { get; set; }
        public int TempLow { get; set; }
        public uint Preesure { get; set; }
        public decimal WindSpeed { get; set; }
        public string WindDirect { get; set; }
        public string WindPower { get; set; }
    }
}
