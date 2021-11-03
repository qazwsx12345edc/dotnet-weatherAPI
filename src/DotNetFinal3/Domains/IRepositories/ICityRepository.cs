using Domains.Models;
using System.Collections.Generic;

namespace Domains.Repositories
{
    public interface ICityRepository
    {
        /// <summary>
        /// AddCity
        /// </summary>
        /// <param name="city"></param>
        public void AddCity(City city);


        /// <summary>
        /// cityName查找List<City>
        /// </summary>
        public List<City> GetCityList(string cityName);


        /// <summary>
        /// 根据省市区全名查找City
        /// </summary>
        /// <param name="fullName"></param>
        /// <returns></returns>
        public City GetCityByFullName(string fullName);


        /// <summary>
        /// 保存
        /// </summary>
        public void Save();
    }
}