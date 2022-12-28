using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using Mafiator.Benchmark.Models;
using MemoryPack;
using MessagePack;
using System.Text.Json;

namespace Mafiator.Benchmark;

[SimpleJob(RunStrategy.ColdStart, launchCount: 10)]
[MemoryDiagnoser(false)]
public class SerializationBenchmark
{
    private MessagePackObject _messagePackObject;
    private MemoryPackObject _memoryPackObject;
    
    [GlobalSetup]
    public void Setup()
    {
        _messagePackObject = new MessagePackObject() { Name = "Mafiator"};
        _memoryPackObject = new MemoryPackObject() { Name = "Mafiator" };
    }

    [Benchmark]
    public string SerializeJson() => JsonSerializer.Serialize(_messagePackObject);

    [Benchmark]
    public byte[] SerializeMessagePack() => MessagePackSerializer.Serialize(_messagePackObject);

    [Benchmark]
    public byte[] SerializeMemoryPack() => MemoryPackSerializer.Serialize(_memoryPackObject);
}