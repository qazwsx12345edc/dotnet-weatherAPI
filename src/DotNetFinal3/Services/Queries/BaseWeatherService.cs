using Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Queries
{
    public class BaseWeatherService
    {
        public string WeatherAlertMessage(SendWeatherDTO currentWeather)
        {
            string msg = "";
            if (currentWeather.WeatherKind.Contains("雨"))
            {
                msg += "外面正在下雨，出门记得带伞哦~\n";
            }
            else if (currentWeather.WindSpeed >= 6)
            {
                msg += "外面有强风，请尽量不要出门！\n";
            }
            else if (currentWeather.Temp > 35)
            {
                msg += "当前外界气温" + currentWeather.Temp + "℃，出门请做好防护！\n";
            }
            else if (currentWeather.Temp <= 0)
            {
                msg += "当前外界气温只有" + currentWeather.Temp + "℃，出门请注意保暖！\n";
            }

            if (msg != "")
            {
                msg = "天气警报：\n" + msg;
            }
            return msg;
        }

    }
}
