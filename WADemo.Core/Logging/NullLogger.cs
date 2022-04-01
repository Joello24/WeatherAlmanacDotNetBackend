using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAlmanac.Core
{
    public class NullLogger : ILogger
    {
        public void Log(string message)
        {
            return;
        }
    }
}
