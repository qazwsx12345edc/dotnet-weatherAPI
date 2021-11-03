using Domains.IRepositories;
using Domains.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class WeatherRepository : IWeatherRepository
    {
        private readonly weatherforecastdbContext _context;

        public WeatherRepository(weatherforecastdbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 添加天气记录
        /// </summary>
        public void AddWeather(Weather weather)
        {
            _context.Weathers.Add(weather);
        }

        /// <summary>
        /// 查找某地所有历史天气状况
        /// </summary>
        public List<Weather> GetWeatherList(string cityId)
        {
            List<Weather> list = _context.Weathers
                .Where(row => row.CityId.ToString().Equals(cityId)).ToList();
            return list;
        }

        /// <summary>
        /// 获取某天某城市的所有被记录的天气状况
        /// </summary>
        /// <param name="timeStr"></param>
        /// <returns></returns>
        public List<Weather> GetOneDayWeatherList(string cityId, string timeStr)
        {
            List<Weather> list = GetWeatherList(cityId);
            if (list == null || list.Count == 0) return null;
            List<Weather> weathers = list
                    .Where(row => row.WeatherDatetime.ToString().Contains(timeStr)).ToList();
            return weathers;
        }

        /// <summary>
        /// 保存
        /// </summary>
        public void Save()
        {
             _context.SaveChanges();
        }

       
    }
}
