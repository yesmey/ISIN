using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Yesmey;

public readonly partial struct Isin : ISpanFormattable, ISpanParsable<Isin>, IUtf8SpanFormattable
{
    private readonly IsinData _data;

    public static readonly Isin Empty;
    public const int Length = 12;

    public Isin(byte[] bytes) : this(bytes.AsSpan())
    {
        ArgumentNullException.ThrowIfNull(bytes);
    }

    public Isin(ReadOnlySpan<byte> bytes)
    {
        ArgumentOutOfRangeException.ThrowIfNotEqual(bytes.Length, Length);
        bytes.CopyTo(_data);
    }

    public Isin(ReadOnlySpan<char> chars)
    {
        this = Parse(chars);
    }

    public Isin(string s)
    {
        this = Parse(s);
    }

    public string BasicCode
    {
        get
        {
            Span<char> chars = stackalloc char[]
            {
                (char)_data[2],
                (char)_data[3],
                (char)_data[4],
                (char)_data[5],
                (char)_data[6],
                (char)_data[7],
                (char)_data[8],
                (char)_data[9],
                (char)_data[10]
            };
            return chars.ToString();
        }
    }

    public string CountryCode
    {
        get
        {
            Span<char> chars = stackalloc char[] { (char)_data[0], (char)_data[1] };
            return chars.ToString();
        }
    }

    public int CheckDigit => _data[Length - 1] - '0';

    public byte[] ToArray() => _data[..].ToArray();

    public override string ToString()
    {
        return string.Create(Length, _data, static (c, b) =>
        {
            for (int i = 0; i < c.Length; i++)
                c[i] = (char)b[i];
        });
    }

    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        hashCode.AddBytes(_data[..]);
        return hashCode.ToHashCode();
    }

    public override bool Equals([NotNullWhen(true)] object? o) => o is Isin isin && Equals(this, isin);

    public bool Equals(Isin isin) => Equals(this, isin);

    public static bool operator ==(Isin a, Isin b) => Equals(a, b);

    public static bool operator !=(Isin a, Isin b) => !Equals(a, b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool Equals(in Isin left, in Isin right)
        => left._data[..].SequenceEqual(right._data);

    [InlineArray(Length)]
    private struct IsinData
    {
        private byte _element0;
    }
}
