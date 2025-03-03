namespace GloboticketWeb.Utils;

public static class CsvUtils
{

    public static string SanitizeCsvValue(string csvValue)
    {
        // Quick return for empty strings
        if (string.IsNullOrEmpty(csvValue))
            return string.Empty;

        // Check if sanitization is needed at all
        if (!csvValue.Contains(',') && !char.IsWhiteSpace(csvValue[0]) && !char.IsWhiteSpace(csvValue[csvValue.Length - 1]))
            return csvValue;

        var parts = csvValue.Split(',');
        List<string> values = new List<string>(parts.Length); // Pre-allocate capacity
        foreach(var part in parts)
        {
            if (!string.IsNullOrWhiteSpace(part))
            {
                values.Add(part.Trim());
            }
        }
        return string.Join(",", values);
    }

    /* ORIGINAL IMPLEMENTATION
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
    }*/
}