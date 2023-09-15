using System.Buffers;
using System.Diagnostics;
using System.Text;

namespace Isin;

public partial struct Isin
{
    public static Isin Parse(string s)
    {
        ArgumentNullException.ThrowIfNull(s);
        return ParseIsin(s);
    }

    public static Isin Parse(ReadOnlySpan<char> s) =>
        ParseIsin(s);

    public static Isin Parse(ReadOnlySpan<char> s, IFormatProvider? provider) =>
        ParseIsin(s);

    public static Isin Parse(string s, IFormatProvider? provider) =>
        ParseIsin(s.AsSpan());

    public static bool TryParse(ReadOnlySpan<char> s, out Isin result)
    {
        return TryParseIsin(s, out result);
    }

    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Isin result)
    {
        return TryParseIsin(s, out result);
    }

    public static bool TryParse(string? s, out Isin result)
    {
        return TryParseIsin(s.AsSpan(), out result);
    }

    public static bool TryParse(string? s, IFormatProvider? provider, out Isin result)
    {
        return TryParseIsin(s.AsSpan(), out result);
    }

    private static Isin ParseIsin(ReadOnlySpan<char> span)
    {
        if (!TryParseIsin(span, out var result))
            throw new FormatException();

        return result;
    }

    private static bool TryParseIsin(ReadOnlySpan<char> span, out Isin result)
    {
        if (ValidateFormat(span))
        {
            var checkDigit = '0' + Checksum.Calculate(span);

            Span<byte> bytes = stackalloc byte[Length];
            OperationStatus conversionStatus = Ascii.FromUtf16(span, bytes, out int test);
            Debug.Assert(conversionStatus == OperationStatus.Done);

            result = new Isin(bytes);
            return true;
        }

        result = default;
        return false;
    }

    private static bool ValidateFormat(ReadOnlySpan<char> span)
    {
        if (span.Length != Length)
            return false;

        // Validate Prefix
        foreach (var chr in span[0..2])
        {
            if (!char.IsAsciiLetterUpper(chr))
                return false;
        }

        // Validate Basic Code
        foreach (var chr in span[2..11])
        {
            if (!char.IsAsciiDigit(chr) && !char.IsAsciiLetterUpper(chr))
                return false;
        }

        // Validate last digit
        if (!char.IsAsciiDigit(span[11]))
            return false;

        return true;
    }
}
