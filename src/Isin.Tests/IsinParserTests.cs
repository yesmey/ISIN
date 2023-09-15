namespace Isin.Tests;

public class IsinParserTests
{
    [Fact]
    public void CheckDigit()
    {
        _ = Isin.TryParse("ES0SI0000005", out var isin);
        Assert.Equal('5', isin.CheckDigit);
    }

    [Fact]
    public void Prefix()
    {
        _ = Isin.TryParse("ES0SI0000005", out var isin);
        Assert.Equal("ES", isin.CountryCode);
    }

    [Fact]
    public void Value()
    {
        _ = Isin.TryParse("ES0SI0000005", out var isin);
        Assert.Equal("ES0SI0000005", isin.Value);
    }
}
