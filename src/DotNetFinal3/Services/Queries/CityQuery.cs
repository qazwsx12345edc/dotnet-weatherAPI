using Domains.Models;
using Infrastructure.Repositories;
using Infrastructure.Utils;
using Nancy.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Services.BO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Services.Queries
{
    public class CityQuery : BaseUtils
    {

        private readonly CityRepository _cityRepository;
        public CityQuery(CityRepository cityRepository)
        {
            _cityRepository = cityRepository;
        }

        /// <summary>
        /// 调用百度api，获取城市list
        /// </summary>
        /// <returns> List<GetCityBO> </returns>
        public List<GetCityBO> GetCityList()
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://jisuweather.api.bdymkt.com/weather/city");
            SetHeaderValue(req.Headers, "X-Bce-Signature", "AppCode/" + "4853cfb038464e14831220b3195f136f");
            SetHeaderValue(req.Headers, "Content-Type", "application/json;charset=UTF-8");

            HttpWebResponse res = (HttpWebResponse)req.GetResponse();
            var resBody = res.GetResponseStream();
            string resString = new StreamReader(resBody).ReadToEnd();

            JObject jo = (JObject)JsonConvert.DeserializeObject(resString);
            string resultJSON = jo["result"].ToString();
            JavaScriptSerializer Serializer = new JavaScriptSerializer();
            List<GetCityBO> cities = Serializer.Deserialize<List<GetCityBO>>(resultJSON);
            return cities;
        }

        /// <summary>
        /// 根据城市名获取城市id
        /// </summary>
        /// <param name="cityName"></param>
        /// <returns></returns>
        public List<City> GetCityId(string cityName)
        {
            List<City> list = _cityRepository.GetCityList(cityName);
            if (list.Count.Equals(0))
            {
                return null;
            }
            else
            {
                return list;
            }
        }

        /// <summary>
        /// 根据全名获取城市
        /// </summary>
        /// <param name="fullName"></param>
        /// <returns></returns>
        public City GetCityByFullName(string fullName)
        {
            return _cityRepository.GetCityByFullName(fullName);
        }


        /// <summary>
        /// 获取被监控的城市列表
        /// </summary>
        /// <returns></returns>
        public List<City> GetMonitoredCities()
        {
            return _cityRepository.GetMonitoredCityList();
        } 
       
    }
}
