using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System.Numerics;
using System.Runtime.InteropServices;

[MemoryDiagnoser(false)]
[HideColumns("Job", "Error", "StdDev", "Median", "RatioSD")]
public class MultipleTests
{
    [Params([
        // 1<<8,
        // 1<<9,
        // 1<<10,
        // 1000,
        // 10000,
        100000,
        1000000,
    ])]
    public int N;


    BigInteger big1, big2;
    string decString;


    [GlobalSetup]
    public void Setup()
    {
        var rnd = new Random(227);
        var bytes = new byte[N];
        rnd.NextBytes(bytes);
        big1 = new BigInteger(bytes);
        rnd.NextBytes(bytes);
        big2 = new BigInteger(bytes[(N / 2)..]);

        decString = new string(Enumerable.Repeat(rnd, N).Select(r => (char)(r.Next(10) + '0')).ToArray());
    }

    [Benchmark]
    public BigInteger Square()
    {
        return big1 * big1;
    }

    [Benchmark]
    public BigInteger Multiply()
    {
        return big1 * big2;
    }

    [Benchmark]
    public BigInteger Divide()
    {
        return big1 / big2;
    }

    [Benchmark]
    public BigInteger Parse()
    {
        return BigInteger.Parse(decString);
    }

    // [Benchmark]
    // public string String()
    // {
    //     return big1.ToString();
    // }
}