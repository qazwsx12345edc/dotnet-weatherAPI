using Domains.Models;
using Domains.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class CityRepository : ICityRepository
    {

        private readonly weatherforecastdbContext _context;

        public CityRepository(weatherforecastdbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 添加城市
        /// </summary>
        public void AddCity(City city)
        {
            _context.Cities.Add(city);
        }

        /// <summary>
        /// cityName查找List<City>
        /// </summary>
        public List<City> GetCityList(string cityName)
        {
            List<City> list = _context.Cities
                .Where(row => row.CityName.Contains(cityName)).ToList();
            return list;
        }

        /// <summary>
        /// 根据省市区全名fullName查找City
        /// </summary>
        /// <param name="fullName"></param>
        /// <returns></returns>
        public City GetCityByFullName(string fullName)
        {
            List<City> city = _context.Cities
                .Where(row => row.FullName.Equals(fullName)).ToList();
            if (city.Count.Equals(1))
            {
                return city[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 返回监控的城市列表，用于每天自动查询天气，保存历史天气
        /// </summary>
        /// <param name="cityName"></param>
        /// <returns></returns>
        public List<City> GetMonitoredCityList()
        {
            List<City> list = _context.Cities
                .Where(row => row.Monitor == 1).ToList();
            return list;
        }

        /// <summary>
        /// 保存改变
        /// </summary>
        public void Save()
        {
            _context.SaveChanges();
        }



    }
}
