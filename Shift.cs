using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using System.Numerics;

[MemoryDiagnoser(false)]
[HideColumns("Job", "Error", "StdDev", "Median", "RatioSD")]
public class ShiftTest
{
    const int shiftSize = (1 << 23) + 13;
    BigInteger positive, negative;

    [GlobalSetup]
    public void Setup()
    {
        var bytes = new byte[1 << 25];
        new Random(227).NextBytes(bytes);
        positive = new BigInteger(bytes, isUnsigned: true);
        negative = -positive;
    }

    [Benchmark] public BigInteger PositiveLeftShift() => positive << shiftSize;
    [Benchmark] public BigInteger NegativeLeftShift() => negative << shiftSize;

    [Benchmark] public BigInteger PositiveRightShift() => positive >> shiftSize;
    [Benchmark] public BigInteger NegativeRightShift() => negative >> shiftSize;

    [Benchmark] public BigInteger PositiveUnsignedRightShift() => positive >>> shiftSize;
    [Benchmark] public BigInteger NegativeUnsignedRightShift() => negative >>> shiftSize;

    [Benchmark] public BigInteger PositiveRotateLeft() => BigInteger.RotateLeft(positive, shiftSize);
    [Benchmark] public BigInteger NegativeRotateLeft() => BigInteger.RotateLeft(negative, shiftSize);

    [Benchmark] public BigInteger PositiveRotateRight() => BigInteger.RotateRight(positive, shiftSize);
    [Benchmark] public BigInteger NegativeRotateRight() => BigInteger.RotateRight(negative, shiftSize);
}