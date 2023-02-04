using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using JM.LinqFaster.SIMD;
using SimdLinq;
using System.Buffers;

namespace Mafiator.Benchmark;

[SimpleJob(RunStrategy.ColdStart, launchCount: 10)]
[MemoryDiagnoser(false)]
public class SimdBenchmark
{
    //int[] _array;
    IMemoryOwner<char> owner;

    [GlobalSetup]
    public void Setup()
    {
        owner = MemoryPool<char>.Shared.Rent();
        //_array = Enumerable.Range(1, 10000).ToArray();
    }

    //[Benchmark]
    //public int SumArray() => _array.Sum();

    //[Benchmark]
    //public int CreateRangeFast() => _ = LinqFasterSIMD.SumS(_array);

    [Benchmark]
    public void Test()
    {
        var value = 666;

        var memory = owner.Memory;

        WriteInt32ToBuffer(value, memory);

        DisplayBufferToConsole(owner.Memory.Slice(0, value.ToString().Length));
    }

    [Benchmark]
    public void NormalTest()
    {
        var value = 666;
        Console.WriteLine(value.ToString());
    }

    static void WriteInt32ToBuffer(int value, Memory<char> buffer)
    {
        var strValue = value.ToString();

        var span = buffer.Span;
        for (int ctr = 0; ctr < strValue.Length; ctr++)
            span[ctr] = strValue[ctr];
    }

    static void DisplayBufferToConsole(Memory<char> buffer) =>
        Console.WriteLine($"Contents of the buffer: '{buffer}'");
}
