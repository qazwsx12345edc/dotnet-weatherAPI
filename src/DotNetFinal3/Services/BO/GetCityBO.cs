using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.BO
{
    public class GetCityBO
    {
        public ulong cityid { set; get; }
        public ulong parentid { set; get; }
        public string citycode { set; get; }
        public string city { set; get; }
    }
}
