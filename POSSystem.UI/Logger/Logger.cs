using log4net;
using log4net.Config;
using System;
using System.IO;
using System.Reflection;

namespace POSSystem.UI
{
    public class Logger : ILogger
    {

        public Logger()
        {
            SetLog4NetConfiguration();
        }

        public ILog GetLogger(Type type)
        {
            return LogManager.GetLogger(type);
        }

        private  void SetLog4NetConfiguration()
        {
            var logRepo = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepo, new FileInfo("log4Net.config"));
        }
    }
}
