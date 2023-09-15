using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Isin;

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
        // format and provider are explicitly ignored
        TryFormatCore(destination, out charsWritten);

    /// <inheritdoc/>
    bool IUtf8SpanFormattable.TryFormat(Span<byte> utf8Destination, out int bytesWritten, ReadOnlySpan<char> format, IFormatProvider? provider) =>
        // format and provider are explicitly ignored
        TryFormatCore(utf8Destination, out bytesWritten);

    private bool TryFormatCore<TChar>(Span<TChar> destination, out int charsWritten) where TChar : unmanaged, IBinaryInteger<TChar>
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(destination.Length, IsinData.Length);

        Span<TChar> tmpDestination = stackalloc TChar[IsinData.Length];

        if (tmpDestination.TryCopyTo(destination))
        {
            charsWritten = IsinData.Length;
            return true;
        }

        charsWritten = 0;
        return false;
    }

}
