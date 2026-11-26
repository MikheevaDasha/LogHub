using System.Globalization;
using System.Text.RegularExpressions;

namespace LogHub.Parsing;

public interface ILogParser { bool TryParse(string line, out LogRecord record); }

public partial class PlainTextLogParser : ILogParser
{
    [GeneratedRegex(@"^\[(?<ts>[^\]]+)\]\s+(?<level>[A-Za-z]+)\s+(?<msg>\S.*)$")]
    private static partial Regex LinePattern();

    public bool TryParse(string line, out LogRecord record)
    {
        record = null!;
        var match = LinePattern().Match(line.Trim());
        if (!match.Success) return false;
        
        if (!DateTime.TryParseExact(match.Groups["ts"].Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var timestamp)) return false;
        if (!Enum.TryParse<Severity>(match.Groups["level"].Value, true, out var severity)) return false;
        
        record = new LogRecord(timestamp, severity, match.Groups["msg"].Value);
        return true;
    }
}