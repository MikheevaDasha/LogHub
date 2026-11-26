namespace LogHub.Sinks;

public class TelegramSink : ILogSink
{
    public void Write(LogRecord record, string text) => System.Console.WriteLine($"[Telegram] {text}");
    public void Dispose() { }
}