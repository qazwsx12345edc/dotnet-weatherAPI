using Domains.Models;
using Infrastructure.Repositories;
using Infrastructure.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Services.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Services.Queries
{
    public class WeatherQuery : BaseUtils
    {
        private readonly WeatherRepository _weatherRepository;
        public WeatherQuery(WeatherRepository weatherRepository)
        {
            _weatherRepository = weatherRepository;
        }

        /// <summary>
        /// 调用api查询天气
        /// </summary>
        /// <param name="city"></param>
        /// <returns></returns>
        public SendWeatherDTO GetWeatherByBaiduAPI(string cityid)
        {
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://jisuweather.api.bdymkt.com/weather/query?cityid=" + cityid);
                SetHeaderValue(req.Headers, "X-Bce-Signature", "AppCode/" + "4853cfb038464e14831220b3195f136f");
                SetHeaderValue(req.Headers, "Content-Type", "application/json;charset=UTF-8");
                req.Method = "POST";
                HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                var resBody = res.GetResponseStream();
                string resString = new StreamReader(resBody).ReadToEnd();
                JObject jo = (JObject)JsonConvert.DeserializeObject(resString);
                string resultJSON = jo["result"].ToString();
                SendWeatherDTO dto = new()
                {
                    WeatherKind = jo["result"]["weather"].ToString(),
                    Temp = (uint)Convert.ToInt32(jo["result"]["temp"].ToString()),
                    TempHigh = Convert.ToInt32(jo["result"]["temphigh"].ToString()),
                    TempLow = Convert.ToInt32(jo["result"]["templow"].ToString()),
                    Preesure = (uint)Convert.ToInt32(jo["result"]["pressure"].ToString()),
                    WindSpeed = Convert.ToDecimal(jo["result"]["windspeed"].ToString()),
                    WindDirect = jo["result"]["winddirect"].ToString(),
                    WindPower = jo["result"]["windpower"].ToString()
                };
                return dto;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 获取某地某天历史天气列表
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public List<Weather> GetHistoryWeather(string cityId, string timeStr)
        {
            return _weatherRepository.GetOneDayWeatherList(cityId, timeStr);
        }

        public List<Weather> test(string cityId)
        {
            return _weatherRepository.GetWeatherList(cityId);
        }
    }
}
