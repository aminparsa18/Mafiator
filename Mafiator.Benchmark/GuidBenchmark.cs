using BenchmarkDotNet.Attributes;
using Org.BouncyCastle.Bcpg.OpenPgp;

namespace Mafiator.Benchmark;

[MemoryDiagnoser, SimpleJob(launchCount: 1, warmupCount: 1, invocationCount: 1000)]
public class GuidBenchmark
{
    private string id;

    [GlobalSetup]
    public void Setup()
    {
        id = "3c95cc1c-8acc-437b-bd0c-0b35b9b75b8a";
    }

    [Benchmark]
    public Guid NormalParse()
    {
        return Guid.Parse(id);
    }

    [Benchmark]
    public Guid SpanParse()
    {
        return Guid.Parse(id.AsSpan());
    }
}
