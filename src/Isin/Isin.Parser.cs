using System.Buffers;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace Yesmey;

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
        var parsingResult = ValidateFormat(span);
        if (parsingResult != ParsingResult.Success)
        {
            parsingResult.ThrowFormattingError();
        }

        return CreateIsinFromChars(span);
    }

    private static bool TryParseIsin(ReadOnlySpan<char> span, out Isin result)
    {
        if (ValidateFormat(span) == ParsingResult.Success)
        {
            result = CreateIsinFromChars(span);
            return true;
        }

        result = default;
        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Isin CreateIsinFromChars(ReadOnlySpan<char> span)
    {
        Span<byte> bytes = stackalloc byte[Length];
        OperationStatus conversionStatus = Ascii.FromUtf16(span, bytes, out int test);
        Debug.Assert(conversionStatus == OperationStatus.Done);
        return new Isin(bytes);
    }

    private static ParsingResult ValidateFormat(ReadOnlySpan<char> span)
    {
        if (span.Length != Length)
            return ParsingResult.Error_Length;

        // Validate Prefix
        foreach (var chr in span[0..2])
        {
            if (!char.IsAsciiLetterUpper(chr))
                return ParsingResult.Error_Prefix;
        }

        // Validate Basic Code
        foreach (var chr in span[2..11])
        {
            if (!char.IsAsciiDigit(chr) && !char.IsAsciiLetterUpper(chr))
                return ParsingResult.Error_BasicCode;
        }

        // Validate last digit
        if (!char.IsAsciiDigit(span[11]))
            return ParsingResult.Error_LastDigit;

        var checkDigit = span[11];
        var checkSum = (char)(Checksum.Calculate(span[0..11]) + '0');
        if (checkDigit != checkSum)
            return ParsingResult.Error_CheckDigit;

        return ParsingResult.Success;
    }
}
