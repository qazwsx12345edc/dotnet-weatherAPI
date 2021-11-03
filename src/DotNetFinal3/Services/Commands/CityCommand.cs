using Domains.Models;
using Infrastructure.Repositories;
using Services.BO;
using System;
using System.Collections.Generic;
using System.Net;

namespace Services.Commands
{
    public class CityCommand
    {
        private readonly CityRepository _cityRepository;

        public CityCommand(CityRepository cityRepository)
        {
            _cityRepository = cityRepository;
        }

        
        private static string fullName = ""; 

        /// <summary>
        /// 根据 parentId 获得省市区全名，存入数据库
        /// </summary>
        public void GetCityFullNameAndFillCityTable(List<GetCityBO> cities)
        {
            // 转换结构
            foreach (GetCityBO city in cities)
            {
                var thisCityFullName = GetCityFullName(city, cities);
                fullName = "";
                var newCity = City.SetCity(city.cityid, city.city, thisCityFullName);

                // 写入数据库
                _cityRepository.AddCity(newCity);
                _cityRepository.Save();
            }
        }

        /// <summary>
        /// 根据 parentId，递归获取省市区fullName
        /// </summary>
        /// <param name="city"></param>
        /// <param name="cities"></param>
        /// <returns></returns>
        public string GetCityFullName(GetCityBO city, List<GetCityBO> cities)
        {
            if (city.parentid.Equals(0))
            {
                fullName = city.city + fullName;
            }
            else
            {
                fullName = city.city + fullName;
                for (int i = 0; i < cities.Count; i++)
                {
                    if (cities[i].cityid == city.parentid)
                    {
                        GetCityFullName(cities[i], cities);
                    }
                }
            }
            return fullName;
        }
    }
}
