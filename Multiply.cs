using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System.Numerics;
using System.Runtime.InteropServices;

[MemoryDiagnoser(false)]
[HideColumns("Job", "Error", "StdDev", "Median", "RatioSD")]
public class MultiplyTests
{
    public IEnumerable<object> GetMultiplyArgs()
    {
        var bytes = new byte[1000000];
        bytes.AsSpan().Fill(byte.MaxValue);
        var lengths = new int[] { 1000, 10000, 100000, 1000000 };
        for (int i = lengths.Length - 1; i >= 0; i--)
        {
            var largeLength = lengths[i];
            var large = Make(largeLength);
            foreach (var p in new double[] { 1, 0.75, 0.5, 0.25 })
            {
                var smallLength = (int)(p * lengths[i]);
                var small = Make(smallLength);
                yield return new Data($"{largeLength:D7}-{smallLength:D7}", large, small);
            }
        }
        BigInteger Make(int length)
        {
            return new BigInteger(bytes.AsSpan().Slice(0, length), isUnsigned: true);
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