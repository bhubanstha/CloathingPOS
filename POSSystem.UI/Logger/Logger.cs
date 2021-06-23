using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace POSSystem.UI
{
    public class Logger
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
            XmlConfigurator.Configure(logRepo, new FileInfo("log4NetConfig.config"));
        }
    }
}
