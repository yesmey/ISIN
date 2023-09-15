using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Isin;

public readonly partial struct Isin : ISpanFormattable, ISpanParsable<Isin>, IUtf8SpanFormattable
{
    private readonly IsinData _data;

    public static readonly Isin Empty;

    public Isin(byte[] bytes) :
        this(new ReadOnlySpan<byte>(bytes))
    {
        ArgumentNullException.ThrowIfNull(bytes);
    }

    public Isin(ReadOnlySpan<byte> bytes)
    {
        ArgumentOutOfRangeException.ThrowIfNotEqual(bytes.Length, IsinData.Length);
        _data = MemoryMarshal.Read<IsinData>(bytes);
    }

    public Isin(ReadOnlySpan<char> chars)
    {
        this = Parse(chars);
    }

    public Isin(string s)
    {
        this = Parse(s);
    }

    public override string ToString()
    {
        Span<char> span = stackalloc char[IsinData.Length];
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
        private byte _element;
        public const int Length = 12;
    }
}
