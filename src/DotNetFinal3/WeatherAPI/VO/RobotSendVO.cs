using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherAPI.VO
{
    public class RobotSendVO
    {
        public RobotSendVO(Text text)
        {
            this.text = text;
        }

        public Text text { set; get; }

        public class Text
        {
            public string content { set; get; }
        }

        
    }

}
