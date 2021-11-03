using System;
using System.Collections.Generic;

#nullable disable

namespace Domains.Models
{
    public partial class Weather
    {
        public ulong Id { get; set; }
        public ulong CityId { get; set; }
        public string WeatherKind { get; set; }
        public uint Temp { get; set; }
        public int TempHigh { get; set; }
        public int TempLow { get; set; }
        public uint Preesure { get; set; }
        public decimal WindSpeed { get; set; }
        public string WindDirect { get; set; }
        public string WindPower { get; set; }
        public DateTime WeatherDatetime { get; set; }
        public DateTime DbCreatedAt { get; set; }
        public DateTime DbUpdatedAt { get; set; }

        private Weather() { }

        public static Weather SetWeather(ulong cityId, string weatherKind, uint temp, int tempHigh, int tempLow, uint preesure, decimal windSpeed, string windDirect, string windPower)
        {
            Weather weather = new()
            {
                CityId = cityId,
                WeatherKind = weatherKind,
                Temp = temp,
                TempHigh = tempHigh,
                TempLow = tempLow,
                Preesure = preesure,
                WindSpeed = windSpeed,
                WindDirect = windDirect,
                WindPower = windPower,
        };
            return weather;
        }
    }
}
