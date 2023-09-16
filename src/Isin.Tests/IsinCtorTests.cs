namespace Yesmey.Tests;

public class IsinCtorTests
{
    [Fact]
    public void CtorByteArray_OutOfRange()
    {
        var bytes = new byte[] { 1, 2, 3 };
        Assert.Throws<ArgumentOutOfRangeException>(() => new Isin(bytes));

        bytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };
        Assert.Throws<ArgumentOutOfRangeException>(() => new Isin(bytes));
    }

    [Fact]
    public void CtorByteArray()
    {
        var bytes = Enumerable.Range(0, 12).Select(x => (byte)0).ToArray();
        var isin = new Isin(bytes);
        Assert.Equal(Isin.Empty, isin);
    }
}
