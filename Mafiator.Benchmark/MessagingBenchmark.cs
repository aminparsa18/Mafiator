using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using CommunityToolkit.Mvvm.Messaging;
using MessagePipe;
using Microsoft.Extensions.DependencyInjection;

namespace Mafiator.Benchmark;

[SimpleJob(RunStrategy.ColdStart, launchCount: 30)]
[MemoryDiagnoser(false)]
public class MessagingBenchmark
{
    private IPublisher<string>? _publisher;

    [GlobalSetup]
    public void Setup()
    {
        var collection = ServiceProviderFactory.BuildServiceProvider();
        _publisher = collection.GetRequiredService<IPublisher<string>>();
    }

    [Benchmark]
    public void MessagePipeSend() => _publisher.Publish("Koskeshaa");

    [Benchmark]
    public void WeakReferenceSend() => WeakReferenceMessenger.Default.Send("Koskeshaa");
}
