using Domains.Models;
using Infrastructure.Utils;
using Microsoft.AspNetCore.Mvc;
using Services.BO;
using Services.Commands;
using Services.DTO;
using Services.Queries;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using WeatherAPI.VO;

namespace WeatherAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly WeatherQuery _weatherQuery;
        private readonly CityQuery _cityQuery;
        private readonly BaseUtils _baseUtils;
        private readonly WeatherCommand _weatherCommand;
        public WeatherController(WeatherQuery weatherQuery, BaseUtils baseUtils, CityQuery cityQuery, WeatherCommand weatherCommand)
        {
            _weatherQuery = weatherQuery;
            _baseUtils = baseUtils;
            _cityQuery = cityQuery;
            _weatherCommand = weatherCommand;
        }

        [HttpPost("TryGetWeather")]
        public object TryGetWeather(string id)
        {
            return _weatherQuery.GetWeatherByBaiduAPI(id);
        }



        /// <summary>
        /// 接收钉钉机器人的消息，获取天气，并调用钉钉机器人输出
        /// </summary>
        /// <param name="city"></param>
        /// <returns></returns>
        [HttpPost("getWeatherByRobot")]
        public int GetWeather(RobotSendVO vo)
        {
            var cityName = vo.text.content.Trim();
            if (cityName.Contains('/') == false) // 查询当日天气
            {
                return GetTodaysWeather(cityName);
            }
            else // 查询历史天气
            {
                return GetHistoryWeather(cityName); 
            }
        }



        /// <summary>
        /// 获取今天天气
        /// </summary>
        /// <param name="cityName"></param>
        /// <returns></returns>
        [HttpPost("GetTodaysWeather")]
        public int GetTodaysWeather(string cityName)
        {
            // 根据输入名查找所有相关的地区
            var list = _cityQuery.GetCityId(cityName);
            var msg = "";
            ulong cityid = 0;
            // 没有找到 =>  不存在 或 输入的是省市区全名
            if (list == null)
            {
                // 根据全名查找一遍
                var city = _cityQuery.GetCityByFullName(cityName);
                if (city == null)
                {
                    _baseUtils.RobotSend("没有找到" + cityName + "的天气信息...");
                    return -1;
                }
                else
                {
                    msg = ("已为你找到" + city.FullName + "的天气信息：\n");
                    cityid = city.Id;
                }

            }
            // 查找到一个
            else if (list.Count.Equals(1))
            {
                msg = ("已为你找到" + list[0].FullName + "的天气信息：\n");
                cityid = list[0].Id;
            }
            // 查找到超过十个，输入过于模糊，如“区”
            else if (list.Count >= 10)
            {
                _baseUtils.RobotSend("马什么梅？什么冬梅？哪的天气？");
                return -1;
            }
            // 查找到多个
            else
            {
                var sendString = "";
                foreach (City city in list)
                {
                    sendString += city.FullName + "\n";
                }
                // 提示输入省市区全名查找
                _baseUtils.RobotSend("找到以下地点的天气信息，\n请选择一项输入：\n" + sendString);
                return -1;
            }
            // 调用api
            SendWeatherDTO dto = _weatherQuery.GetWeatherByBaiduAPI(cityid.ToString());
            // 返回null说明输入的地区为省级
            if (dto == null)
            {
                _baseUtils.RobotSend("没有找到" + cityName + "的天气信息...请将地区精确到市级及以下...");
                return -1;
            }
            string weatherInfo = "天气：" + dto.WeatherKind + "\n当前气温：" + dto.Temp.ToString()
                + " ℃\n今日气温：" + dto.TempLow + "-" + dto.TempHigh + " ℃\n气压：" + dto.Preesure.ToString()
                + " hPa\n风速：" + dto.WindSpeed + " m/s\n风向：" + dto.WindDirect;
            msg = msg + weatherInfo;

            _baseUtils.RobotSend(msg);
            
            AddWeatherBO bo = new AddWeatherBO(cityid, dto.WeatherKind, dto.Temp, dto.TempHigh, dto.TempLow, dto.Preesure, dto.WindSpeed, dto.WindDirect, dto.WindPower);
            _weatherCommand.SaveWeather(bo);
            return 0;
        }



        /// <summary>
        /// 获取历史天气
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        [HttpPost("GetHistoryWeather")]

        public int GetHistoryWeather(string str)
        {
            // 输入中带“/”， 查询历史天气，城市日期用/隔开
            string[] arrCityAndDate = str.Split(new char[] { '/' });
            var city = arrCityAndDate[0].Trim();
            var date = arrCityAndDate[1].Trim();
            var msg = "";
            // 没有输入日期
            if (date == "")
            {
                _baseUtils.RobotSend("查询历史天气，请在/后输入日期...");
                return -1;
            }

            // 正则表达式 yyyy-mm-dd  允许个位数的月日前面不补0
            else if (Regex.IsMatch(date, @"^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-9]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-))$") == false)
            {
                _baseUtils.RobotSend("查询历史天气，请输入正确的日期格式，如：2021-01-01");
                return -1;
            }
            else // 日期格式正确
            {
                List<City> list = _cityQuery.GetCityId(city);
                // 查短名查不到
                if (list == null || list.Count == 0)
                {
                    var cityByFullName = _cityQuery.GetCityByFullName(city);
                    if (cityByFullName == null)
                    {
                        _baseUtils.RobotSend("请输入正确的地区..");
                        return -1;
                    }
                    else
                    {
                        msg = ("查询到" + cityByFullName.FullName + "在" + date + "的天气...\n");
                        List<Weather> weather = GetHistoryWeatherByCityId(cityByFullName.Id.ToString(), date);
                        if (weather == null || weather.Count == 0)
                        {
                            _baseUtils.RobotSend("很抱歉，没有查找到" + cityByFullName.FullName + "在" + date + "的历史天气...\n");
                            return -1;
                        }
                        // 查找到当日的全部历史天气，求平均值
                        else
                        {
                            var dto = weather[0];
                            uint pressure = 0;
                            decimal windSpeed = 0;
                            foreach (Weather w in weather)
                            {
                                pressure += w.Preesure;
                                windSpeed += w.WindSpeed;
                            }
                            string weatherInfo = "天气：" + dto.WeatherKind + "\n当日气温：" + dto.TempLow + "-"
                                + dto.TempHigh + " ℃\n平均气压：" + pressure/weather.Count
                            + " hPa\n平均风速：" + windSpeed/weather.Count;
                            msg = msg + weatherInfo;
                        }
                        _baseUtils.RobotSend(msg);
                        return 0;
                    }
                }
                // 查到一个
                else if (list.Count == 1)
                {
                    msg = ("查询到" + list[0].FullName + "在" + date + "的天气...\n");
                    List<Weather> weather = GetHistoryWeatherByCityId(list[0].Id.ToString(), date);
                    if (weather == null)
                    {
                        _baseUtils.RobotSend("很抱歉，没有查找到" + list[0].FullName + "在" + date + "的历史天气...\n");
                        return -1;
                    }
                    else
                    {
                        var dto = weather[0];
                        uint pressure = 0;
                        decimal windSpeed = 0;
                        foreach (Weather w in weather)
                        {
                            pressure += w.Preesure;
                            windSpeed += w.WindSpeed;
                        }
                        string weatherInfo = "天气：" + dto.WeatherKind + "\n当日气温：" + dto.TempLow + "-" 
                            + dto.TempHigh + " ℃\n平均气压：" + pressure / weather.Count
                        + " hPa\n平均风速：" + windSpeed / weather.Count;
                        msg = msg + weatherInfo;
                    }
                    _baseUtils.RobotSend(msg);
                    return 0;
                }
                // 查到多个
                else if (list.Count >= 2 && list.Count <= 10)
                {
                    foreach (City c in list)
                    {
                        msg += c.FullName + "\n";
                    }
                    _baseUtils.RobotSend("你想找的可能是以下地点的历史天气，\n请在全称后加上/并输入日期：\n" + msg);
                    return -1;
                }
                else
                {
                    _baseUtils.RobotSend("查询历史天气，请输入详细的地区..");
                    return -1;
                }
                
            }
        }



        /// <summary>
        /// 获取历史天气
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        [HttpGet("GetHistoryWeatherByCityId")]
        // 转换输入的日期格式  主要是把日期的 - 改成 /，补上个位数月日前面的0
        // 调用GetHistoryWeather()
        public List<Weather> GetHistoryWeatherByCityId(string cityId, string time)
        {
            string timeStr = "";
            // 日期格式转换
            for (int i = 0; i < time.Length; i++)
            {
                if (time[i].Equals('-')) 
                {
                    timeStr += '/';
                }
                else if(time[i].Equals('0') && time[i - 1].Equals('-'))
                {

                }
                else
                {
                    timeStr += time[i];
                }
            }
            return _weatherQuery.GetHistoryWeather(cityId, timeStr);
        }

    }
}
