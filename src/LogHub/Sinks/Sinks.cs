using LogHub.Formatting;

namespace LogHub.Sinks;

public interface ILogSink : IDisposable { void Write(LogRecord record, string text); }
public interface IConsoleOutput { void WriteLine(string text); }

public class ConsoleSink(IConsoleOutput output) : ILogSink
{
    public void Write(LogRecord record, string text) => output.WriteLine(text);
    public void Dispose() { }
}

// Файловый приёмник с ротацией (Вариант 1)
public class RotatingFileSink(string filePath, long maxBytes, int maxBackups) : ILogSink
{
    public void Write(LogRecord record, string text)
    {
        var line = text + Environment.NewLine;
        var info = new FileInfo(filePath);
        if (info.Exists && info.Length > 0 && info.Length + System.Text.Encoding.UTF8.GetByteCount(line) > maxBytes)
        {
            Rotate();
        }
        File.AppendAllText(filePath, line);
    }

    private void Rotate()
    {
        var oldest = $"{filePath}.{maxBackups}";
        if (File.Exists(oldest)) File.Delete(oldest);
        
        for (var i = maxBackups - 1; i >= 1; i--)
        {
            var source = $"{filePath}.{i}";
            var dest = $"{filePath}.{i + 1}";
            if (File.Exists(source)) File.Move(source, dest);
        }
        File.Move(filePath, $"{filePath}.1");
    }
    public void Dispose() { }
}