using LogHub;
using LogHub.Formatting;
using LogHub.Parsing;
using LogHub.Processing;
using LogHub.Sinks;

if (args.Length == 0)
{
    Console.Error.WriteLine("Использование: LogHub <файл-лога> [файл-конфигурации]");
    return 1;
}

var logPath = args[0];
var configPath = args.Length > 1 ? args[1] : "loghub.config";
var configLines = File.Exists(configPath) ? File.ReadAllLines(configPath) : Array.Empty<string>();
var config = LogHubConfig.Parse(configLines);

ILogParser parser = new PlainTextLogParser();
ILogFormatter formatter = config.Format == "MessageOnly" ? new MessageOnlyFormatter() : new PlainFormatter();

ILogSink sink = config.SinkType switch
{
    "Console" => new ConsoleSink(new SystemConsoleOutput()),
    "File" => new RotatingFileSink(config.FilePath, config.MaxFileBytes, config.MaxBackups),
    _ => throw new Exception($"Неизвестный тип приёмника: {config.SinkType}")
};

using (sink)
{
    var processor = new LogProcessor(parser, formatter, sink, config.MinSeverity);
    var result = processor.Process(File.ReadLines(logPath));
    Console.WriteLine($"Записано: {result.Written}, пропущено: {result.Skipped}");
}
return 0;

public class SystemConsoleOutput : IConsoleOutput
{
    public void WriteLine(string text) => Console.WriteLine(text);
}