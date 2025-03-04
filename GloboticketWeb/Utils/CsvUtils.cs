namespace GloboticketWeb.Utils;

public static class CsvUtils
{
    public static string SanitizeCsvValue(string csvValue)
    {
        if (string.IsNullOrEmpty(csvValue))
        {
            return string.Empty;
        }
    
        // Quick check if sanitization is likely needed
        if (!csvValue.Contains(',') && !char.IsWhiteSpace(csvValue[0]) && !char.IsWhiteSpace(csvValue[^1]))
        {
            return csvValue;
        }
    
        var parts = csvValue.Split(',');
        bool needsSanitization = false;
        
        // Check if any part needs trimming or removal
        for (int i = 0; i < parts.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(parts[i]) || parts[i] != parts[i].Trim())
            {
                needsSanitization = true;
                break;
            }
        }
        
        if (!needsSanitization)
        {
            return csvValue;
        }
        
        // Only allocate the list and perform operations if needed
        List<string> values = new List<string>();
        foreach (var part in parts)
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