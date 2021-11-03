using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.BO
{
    public class AddWeatherBO
    {
        public AddWeatherBO(ulong cityId, string weatherKind, uint temp, int tempHigh, int tempLow, uint preesure, decimal windSpeed, string windDirect, string windPower)
        {
            CityId = cityId;
            WeatherKind = weatherKind;
            Temp = temp;
            TempHigh = tempHigh;
            TempLow = tempLow;
            Preesure = preesure;
            WindSpeed = windSpeed;
            WindDirect = windDirect;
            WindPower = windPower;
        }

        public ulong CityId { get; set; }
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
