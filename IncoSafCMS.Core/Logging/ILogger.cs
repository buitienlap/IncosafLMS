using System;

namespace IncosafCMS.Core.Logging
{
    public interface ILogger
    {
        void Log(string message);
        void Log(Exception ex);
    }
}
