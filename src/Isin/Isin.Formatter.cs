using System.Numerics;

namespace Yesmey;

public partial struct Isin
{
    string IFormattable.ToString(string? format, IFormatProvider? formatProvider) =>
        ToString();

    public bool TryFormat(Span<char> destination, out int charsWritten) =>
        TryFormatCore(destination, out charsWritten);

    /// <summary>Tries to format the current ISIN into the provided span.</summary>
    /// <param name="utf8Destination">When this method returns, the current as a span of UTF-8 bytes.</param>
    /// <param name="bytesWritten">When this method returns, the number of bytes written into the <paramref name="utf8Destination"/>.</param>
    /// <returns><see langword="true" /> if the formatting was successful; otherwise, <see langword="false" />.</returns>
    public bool TryFormat(Span<byte> utf8Destination, out int bytesWritten) =>
        TryFormatCore(utf8Destination, out bytesWritten);

    /// <inheritdoc/>
    bool ISpanFormattable.TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider) =>
        TryFormatCore(destination, out charsWritten);

    /// <inheritdoc/>
    bool IUtf8SpanFormattable.TryFormat(Span<byte> utf8Destination, out int bytesWritten, ReadOnlySpan<char> format, IFormatProvider? provider) =>
        TryFormatCore(utf8Destination, out bytesWritten);

    private bool TryFormatCore<TChar>(Span<TChar> destination, out int charsWritten) where TChar : unmanaged, IBinaryInteger<TChar>
    {
        if (destination.Length < Length)
        {
            charsWritten = 0;
            return default;
        }

        for (var i = 0; i < Length; i++)
        {
            destination[i] = TChar.CreateTruncating(_data[i]);
        }

        charsWritten = Length;
        return true;
    }
}
