using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Configs;
using System.Numerics;
using System.Runtime.InteropServices;

[DisassemblyDiagnoser]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByMethod)]
public class DivideTest
{
    public IEnumerable<object> ValuesSameSize()
    {
        yield return new BigIntegers(new[] { 16, 16 });
        yield return new BigIntegers(new[] { 1024, 1024 });
        yield return new BigIntegers(new[] { 65536, 65536 });
    }

    public IEnumerable<object> ValuesHalfSize()
    {
        yield return new BigIntegers(new[] { 1024, 1024 / 2 });
        yield return new BigIntegers(new[] { 65536, 65536 / 2 });
        yield return new BigIntegers(new[] { 262144,262144 / 2 });
    }

    public IEnumerable<object> ValuesSameOrHalfSize()
    {
        foreach (var item in ValuesSameSize()) yield return item;
        foreach (var item in ValuesHalfSize()) yield return item;
    }

    [Benchmark]
    [ArgumentsSource(nameof(ValuesHalfSize))]
    public BigInteger Divide(BigIntegers arguments)
        => BigInteger.Divide(arguments.Left, arguments.Right);

    // [Benchmark]
    // [ArgumentsSource(nameof(ValuesHalfSize))]
    // public BigInteger Remainder(BigIntegers arguments)
    //     => BigInteger.Remainder(arguments.Left, arguments.Right);

    public class BigIntegerData
    {
        public string Text { get; }
        public byte[] Bytes { get; }
        public BigInteger Value { get; }

        public BigIntegerData(string numberString)
        {
            Text = numberString;
            Value = BigInteger.Parse(numberString);
            Bytes = Value.ToByteArray();
        }

        public override string ToString() => Text;
    }

    public class BigIntegers
    {
        private readonly int[] _bitCounts;
        private readonly string _description;

        public BigInteger Left { get; }
        public BigInteger Right { get; }
        public BigInteger Other { get; }

        public BigIntegers(int[] bitCounts)
        {
            _bitCounts = bitCounts;
            _description = $"{string.Join(",", _bitCounts)} bits";
            var values = GenerateBigIntegers(bitCounts);

            Left = values[0];
            Right = values[1];
            Other = values.Length == 3 ? values[2] : BigInteger.Zero;
        }

        public BigIntegers(BigInteger left, BigInteger right, string description)
        {
            Left = left;
            Right = right;
            _description = description;
        }

        public override string ToString() => _description;

        private static BigInteger[] GenerateBigIntegers(int[] bitCounts)
        {
            Random random = new Random(1138); // we always use the same seed to have repeatable results!

            BigInteger[] result = new BigInteger[bitCounts.Length];

            for (int i = 0; i < bitCounts.Length; i++)
                result[i] = CreateRandomInteger(random, bitCounts[i]);

            return result;
        }

        private static BigInteger CreateRandomInteger(Random random, int bits)
        {
            byte[] value = new byte[(bits + 8) / 8];
            BigInteger result = BigInteger.Zero;

            while (result.IsZero)
            {
                random.NextBytes(value);

                // ensure actual bit count (remaining bits not set)
                // ensure positive value (highest-order bit not set)
                value[value.Length - 1] &= (byte)(0xFF >> 8 - bits % 8);

                result = new BigInteger(value);
            }

            return result;
        }
    }
}