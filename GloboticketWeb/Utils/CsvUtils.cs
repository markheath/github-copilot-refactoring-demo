namespace GloboticketWeb.Utils;

public static class CsvUtils
{
    public static string SanitizeCsvValue(string csvValue)
    {
        var parts = csvValue.Split(',');
        List<string> values = new List<string>();
        foreach(var part in parts)
        {
            if (!string.IsNullOrWhiteSpace(part))
            {
                values.Add(part.Trim());
            }
        }
        return string.Join(",", values);
    }
}