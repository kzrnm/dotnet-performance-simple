using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using System.Numerics;

[DisassemblyDiagnoser]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByMethod)]
public class ToStringTest
{
    [Params(100, 1000, 10000, 100000)]
    public int N;

    BigInteger b;

    [GlobalSetup]
    public void Setup()
    {
        b = BigInteger.Parse(new string('9', N));
    }

    [Benchmark] public string DecimalString() => b.ToString();
}