namespace ImageToPDF
{
    interface ILogger
    {
        void SetLogLevel(LogLevel level);
        void WriteLog(string log, LogLevel level);
    }
}
