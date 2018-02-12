namespace CustomExceptionHandler.Loggers
{
    using System;
    using System.Configuration;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Helpers;
    using LoggerContracts;
    using ResourceFiles;

    /// <summary>
    /// This is a file logger you can write 
    /// </summary>
    public class FileLogger : ILogger
    {
        /// <summary>
        /// holds reference to file streamwriter
        /// </summary>
        private StreamWriter _writer;

        /// <summary>
        /// according to business logic after few days(normally 7 days) old logs will be deleted
        /// </summary>
        private static DateTime _lastDeleted;

        /// <summary>
        /// where the log file is stored configurable under the config file
        /// </summary>
        private static string _filePath;

        /// <summary>
        /// How manys days old files should be deleted
        /// </summary>
        private static int _logFileLife;


        /// <summary>
        /// Reads from config file the path where log files will be stored
        /// </summary>
        private static string FilePath => _filePath ?? (_filePath = ConfigurationManager.AppSettings[Constants.LogFileDirectoryPath] ?? Constants.DefaultLogDirectory);

        /// <summary>
        /// days to keep log
        /// </summary>
        private static int LogFileLife
        {
            get
            {
                if (_logFileLife != 0) return _logFileLife;
                var success = int.TryParse(ConfigurationManager.AppSettings[Constants.LogHistoryLife], out _logFileLife);
                if (!success)
                {
                    _logFileLife = 7;
                }
                return _logFileLife;
            }
        }

        /// <summary>
        /// logs the exception message in the file(.txt file)
        /// </summary>
        /// <param name="exception">exception occured</param>
        public void Log(Exception exception)
        {
            try
            {
                string logFilePath = FilePath + @"LogFile_" + DateTime.UtcNow.Date.ToString(Constants.LogFileNameFormat) + ".txt";
                FileInfo file = new FileInfo(logFilePath);
                if (file.Directory != null && !file.Directory.Exists)
                {
                    if (file.DirectoryName != null) Directory.CreateDirectory(file.DirectoryName);
                }
                if ((DateTime.UtcNow - _lastDeleted).TotalDays > LogFileLife)
                {
                    DeleteOldFilesIfExist(file.DirectoryName);
                }
                using (_writer = File.AppendText(logFilePath))
                {
                    _writer.WriteLine(GetExceptionDetails(exception));
                }
            }
            finally
            {
                if (_writer != null)
                {
                    _writer.Close();
                    _writer.Dispose();
                }
            }
        }

        /// <summary>
        /// Deletes old files in the log directory
        /// </summary>
        /// <param name="directoryPath">path of the directory from which log files need to be deleted</param>
        private void DeleteOldFilesIfExist(string directoryPath)
        {
            _lastDeleted = DateTime.UtcNow;
            var timeDifference = DateTime.Now.AddDays(-LogFileLife);
            var files = Directory.GetFiles(directoryPath).Where(x => x.Contains("LogFile_")).ToList();
            foreach (var file in files)
            {
                FileInfo fi = new FileInfo(file);
                if (fi.CreationTime < timeDifference)
                    fi.Delete();
            }
        }

        private string GetExceptionDetails(Exception ex)
        {
            StringBuilder logText = new StringBuilder();
            logText.AppendLine();
            logText.AppendLine($"{GlobalResource.DateTime } {DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)}");
            logText.AppendLine(GlobalResource.NewLineSeperator);
            logText.AppendLine($"{GlobalResource.Message} {ex.Message}");
            logText.AppendLine(GlobalResource.NewLineSeperator);
            logText.AppendLine($"{GlobalResource.Environament} {ex.Source}");
            logText.AppendLine(GlobalResource.NewLineSeperator);
            logText.AppendLine($"{GlobalResource.StackTrace} {ex.StackTrace}");
            logText.AppendLine(GlobalResource.NewLineSeperator);
            return logText.ToString();
        }
    }
}
