using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using System.Numerics;

[MemoryDiagnoser(false)]
[HideColumns("Job", "Error", "StdDev", "Median", "RatioSD")]
public class ToStringTest
{
    char[] _dest = new char[1000010];
    [Params(100, 1000, 10000, 100000, 1000000)]
    public int N;

    BigInteger b;

    [GlobalSetup]
    public void Setup()
    {
        b = BigInteger.Parse(new string('9', N));
    }

    [Benchmark] public void DecimalString() => b.TryFormat(_dest, out _);
}