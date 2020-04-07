using System;

namespace ImageToPDF
{
    class StdoutLogger : ILogger
    {
        private LogLevel minimumLogLevelToDisplay;

        public StdoutLogger()
        {
            this.minimumLogLevelToDisplay = LogLevel.Info;
        }

        public void SetLogLevel(LogLevel level) => this.minimumLogLevelToDisplay = level;

        public void WriteLog(string log, LogLevel level)
        {
            if (level < this.minimumLogLevelToDisplay)
            {
                return;
            }
            switch (level)
            {
                case LogLevel.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogLevel.Warn:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogLevel.Info:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case LogLevel.Debug:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                default:
                    Console.WriteLine($"Internal error: unexpected log level: {level}");
                    return;
            }

            Console.WriteLine(log);
            Console.ResetColor();
        }
    }
}
