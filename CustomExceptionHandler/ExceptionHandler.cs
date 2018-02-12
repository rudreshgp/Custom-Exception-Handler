namespace CustomExceptionHandler
{
    using System;
    using System.Collections.Generic;
    using Helpers;
    using LoggerContracts;

    /// <summary>
    /// This class handles the logging of exceptions occurring in the application
    /// except the business exceptions
    /// </summary>
    public class ExceptionHandler
    {

        /// <summary>
        /// self Reference to exception handler instance. Because at a time only one need to write to exception file.
        /// </summary>
        private static ExceptionHandler _handler;

        /// <summary>
        /// holds reference to logger object instance. If we want othere loggers like Database, mail then they need to inherit from Logger and we need to seperate File logger from that
        /// </summary>
        private static List<ILogger> _loggers;

        /// <summary>
        /// instance of exception handler
        /// </summary>
        protected static ExceptionHandler Handler => _handler ?? (_handler = new ExceptionHandler());

        /// <summary>
        /// instance of logger -- Normally we need to inject them dynamically
        /// </summary>
        protected static List<ILogger> Loggers =>
            _loggers ?? (_loggers = GenericHelper.GetLoggers());
        private ExceptionHandler()
        {

        }

        /// <summary>
        /// Handles the exception
        /// </summary>
        /// <param name="ex">exception occured </param>
        private void ProcessException(Exception ex)
        {
            while (true)
            {
                Loggers.ForEach(
                     x =>
                             x.Log(ex)
                 );
                if (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                    continue;
                }
                break;
            }
        }

        /// <summary>
        /// static method to invoke from outside
        /// </summary>
        /// <param name="ex">the occured exception</param>
        public static void HandleException(Exception ex)
        {
            Handler.ProcessException(ex);

        }
    }
}
