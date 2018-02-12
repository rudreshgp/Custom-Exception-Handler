# CustomExceptionHandler
Custom exception handler for .net applications

The CustomException handler handles all types of exceptions occuring in the application except the User thrown exceptions.
Currently library contains <b>FileLogger</b> but other loggers can also be included along with FileLogger just add the class which implements <b>ILogger</b>.

After adding class which implements <b>ILogger</b>. Make sure you add the name of the class in the config file in the value field of EnabledLoggers key under appSettings. 

<b>\<add key="EnabledLoggers" value="File;"\/></b>

The appSettings section will be under the App.config or the Web.config of the application. 

Following are the different key value pairs used by the Logger.
<ul>
  <li><b>EnabledLoggers</b> - Used to enable kinds of Loggers. Each Logger seperated by a ;. For eg., if there are thre loggers then the value will be <b>File;Database;Xml</b>. The case of the text doesn't matter as long as there exists classes whose name starts with the metioned values and implement ILogge</li>
  <li><b>LogFileDirectoryPath</b> - The parent directory of the Log File for the <b>FileLogger</b>. By default if no value entered then Applications root directory is selected. Defaults its either bin/debug/LogFiles or bin/release/LogFiles folder.</li>
</ul>

There are other configuration settings you can add but without them FileLogger works. As it is the default logger.

Console project uses the CustomExceptionHandler's Logging feature.

