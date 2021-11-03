using Domains.Models;
using Infrastructure.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Services.BO;
using Services.Commands;
using Services.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WeatherAPI.BackgroundTask
{
    /// <summary>
    /// 定时任务 天气警报
    /// </summary>
    public class WeatherAlert : BackgroundService
    {
        private readonly WeatherQuery _weatherQuery;
        private readonly CityQuery _cityQuery;
        private readonly BaseUtils _baseUtils;
        private readonly WeatherCommand _weatherCommand;
        private readonly BaseWeatherService _baseWeatherService;
        public WeatherAlert(IServiceScopeFactory factory)
        {
            _weatherQuery = factory.CreateScope().ServiceProvider.GetRequiredService<WeatherQuery>();
            _baseUtils = factory.CreateScope().ServiceProvider.GetRequiredService<BaseUtils>(); 
            _cityQuery = factory.CreateScope().ServiceProvider.GetRequiredService<CityQuery>(); 
            _weatherCommand = factory.CreateScope().ServiceProvider.GetRequiredService<WeatherCommand>();
            _baseWeatherService = factory.CreateScope().ServiceProvider.GetRequiredService<BaseWeatherService>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // 天气警报 
                    var cityid = _cityQuery.GetCityId("滨江");
                    var currentWeather = _weatherQuery.GetWeatherByBaiduAPI(cityid[0].Id.ToString());
                    _baseUtils.RobotSend(_baseWeatherService.WeatherAlertMessage(currentWeather));

                    // 定时往数据库写入city.monitor == 1的城市（即支持查询历史天气的城市）天气
                    List<City> monitoredCityList = _cityQuery.GetMonitoredCities();
                    if (monitoredCityList != null && monitoredCityList.Count != 0)
                    {
                        foreach(City city in monitoredCityList)
                        {
                            var dto = _weatherQuery.GetWeatherByBaiduAPI(city.Id.ToString());
                            AddWeatherBO weather = new AddWeatherBO(city.Id, dto.WeatherKind, dto.Temp, dto.TempHigh, dto.TempLow, dto.Preesure, dto.WindSpeed, dto.WindDirect, dto.WindPower);
                            _weatherCommand.SaveWeather(weather);

                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                await Task.Delay(1000 * 60 * 60, stoppingToken); // 一小时执行一次
            }
        }
    }
}
