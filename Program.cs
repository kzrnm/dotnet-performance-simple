using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Toolchains.CsProj;
using BenchmarkDotNet.Toolchains.DotNetCli;
using System.Numerics;
using System.Runtime.InteropServices;

public class Program
{
    static void Main(string[] args)
    {
#if NoCorerun
        _ = BenchmarkRunner.Run(typeof(Program).Assembly, new NoCorerunConfig(), args);
#else
        BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
#endif
    }
}
public class NoCorerunConfig : ManualConfig
{
    public NoCorerunConfig()
    {
        AddExporter(BenchmarkDotNet.Exporters.MarkdownExporter.GitHub);
        AddExporter(BenchmarkDotNet.Exporters.HtmlExporter.Default);
        AddLogger(ConsoleLogger.Default);
        AddJob([
            Job.ShortRun.WithToolchain(CsProjCoreToolchain.NetCoreApp80),
            Job.ShortRun.WithToolchain(CsProjCoreToolchain.NetCoreApp90),
            //Job.ShortRun.WithToolchain(CsProjCoreToolchain.From(new("net10.0", null, ".NET 10.0"))),
        ]);
        AddColumnProvider(DefaultColumnProviders.Instance);
        SummaryStyle = SummaryStyle.Default.WithRatioStyle(BenchmarkDotNet.Columns.RatioStyle.Value)
        //.WithTimeUnit(Perfolizer.Horology.TimeUnit.Millisecond)
        ;
    }
}
