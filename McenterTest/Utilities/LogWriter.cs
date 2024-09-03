using System.IO;

namespace McenterTest.Utilities
{
    public class LogWriter
    {
        // The logwriter will be a singleton for now
        // This can be changed if we want to create multiple loggers for
        // in the future
        private static LogWriter logWriterInstance;
        private static readonly object _lock = new object();
        private readonly string logFilePath;

        // Log levels
        public enum LogLevel { Debug, Info, Error }


        // Singleton instance access
        public static LogWriter Instance
        {
            get
            {
                if (logWriterInstance == null)
                {
                    lock (_lock)
                    {
                        if (logWriterInstance == null)
                        {
                            logWriterInstance = new LogWriter();
                        }
                    }
                }
                return logWriterInstance;
            }
        }


        private LogWriter() 
        {
            // Determine the log file path based on environment
            string logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
            Directory.CreateDirectory(logDirectory);

            // Create a unique log file name with a timestamp
            string logFileName = $"logfile_{DateTime.Now:yyyy-MM-dd_HHmm}.txt";
            logFilePath = Path.Combine(logDirectory, logFileName);
        }

        public void LogWithTimestamp(string message, LogLevel level)
        {
            string formattedMessage;

            if (level == LogLevel.Info) { formattedMessage = $"[{DateTime.Now}]  |  [{level}]   |  {message}";  }
            else { formattedMessage = $"[{DateTime.Now}]  |  [{level}]  |  {message}"; }
            File.AppendAllText(logFilePath, formattedMessage + Environment.NewLine);
        }

    }
}
