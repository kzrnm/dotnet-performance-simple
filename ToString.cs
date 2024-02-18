using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Configs;
using System.Numerics;
using System.Runtime.InteropServices;

[DisassemblyDiagnoser]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByMethod)]
public class ToStringTest
{
    [Params(1000, 10000, 100000)]
    public int N;

    BigInteger b1, b2, b3;

    [GlobalSetup]
    public void Setup()
    {
        b1 = BigInteger.Parse("1" + new string('0', N));
        b2 = BigInteger.Parse("1" + new string('0', N - 1) + "1");
        b3 = BigInteger.Parse(new string('9', N));
    }

    [Benchmark] public string PowerOfTen() => b1.ToString();
    [Benchmark] public string PowerOfTenPlusOne() => b2.ToString();
    [Benchmark] public string PowerOfTenMinusOne() => b3.ToString();
}