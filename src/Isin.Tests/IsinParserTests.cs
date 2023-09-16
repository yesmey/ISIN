namespace Yesmey.Tests;

public class IsinParserTests
{
    [Theory]
    [InlineData("AU0000XVGZA3", 3)]
    [InlineData("ES0SI0000005", 5)]
    public void Valid_CheckDigit(string text, int checkDigit)
    {
        var valid = Isin.TryParse(text, out var isin);
        Assert.True(valid);
        Assert.Equal(checkDigit, isin.CheckDigit);
    }

    [Theory]
    [InlineData("AU0000XVGZA5", 5)]
    [InlineData("ES0SI0000009", 9)]
    public void Invalid_CheckDigit(string text, int checkDigit)
    {
        var valid = Isin.TryParse(text, out var isin);
        Assert.False(valid);
        Assert.NotEqual(checkDigit, isin.CheckDigit);
    }

    [Theory]
    [InlineData("AU0000XVGZA3", "AU")]
    [InlineData("ES0SI0000005", "ES")]
    public void Prefix(string value, string countryCode)
    {
        var valid = Isin.TryParse(value, out var isin);
        Assert.True(valid);
        Assert.Equal(countryCode, isin.CountryCode);
    }

    [Theory]
    [InlineData("AU0000XVGZA3", "0000XVGZA")]
    [InlineData("ES0SI0000005", "0SI000000")]
    public void BasicCode(string value, string basicCode)
    {
        var valid = Isin.TryParse(value, out var isin);
        Assert.True(valid);
        Assert.Equal(basicCode, isin.BasicCode);
    }
}
