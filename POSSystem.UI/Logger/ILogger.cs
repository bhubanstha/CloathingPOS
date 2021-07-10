using log4net;
using System;

namespace POSSystem.UI
{
    public interface ILogger
    {
        ILog GetLogger(Type type);
    }
}