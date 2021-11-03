using Domains.IRepositories;
using Infrastructure.Repositories;
using Infrastructure.Utils;
using Moq;
using Services.Commands;
using Services.Queries;
using WeatherAPI.VO;
using Xunit;

namespace WeatherAPI.Controllers.Tests
{
    public class WeatherControllerTests
    {
        [Theory]
        [InlineData("杭州")]

        public void GetWeatherTest(string cityName)
        {
            WeatherQuery weatherQuery = new Mock<WeatherQuery>().Object;
            CityQuery cityQuery = new Mock<CityQuery>().Object;
            BaseUtils baseUtils = new Mock<BaseUtils>().Object;
            WeatherCommand weatherCommand = new Mock<WeatherCommand>().Object;

            WeatherController weatherController = new WeatherController(weatherQuery, baseUtils, cityQuery, weatherCommand);

            RobotSendVO.Text text = new RobotSendVO.Text
            {
                content = cityName
            };
            RobotSendVO vo = new RobotSendVO(text);
            int result = weatherController.GetWeather(vo);
            Assert.Equal(1, result);
        }
    }
}