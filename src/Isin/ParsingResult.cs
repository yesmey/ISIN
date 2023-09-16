using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Yesmey;

internal enum ParsingResult
{
    Success,
    Error_Length,
    Error_Prefix,
    Error_BasicCode,
    Error_LastDigit,
    Error_CheckDigit
}

internal static class ParsingOperationExtensions
{
    [DoesNotReturn]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ThrowFormattingError(this ParsingResult errorCodes)
    {
        Debug.Assert(errorCodes != ParsingResult.Success);

        var errorMessage = errorCodes switch
        {
            ParsingResult.Error_Length => $"Invalid Length - must be {Isin.Length} characters",
            ParsingResult.Error_Prefix => "Invalid CountryCode prefix. Must be 2 uppercase characters",
            ParsingResult.Error_BasicCode => "Basic Code (9 characters) must be uppercase alphanumeric",
            ParsingResult.Error_LastDigit => "Last character must be a digit 0-9",
            ParsingResult.Error_CheckDigit => "Could not validate checksum",
            _ => throw new NotImplementedException()
        };
        throw new FormatException(errorMessage);
    }
}
