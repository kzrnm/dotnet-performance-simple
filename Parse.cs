using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Configs;
using System.Numerics;
using System.Runtime.InteropServices;

[DisassemblyDiagnoser]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByMethod)]
public class ParseTest
{
    [Params(1000, 10000, 100000, 1000000)]
    public int N;

    string s1, s2, s3;

    [GlobalSetup]
    public void Setup()
    {
        s1 = "1" + new string('0', N);
        s2 = "1" + new string('0', N - 1) + "1";
        s3 = new string('9', N);
    }

    [Benchmark] public BigInteger PowerOfTen() => BigInteger.Parse(s1);
    [Benchmark] public BigInteger PowerOfTenPlusOne() => BigInteger.Parse(s2);
    [Benchmark] public BigInteger PowerOfTenMinusOne() => BigInteger.Parse(s3);
}