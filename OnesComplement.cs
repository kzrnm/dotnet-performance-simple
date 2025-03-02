using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;

[MemoryDiagnoser(false)]
[HideColumns("Job", "Error", "StdDev", "Median", "RatioSD")]
public class OnesComplementTest
{
    uint[] array;
    [Params([
        7,
        8,
        15,
        16
    ])]
    public int N;

    [GlobalSetup]
    public void Setup()
    {
        array = new uint[N];
        new Random(227).NextBytes(MemoryMarshal.AsBytes(array.AsSpan()));
    }

    [Benchmark(Baseline = true)] public void OnesComplement() => DangerousMakeOnesComplement(array);
    [Benchmark] public void OnesComplementWithIf() => DangerousMakeOnesComplementWithIf(array);

    // Do an in-place one's complement. "Dangerous" because it causes
    // a mutation and needs to be used with care for immutable types.
    public static void DangerousMakeOnesComplementWithIf(Span<uint> d)
    {
        // Given a number:
        //     XXXXXXXXXXX
        // where Y is non-zero,
        // The result of one's complement is
        //     AAAAAAAAAAA
        // where A = ~X

        int offset = 0;
        ref uint start = ref MemoryMarshal.GetReference(d);

        if (Vector512.IsHardwareAccelerated) while (d.Length - offset >= Vector512<uint>.Count)
            {
                Vector512<uint> complement = ~Vector512.LoadUnsafe(ref start, (nuint)offset);
                Vector512.StoreUnsafe(complement, ref start, (nuint)offset);
                offset += Vector512<uint>.Count;
            }

        if (Vector256.IsHardwareAccelerated) while (d.Length - offset >= Vector256<uint>.Count)
            {
                Vector256<uint> complement = ~Vector256.LoadUnsafe(ref start, (nuint)offset);
                Vector256.StoreUnsafe(complement, ref start, (nuint)offset);
                offset += Vector256<uint>.Count;
            }

        if (Vector128.IsHardwareAccelerated) while (d.Length - offset >= Vector128<uint>.Count)
            {
                Vector128<uint> complement = ~Vector128.LoadUnsafe(ref start, (nuint)offset);
                Vector128.StoreUnsafe(complement, ref start, (nuint)offset);
                offset += Vector128<uint>.Count;
            }
        for (; offset < d.Length; offset++)
        {
            d[offset] = ~d[offset];
        }
    }

    // Do an in-place one's complement. "Dangerous" because it causes
    // a mutation and needs to be used with care for immutable types.
    public static void DangerousMakeOnesComplement(Span<uint> d)
    {
        // Given a number:
        //     XXXXXXXXXXX
        // where Y is non-zero,
        // The result of one's complement is
        //     AAAAAAAAAAA
        // where A = ~X

        int offset = 0;
        ref uint start = ref MemoryMarshal.GetReference(d);

        while (Vector512.IsHardwareAccelerated && d.Length - offset >= Vector512<uint>.Count)
        {
            Vector512<uint> complement = ~Vector512.LoadUnsafe(ref start, (nuint)offset);
            Vector512.StoreUnsafe(complement, ref start, (nuint)offset);
            offset += Vector512<uint>.Count;
        }

        while (Vector256.IsHardwareAccelerated && d.Length - offset >= Vector256<uint>.Count)
        {
            Vector256<uint> complement = ~Vector256.LoadUnsafe(ref start, (nuint)offset);
            Vector256.StoreUnsafe(complement, ref start, (nuint)offset);
            offset += Vector256<uint>.Count;
        }

        while (Vector128.IsHardwareAccelerated && d.Length - offset >= Vector128<uint>.Count)
        {
            Vector128<uint> complement = ~Vector128.LoadUnsafe(ref start, (nuint)offset);
            Vector128.StoreUnsafe(complement, ref start, (nuint)offset);
            offset += Vector128<uint>.Count;
        }
        for (; offset < d.Length; offset++)
        {
            d[offset] = ~d[offset];
        }
    }
}