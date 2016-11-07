using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kancelaria.Globals
{
    public class LogFactory
    {
        static LogFactory()
        {
            //var confFile = ConfigurationSettings.AppSettings.Get("log4net.config");
            //var fi = new FileInfo(confFile);
            //XmlConfigurator.Configure(fi);
            XmlConfigurator.Configure();
        }

        public static ILog GetLog()
        {
            return LogManager.GetLogger("KancelariaLog");
        }

        public static ILog GetLog(string logName)
        {
            return LogManager.GetLogger(logName);
        }
    }
}