using Domains.Models;
using Infrastructure.Repositories;
using Services.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Commands
{
    public class WeatherCommand
    {
        private readonly WeatherRepository _weatherRepository;
        public WeatherCommand(WeatherRepository weatherRepository)
        {
            _weatherRepository = weatherRepository;
        }

        /// <summary>
        /// 存储天气记录
        /// </summary>
        /// <param name="bo"></param>
        public void SaveWeather(AddWeatherBO bo)
        {
            var weather = Weather.SetWeather(bo.CityId, bo.WeatherKind, bo.Temp, bo.TempHigh, bo.TempLow, bo.Preesure, bo.WindSpeed, bo.WindDirect, bo.WindPower);
            _weatherRepository.AddWeather(weather);
            _weatherRepository.Save();
        }
    }
}
