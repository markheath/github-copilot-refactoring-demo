using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using GloboticketWeb.Utils;

namespace GloboticketWeb.Benchmarks;


[MemoryDiagnoser]  // This will show memory allocation stats
public class CsvUtilsBenchmark
{
    private string _simpleString = "simple";
    private string _stringWithCommas = "hello, world, from, benchmark";
    private string _stringWithWhitespace = " value with spaces ";

    [Benchmark(Baseline = true)]
    public string Original_Simple()
    {
        return OriginalSanitizeCsvValue(_simpleString);
    }

    [Benchmark]
    public string Optimized_Simple()
    {
        return CsvUtils.SanitizeCsvValue(_simpleString);
    }

    [Benchmark]
    public string Original_WithCommas()
    {
        return OriginalSanitizeCsvValue(_stringWithCommas);
    }

    [Benchmark]
    public string Optimized_WithCommas()
    {
        return CsvUtils.SanitizeCsvValue(_stringWithCommas);
    }

    [Benchmark]
    public string Original_WithWhitespace()
    {
        return OriginalSanitizeCsvValue(_stringWithWhitespace);
    }

    [Benchmark]
    public string Optimized_WithWhitespace()
    {
        return CsvUtils.SanitizeCsvValue(_stringWithWhitespace);
    }

    // Original implementation for comparison
    private string OriginalSanitizeCsvValue(string csvValue)
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

public class Program
{
    public static void Main(string[] args)
    {
        var summary = BenchmarkRunner.Run<CsvUtilsBenchmark>();
    }
}