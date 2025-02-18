using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System.Numerics;
using System.Runtime.InteropServices;

[MemoryDiagnoser(false)]
[HideColumns("Job", "Error", "StdDev", "Median", "RatioSD")]
public class AddTests
{
    [Params([
        100000,
        1000000,
    ])]
    public int N;

    BigInteger big1, big2;

    [GlobalSetup]
    public void Setup()
    {
        var rnd = new Random(227);
        var bytes = new byte[N];
        rnd.NextBytes(bytes);
        big1 = new BigInteger(bytes);
        rnd.NextBytes(bytes);
        big2 = new BigInteger(bytes);
    }

    [Benchmark]
    public BigInteger Add()
    {
        return big1 + big2;
    }

    [Benchmark]
    public BigInteger Subtract()
    {
        return big1 - big2;
    }
}