using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System.Numerics;
using System.Runtime.InteropServices;

BenchmarkSwitcher.FromAssembly(typeof(Tests).Assembly).Run(args);

[DisassemblyDiagnoser]
public class Tests
{
    public record Data(int Length)
    {
        public string Text = new string('9', Length);
        public override string ToString() => $"{Length}";
    }
    public IEnumerable<object> NumberStrings()
    {
        yield return new Data(10);
        yield return new Data(200 - 10);
        yield return new Data(200);
        yield return new Data(200 + 10);
        yield return new Data(500);
        yield return new Data(616 - 10);
        yield return new Data(616);
        yield return new Data(616 + 10);
        yield return new Data(1000);
        yield return new Data(1233 - 10);
        yield return new Data(1233);
        yield return new Data(1233 + 10);
        yield return new Data(2000);
        yield return new Data(2466 - 10);
        yield return new Data(2466);
        yield return new Data(2466 + 10);
        yield return new Data(3000);
        yield return new Data(3100);
        yield return new Data(3200 - 10);
        yield return new Data(3200);
        yield return new Data(3200 + 10);
        yield return new Data(3300);
        yield return new Data(4000);
        yield return new Data(4932 - 50);
        yield return new Data(4932 + 50);
        yield return new Data(7500);
        yield return new Data(10000);
        yield return new Data(15000);
        yield return new Data(20000 - 1000);
        yield return new Data(20000);
        yield return new Data(20000 + 1000);
        yield return new Data(25000);
        yield return new Data(50000);
    }


    [Benchmark]
    [ArgumentsSource(nameof(NumberStrings))]
    public BigInteger Parse(Data data)
        => BigInteger.Parse(data.Text);
}