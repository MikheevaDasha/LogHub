namespace LogHub;
public class LogHubConfig
{
    public string SinkType { get; set; } = "Console";
    public string Format { get; set; } = "Plain";
    public Severity MinSeverity { get; set; } = Severity.Info;
    public string FilePath { get; set; } = "loghub.out.log";
    public long MaxFileBytes { get; set; } = 1024 * 1024;
    public int MaxBackups { get; set; } = 3;

    public static LogHubConfig Parse(string[] lines)
    {
        var config = new LogHubConfig();
        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#")) continue;
            var parts = line.Split('=', 2);
            if (parts.Length != 2) continue;
            var key = parts[0].Trim().ToLower();
            var val = parts[1].Trim();
            switch (key)
            {
                case "sink": config.SinkType = val; break;
                case "format": config.Format = val; break;
                case "minseverity": config.MinSeverity = Enum.Parse<Severity>(val, true); break;
                case "filepath": config.FilePath = val; break;
                case "maxfilebytes": config.MaxFileBytes = long.Parse(val); break;
                case "maxbackups": config.MaxBackups = int.Parse(val); break;
            }
        }
        return config;
    }
}