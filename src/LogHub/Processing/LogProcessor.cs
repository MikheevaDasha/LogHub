using LogHub.Formatting;
using LogHub.Parsing;
using LogHub.Sinks;

namespace LogHub.Processing;
public record ProcessingResult(int Written, int Skipped);

public class LogProcessor(ILogParser parser, ILogFormatter formatter, ILogSink sink, Severity minSeverity)
{
    public ProcessingResult Process(IEnumerable<string> lines)
    {
        var written = 0;
        var skipped = 0;
        foreach (var line in lines)
        {
            if (!parser.TryParse(line, out var record) || record.Level < minSeverity)
            {
                skipped++;
                continue;
            }
            sink.Write(record, formatter.Format(record));
            written++;
        }
        return new ProcessingResult(written, skipped);
    }
}