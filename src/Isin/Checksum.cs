using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Yesmey;

internal static class Checksum
{
    private static ReadOnlySpan<byte> Widths => new byte[]
    {
        1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
        2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
        2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
        2, 2, 2, 2, 2, 2
    };

    private static ReadOnlySpan<byte> Odds => new byte[]
    {
        0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
        2, 3, 4, 5, 6, 7, 8, 9, 0, 1,
        4, 5, 6, 7, 8, 9, 0, 1, 2, 3,
        6, 7, 8, 9, 0, 1
    };

    private static ReadOnlySpan<byte> Evens => new byte[]
    {
        0, 2, 4, 6, 8, 1, 3, 5, 7, 9,
        1, 3, 5, 7, 9, 2, 4, 6, 8, 0,
        2, 4, 6, 8, 0, 3, 5, 7, 9, 1,
        3, 5, 7, 9, 1, 4
    };

    public static int Calculate(ReadOnlySpan<char> span)
    {
        int sum = 0;
        uint idx = 0;

        for (var i = span.Length - 1; i >= 0; i--)
        {
            var v = CharValue(span[i]);

            if (sum > byte.MaxValue - 9)
                sum %= 10;

            sum += (idx % 2) == 0 ? Evens[v] : Odds[v];
            idx += Widths[v];
        }

        sum %= 10;

        var diff = 10 - sum;
        return diff == 10 ? 0 : diff;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int CharValue(char b)
    {
        if (char.IsAsciiDigit(b))
        {
            return b - '0';
        }
        else
        {
            Debug.Assert(char.IsAsciiLetterUpper(b));
            return b - 'A' + 10;
        }
    }
}