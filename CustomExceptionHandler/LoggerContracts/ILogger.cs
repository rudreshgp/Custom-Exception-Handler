using System;

namespace CustomExceptionHandler.LoggerContracts
{
    public interface ILogger
    {
        void Log(Exception exception);
    }
}
