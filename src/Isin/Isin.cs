using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Isin;

public readonly partial struct Isin : ISpanFormattable, ISpanParsable<Isin>, IUtf8SpanFormattable
{
    private const int Length = 12;
    private readonly IsinData _data;

    public static readonly Isin Empty = new(new byte[Length]);

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

    public string Value
    {
        get
        {
            return string.Create(Length, _data, static (c, b) =>
            {
                for (int i = 0; i < c.Length; i++)
                    c[i] = (char)b[i];
            });
        }
    }

    public string CountryCode
    {
        get
        {
            Span<char> stack = stackalloc char[] { (char)_data[0], (char)_data[1] };
            return stack.ToString();
        }
    }

    public char CheckDigit => (char)_data[Length - 1];

    public byte[] ToArray() => _data[..].ToArray();

    public override string ToString()
    {
        Span<char> span = stackalloc char[Length];
        TryFormat(span, out _);
        return span.ToString();
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
        => left._data[..].SequenceEqual(right._data[..]);

    [InlineArray(Length)]
    private struct IsinData
    {
        private byte _element0;
    }
}
