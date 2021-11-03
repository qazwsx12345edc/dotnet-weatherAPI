using Domains.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.IRepositories
{
    public interface IWeatherRepository
    {
        /// <summary>
        /// AddWeather
        /// </summary>
        /// <param name="weather"></param>
        public void AddWeather(Weather weather);

        /// <summary>
        /// 查找某地所有历史天气状况
        /// </summary>
        public List<Weather> GetWeatherList(string cityId);

        /// <summary>
        /// 保存
        /// </summary>
        public void Save();
    }
}
