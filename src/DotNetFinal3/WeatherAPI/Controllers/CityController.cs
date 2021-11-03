using Domains.Models;
using Microsoft.AspNetCore.Mvc;
using Services.BO;
using Services.Commands;
using Services.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CityController : ControllerBase
    {
        private readonly CityCommand _cityCommand;
        private readonly CityQuery _cityQuery;

        public CityController(CityCommand cityCommand, CityQuery cityQuery)
        {
            _cityCommand = cityCommand;
            _cityQuery = cityQuery;
        }

        /// <summary>
        /// 获取全国城市存入数据库
        /// </summary>
        [HttpGet("GetCities")]
        public void GetCities()
        {
            var list = _cityQuery.GetCityList();
            _cityCommand.GetCityFullNameAndFillCityTable(list);
        }

    }
}
