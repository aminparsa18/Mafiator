using BenchmarkDotNet.Attributes;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Text;
using System.Text.Json;

namespace Mafiator.Benchmark.Avatars;

[MemoryDiagnoser, SimpleJob(launchCount: 1, warmupCount: 1, invocationCount: 1000)]
public class AvatarsApiBenchmark
{
    private static HttpClient FastEndpointClient { get; } = new WebApplicationFactory<Api.Program>().CreateClient();

    [Benchmark(Baseline = true)]
    public Task ArdalisEndpoints()
    {
        var msg = new HttpRequestMessage()
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"{FastEndpointClient.BaseAddress}api/v1/avatars"),
        };

        return FastEndpointClient.SendAsync(msg);
    }

    [Benchmark]
    public Task FastEndpoints()
    {
        var msg = new HttpRequestMessage()
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"{FastEndpointClient.BaseAddress}api/avatars"),
        };

        return FastEndpointClient.SendAsync(msg);
    }
}