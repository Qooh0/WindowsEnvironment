namespace QadiffWindowsEnvironmentManager;

class SimpleIniReaderWriter
{
    private Dictionary<string, string> _entries = new Dictionary<string, string>();

    public SimpleIniReaderWriter(string path)
    {
        foreach (var line in File.ReadAllLines(path))
        {
            if (!string.IsNullOrWhiteSpace(line) && !line.StartsWith(";") && line.Contains("="))
            {
                var parts = line.Split('=');
                if (parts.Length > 1)
                {
                    var key = parts[0].Trim();
                    var value = parts[1].Trim();
                    _entries[key] = value;
                }
            }
        }
    }

    public bool isInifile(string path)
    {
        return File.Exists(path) && File.ReadAllLines(path).Any(line => !string.IsNullOrWhiteSpace(line) && !line.StartsWith(";") && (line.Contains("=") || line.Contains("[")));
    }
    
    public string? GetValue(string key)
    {
        return _entries.TryGetValue(key, out string? value) ? value : null;
    }

    public void SetValue(string key, string value)
    {
        _entries[key] = value;
    }

    public void Save(string path)
    {
        using (var writer = new StreamWriter(path))
        {
            foreach (var entry in _entries)
            {
                writer.WriteLine($"{entry.Key}={entry.Value}");
            }
        }
    }
}