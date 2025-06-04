using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIS.Infrastructure.Logging
{
    public class Logger
    {
        private readonly ILog log4NetAdapter;
        public Logger(Type type)
        {
            this.log4NetAdapter = LogManager.GetLogger(type);
        }
        public void LogError(Exception ex, string message)
        {
            this.log4NetAdapter.Error(message);
        }
        public void LogError(string message, Exception exception)
        {
            this.log4NetAdapter.Error(message, exception);
        }
        public void LogInfo(string message)
        {
            this.log4NetAdapter.Info(message);
        }
        public void LogInfo(string message, Exception exception)
        {
            this.log4NetAdapter.Info(message, exception);
        }
    }
}

