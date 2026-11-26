namespace LogHub;
public enum Severity { Debug, Info, Warning, Error, Critical }
public record LogRecord(DateTime Timestamp, Severity Level, string Message);