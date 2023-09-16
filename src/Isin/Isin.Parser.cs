using System.Buffers;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
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
        (bool valid, ErrorCodes errorCodes) = ValidateFormat(span);
        if (!valid)
        {
            ThrowFormattingError(errorCodes);
        }

        return CreateIsinFromChars(span);
    }

    private static bool TryParseIsin(ReadOnlySpan<char> span, out Isin result)
    {
        (bool valid, _) = ValidateFormat(span);
        if (valid)
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

    private static (bool, ErrorCodes) ValidateFormat(ReadOnlySpan<char> span)
    {
        if (span.Length != Length)
            return (false, ErrorCodes.Length);

        // Validate Prefix
        foreach (var chr in span[0..2])
        {
            if (!char.IsAsciiLetterUpper(chr))
                return (false, ErrorCodes.Prefix);
        }

        // Validate Basic Code
        foreach (var chr in span[2..11])
        {
            if (!char.IsAsciiDigit(chr) && !char.IsAsciiLetterUpper(chr))
                return (false, ErrorCodes.BasicCode);
        }

        // Validate last digit
        if (!char.IsAsciiDigit(span[11]))
            return (false, ErrorCodes.LastDigit);

        var checkDigit = span[11];
        var checkSum = (char)(Checksum.Calculate(span[0..11]) + '0');
        if (checkDigit != checkSum)
            return (false, ErrorCodes.CheckDigit);

        return (true, ErrorCodes.None);
    }

    [DoesNotReturn]
    [MethodImpl(MethodImplOptions.NoInlining)]
    private static void ThrowFormattingError(ErrorCodes errorCodes)
    {
        var errorMessage = errorCodes switch
        {
            ErrorCodes.Length => $"Invalid Length - must be {Length} characters",
            ErrorCodes.Prefix => "Invalid CountryCode prefix. Must be 2 uppercase characters",
            ErrorCodes.BasicCode => "Basic Code (9 characters) must be uppercase alphanumeric",
            ErrorCodes.LastDigit => "Last character must be a digit 0-9",
            ErrorCodes.CheckDigit => "Could not validate checksum",
            _ => throw new NotImplementedException()
        };
        throw new FormatException(errorMessage);
    }

    private enum ErrorCodes
    {
        None,
        Length,
        Prefix,
        BasicCode,
        LastDigit,
        CheckDigit
    }
}
