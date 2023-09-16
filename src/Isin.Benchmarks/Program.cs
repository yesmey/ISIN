using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Yesmey;

BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);

[MemoryDiagnoser]
public class Perf_Isin
{
    private Isin _isin;

    [GlobalSetup]
    public void SetupIndexOf()
    {
        _isin = Isin.Parse("AU0000XVGZA3");
    }

    [Benchmark]
    public Isin Isin_Parse() =>
        Isin.Parse("AU0000XVGZA3");

    [Benchmark]
    public Isin Isin_TryParse()
    {
        _ = Isin.TryParse("AU0000XVGZA3", out var result);
        return result;
    }

    [Benchmark]
    public string Isin_ToString() =>
        _isin.ToString();

    [Benchmark]
    public string Isin_BasicCode() =>
        _isin.BasicCode;

    [Benchmark]
    public string Isin_CountryCode() =>
        _isin.CountryCode;
}
