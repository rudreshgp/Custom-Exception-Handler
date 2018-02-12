namespace CustomExceptionHandler.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using LoggerContracts;
    using ResourceFiles;

    /// <summary>
    /// This class is used as a helper method. currently we are using it to return list of loggers and logs the exception --- We can move it to seperate files for following SOLID principals
    /// </summary>
    public static class GenericHelper
    {
        /// <summary>
        /// Get the logger instances in the application based on the config file
        /// </summary>
        /// <returns>list of all logger implementations</returns>
        public static List<ILogger> GetLoggers()
        {
            var types = GetInstanceTypesForType(typeof(ILogger));
            if (types?.Any() != true)
            {
                throw new Exception(GlobalResource.NoLoggerDefined);
            }
            var definedLoggers = GetDefinedLoggers();
            var loggerInstances = new List<ILogger>();
            definedLoggers.ForEach(x =>
            {
                var loggerInstance = GetInstanceForType(x, types);
                if (loggerInstance != null)
                {
                    loggerInstances.Add((ILogger)loggerInstance);
                }
            });
            return loggerInstances;
        }

        /// <summary>
        /// Handling of exception
        /// </summary>
        /// <param name="ex">othrown exception</param>
        public static void HandleException(this Exception ex)
        {
            if (ex is CustomException)
            {
                return; //Don't log it's a custom exception thrown by the user intensionally
            }
            ExceptionHandler.HandleException(ex);
        }

        /// <summary>
        /// gets list of all logger implementations
        /// </summary>
        /// <param name="interfaceType">interface being implemented by the loggers</param>
        /// <returns>list of all logger implementations</returns>
        private static List<Type> GetInstanceTypesForType(Type interfaceType)
        {
            var implementedTypes =
               AppDomain.CurrentDomain.GetAssemblies()
                   .SelectMany(x => x.GetTypes())
                   .Where(interfaceType.IsAssignableFrom).ToList();

            implementedTypes.Remove(interfaceType);
            return implementedTypes;
        }

        /// <summary>
        /// Gets all the loggers defined in the configuration file
        /// </summary>
        /// <returns>names of the defined loggers</returns>
        private static List<string> GetDefinedLoggers()
        {
            var definedLoggers = ConfigurationManager.AppSettings.Get(Constants.EnabledLoggers);
            if (string.IsNullOrWhiteSpace(definedLoggers))
            {
                definedLoggers = Constants.DefaultLoggers;
            }
            return definedLoggers.Split(';').Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
        }

        /// <summary>
        /// Creates the instance for the specified type
        /// </summary>
        /// <param name="name">starting text of the class for eg. File for FileLogger</param>
        /// <param name="types">list of logger implementation types </param>
        /// <returns>instance of the ILogger implementation matching the name</returns>
        private static object GetInstanceForType(string name, List<Type> types)
        {
            var instanceType = types.FirstOrDefault(x => x.Name.StartsWith(name, StringComparison.OrdinalIgnoreCase));
            if (instanceType == null) return null;
            return Activator.CreateInstance(instanceType);
        }

    }
}
