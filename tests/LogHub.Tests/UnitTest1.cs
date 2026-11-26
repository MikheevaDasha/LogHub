using LogHub;
using LogHub.Formatting;
using LogHub.Parsing;
using LogHub.Processing;
using LogHub.Sinks;

namespace LogHub.Tests;

public class LogHubTests
{
    [Fact]
    public void Parse_ValidLine_ReturnsTrue()
    {
        var parser = new PlainTextLogParser();
        var result = parser.TryParse("[2026-11-26 10:00:00] INFO Привет мир", out var record);
        Assert.True(result);
        Assert.Equal(Severity.Info, record.Level);
        Assert.Equal("Привет мир", record.Message);
    }

    [Fact]
    public void Parse_InvalidLine_ReturnsFalse()
    {
        var parser = new PlainTextLogParser();
        Assert.False(parser.TryParse("Некорректная строка", out _));
    }

    [Fact]
    public void Process_FiltersAndWrites()
    {
        var parser = new PlainTextLogParser();
        var formatter = new PlainFormatter();
        var sink = new FakeSink();
        var processor = new LogProcessor(parser, formatter, sink, Severity.Warning);

        var lines = new[]
        {
            "[2026-11-26 10:00:00] INFO Сообщение 1", // пропущено
            "[2026-11-26 10:00:00] WARNING Сообщение 2", // записано
            "битая строка" // пропущено
        };

        var result = processor.Process(lines);
        Assert.Equal(1, result.Written);
        Assert.Equal(2, result.Skipped);
        Assert.Single(sink.WrittenLines);
    }

    private class FakeSink : ILogSink
    {
        public List<string> WrittenLines { get; } = new();
        public void Write(LogRecord record, string text) => WrittenLines.Add(text);
        public void Dispose() { }
    }
}