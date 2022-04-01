using System;
using Ninject;
using WADemo.Core.Interfaces;
using WADemo.DAL;
using WADemo.BLL;
using WADemo.Core;
using WeatherAlmanac.Core;
using WeatherAlmanac.Core.DTO;
using WADemo.UI;

namespace WADemo.App
{
    public static class NinjectContainer
    {
        public static StandardKernel Kernel { get; private set; }
        
        public static void Configure(ApplicationMode appMode, LoggingMode logMode, string filename, string logfile)
        {
            Kernel = new StandardKernel();
            
            if (logMode == LoggingMode.Console)
            {
                Kernel.Bind<ILogger>().To<ConsoleLogger>();
            }
            else if(logMode == LoggingMode.File)
            {
              Kernel.Bind<ILogger>().To<FileLogger>().WithConstructorArgument(logfile);  
            }
            else
            {
                Kernel.Bind<ILogger>().To<NullLogger>();            
            }
            if (appMode == ApplicationMode.Test)
            {
                Kernel.Bind<IRecordRepository>().To<MockRecordRepository>();
            }
            else
            {
                Kernel.Bind<IRecordRepository>().To<CsvRecordRepository>().WithConstructorArgument("fileName", filename)
                  .WithConstructorArgument("logger", Kernel.Get<ILogger>());
            }
            Kernel.Bind<IRecordService>().To<RecordService>().WithConstructorArgument(Kernel.Get<IRecordRepository>());
            Kernel.Bind<Controller>().To<Controller>().WithConstructorArgument(Kernel.Get<IRecordService>());
        }
    }
}
