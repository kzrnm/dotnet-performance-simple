﻿using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Configs;
using System.Numerics;
using System.Runtime.InteropServices;

[DisassemblyDiagnoser]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByMethod)]
public class PerformanceTest
{
    [Benchmark]
    [Arguments(100000)]
    [Arguments(250001)]
    public string Run(int n)
    {
        var result = new System.Text.StringBuilder();
        var k = BinarySearch(n);
        var (p, q) = SumTerms(0, k - 1);
        p += q;
        var a = BigInteger.Pow(new BigInteger(10), n - 1);
        var answer = p * a / q;
        var answerStr = answer.ToString();
        Span<char> sb = stackalloc char[10];
        for (var i = 0; i < n; i += 10)
        {
            var count = i + 10;
            if (count > n)
            {
                count = n;
            }
            for (var j = i; j < i + 10; j++)
            {
                if (j < n)
                {
                    sb[j - i] = answerStr[j];
                }
                else
                {
                    sb[j - i] = ' ';
                }
            }

            result.AppendLine($"{new String(sb)}\t:{count}");
        }
        return result.ToString();
    }

    static (BigInteger, BigInteger) SumTerms(int a, int b)
    {
        if (b == a + 1)
        {
            return (new BigInteger(1), new BigInteger(b));
        }
        var mid = (a + b) / 2;
        var (pLeft, qLeft) = SumTerms(a, mid);
        var (pRight, qRight) = SumTerms(mid, b);
        return (pLeft * qRight + pRight, qLeft * qRight);
    }

    static int BinarySearch(int n)
    {
        var a = 0;
        var b = 1;
        while (!TestK(n, b))
        {
            a = b;
            b *= 2;
        }
        while (b - a > 1)
        {
            var m = (a + b) / 2;
            if (TestK(n, m))
            {
                b = m;
            }
            else
            {
                a = m;
            }
        }
        return b;
    }

    static bool TestK(int n, int k)
    {
        if (k < 0)
        {
            return false;
        }
        var lnKFactorial = k * (Math.Log((double)k) - 1) + 0.5 * Math.Log(Math.PI * 2);
        var log10KFactorial = lnKFactorial / Math.Log(10);
        return log10KFactorial >= (double)(n + 50);
    }
}