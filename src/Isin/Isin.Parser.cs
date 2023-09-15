using System.Buffers;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
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

    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out Isin result)
    {
        return TryParseIsin(s, out result);
    }

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Isin result)
    {
        return TryParseIsin(s.AsSpan(), out result);
    }

    private static Isin ParseIsin(ReadOnlySpan<char> span)
    {
        //Span<byte> bytes = stackalloc byte[12];
        //var conversionStatus = Ascii.FromUtf16(span, bytes, out _);
        //if (conversionStatus != OperationStatus.Done)
        //    throw new Exception();

        return default;
    }

    private static bool TryParseIsin(ReadOnlySpan<char> span, out Isin result)
    {
        if (Validate(span))
        {
            Span<byte> bytes = stackalloc byte[IsinData.Length];
            OperationStatus conversionStatus = Ascii.FromUtf16(span, bytes, out _);
            Debug.Assert(conversionStatus == OperationStatus.Done);
            result = new Isin(bytes);
        }

        result = default;
        return false;
    }

    private static bool Validate(ReadOnlySpan<char> span)
    {
        if (span.Length != 12)
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
