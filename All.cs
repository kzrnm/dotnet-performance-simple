using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using System.Numerics;

[MemoryDiagnoser(false)]
[HideColumns("Job", "Error", "StdDev", "Median", "RatioSD")]
public class AllTest
{
    static string NinesString = new string('9', 100000);
    static string NinesString2 = new string('9', 100000 - 1);
    static string NinesString3 = new string('9', 100000 / 2);

    BigInteger Nines = BigInteger.Parse(NinesString);
    BigInteger Nines2 = BigInteger.Parse(NinesString2);
    BigInteger Nines3 = BigInteger.Parse(NinesString3);


    [Benchmark] public BigInteger Add() => Nines + Nines2;
    [Benchmark] public BigInteger Multiply() => Nines * Nines2;
    [Benchmark] public BigInteger Divide() => Nines / Nines3;

    [Benchmark] public BigInteger LeftShift() => Nines << 20;
    [Benchmark] public BigInteger RightShift() => Nines >> 20;
    [Benchmark] public BigInteger RightShiftUnsigned() => Nines >>> 20;

    [Benchmark] public BigInteger DecimalParse() => BigInteger.Parse(NinesString);
    char[] DecimalStringDest = new char[1000010];
    [Benchmark] public void DecimalString() => Nines.TryFormat(DecimalStringDest, out _);
}