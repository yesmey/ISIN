using System.Net;

namespace Yesmey.Tests;

public class IsinFormatterTests
{
    [Fact]
    public void Valid_TryFormatSpan()
    {
        Isin isin = Isin.Parse("US0378331005");

        // UTF16 exact length
        {
            Span<char> span = stackalloc char[Isin.Length];
            Assert.True(isin.TryFormat(span, out int charsWritten));
            Assert.Equal(Isin.Length, charsWritten);
        }

        // UTF8 exact length
        {
            Span<byte> span = stackalloc byte[Isin.Length];
            Assert.True(isin.TryFormat(span, out int bytesWritten));
            Assert.Equal(Isin.Length, bytesWritten);
        }

        // UTF16 extra length
        {
            Span<char> span = stackalloc char[Isin.Length + 10];
            Assert.True(isin.TryFormat(span, out int charsWritten));
            Assert.Equal(Isin.Length, charsWritten);
        }

        // UTF8 extra length
        {
            Span<byte> span = stackalloc byte[Isin.Length + 10];
            Assert.True(isin.TryFormat(span, out int bytesWritten));
            Assert.Equal(Isin.Length, bytesWritten);
        }
    }

    [Fact]
    public void Invalid_TryFormatSpan()
    {
        Isin isin = Isin.Parse("US0378331005");

        // UTF16 too short
        {
            Span<char> span = stackalloc char[Isin.Length - 1];
            Assert.False(isin.TryFormat(span, out int charsWritten));
            Assert.Equal(0, charsWritten);
        }

        // UTF8 too short
        {
            Span<byte> span = stackalloc byte[Isin.Length - 1];
            Assert.False(isin.TryFormat(span, out int bytesWritten));
            Assert.Equal(0, bytesWritten);
        }
    }
}