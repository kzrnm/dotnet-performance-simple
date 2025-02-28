using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System.Numerics;
using System.Runtime.InteropServices;

[MemoryDiagnoser(false)]
[HideColumns("Job", "Error", "StdDev", "Median", "RatioSD")]
public class MultiplySomeSizeTests
{
    public IEnumerable<object> GetMultiplyArgs()
    {
        var rnd = new Random(227);
        var bytes = new byte[1000000];
        int[] lengths =
        [
            100,
            500,
            1000,
            10000,
            100000,
            1000000,
        ];
        for (int i = lengths.Length - 1; i >= 0; i--)
        {
            var largeLength = lengths[i];
            var large = Make(largeLength);
            foreach (var p in new double[] { 0.999999999999999, 0.75, 0.5, 0.25 })
            {
                var smallLength = (int)(p * lengths[i]);
                var small = Make(smallLength);
                yield return new Data($"{largeLength:D7}-{smallLength:D7}", large, small);
            }

            yield return new Data($"Square{largeLength:D7}", large, large);
        }
        BigInteger Make(int length)
        {
            var b = bytes.AsSpan().Slice(0, length);
            rnd.NextBytes(b);
            return new BigInteger(b, isUnsigned: true);
        }
    }

    public record Data(string Name, BigInteger Large, BigInteger Small)
    {
        public override string ToString() => Name;
    }

    [Benchmark]
    [ArgumentsSource(nameof(GetMultiplyArgs))]
    public BigInteger Multiply(Data data)
    {
        return data.Large * data.Small;
    }
}