using Ninject;
using WADemo.BLL;
using WADemo.Core;
using WADemo.Core.Interfaces;
using WADemo.DAL;
using WADemo.UI;
using WeatherAlmanac.Core;
using WeatherAlmanac.Core.DTO;
using WADemo.UI;

namespace WADemo.App;

public static class Startup
{
  internal static void Run()
  {
    // Update this as needed to match your project
    // ⚠️ Can't use var with const - have to specify type explicitly
    const string dataDir = "../data/";
    const string dataFile = "almanac.csv";
    const string logFile = "log.error.csv";

    View.DisplayHeader("Welcome to Weather Almanac");

    LoggingMode logMode = (LoggingMode)View.GetLoggingMode() switch
    {
      LoggingMode.None => LoggingMode.None,
      LoggingMode.Console => LoggingMode.Console,
      LoggingMode.File => LoggingMode.File,
      _ => throw new ArgumentOutOfRangeException()
    };

    ApplicationMode mode = (ApplicationMode)View.GetApplicationMode() switch
    {
      ApplicationMode.Live => ApplicationMode.Live,
      ApplicationMode.Test => ApplicationMode.Test,
      _ => throw new ArgumentOutOfRangeException()
    };
    
    NinjectContainer.Configure(mode, logMode, dataDir +dataFile, dataDir + logFile);
    var controller = NinjectContainer.Kernel.Get<Controller>();
    
    controller.Run();
  }
}
