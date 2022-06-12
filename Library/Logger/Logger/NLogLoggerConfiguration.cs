namespace Logger;

public class NLogLoggerConfiguration : ILoggerConfiguration
{
    public LogLevel LogLevel { get; set; } = LogLevel.Trace;
    public bool LogToConsole { get; set; } = true;
    public bool LogToFile { get; set; } = true;
}