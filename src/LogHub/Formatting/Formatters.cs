namespace LogHub.Formatting;
public interface ILogFormatter { string Format(LogRecord record); }
public class PlainFormatter : ILogFormatter 
{ 
    public string Format(LogRecord r) => $"[{r.Timestamp:yyyy-MM-dd HH:mm:ss}] {r.Level} {r.Message}"; 
}
public class MessageOnlyFormatter : ILogFormatter 
{ 
    public string Format(LogRecord r) => r.Message; 
}