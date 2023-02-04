using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using Mafiator.Common.Data.Dtos.Avatars;
using Mafiator.Data;
using System.Runtime.InteropServices;

namespace Mafiator.Benchmark;

[SimpleJob(RunStrategy.ColdStart, launchCount: 10)]
[MemoryDiagnoser(false)]
public class MarshalCollectionBenchmark
{
    private List<AvatarResult> avatars;

    [GlobalSetup]
    public void Setup()
    {
        avatars = new List<AvatarResult>(){
            new AvatarResult { Name = "Avatar test data" },
            new AvatarResult { Name = "Avatar test data" },
            new AvatarResult { Name = "Avatar test data" },
            new AvatarResult { Name = "Avatar test data" },
            new AvatarResult { Name = "Avatar test data" },
            new AvatarResult { Name = "Avatar test data" },
            new AvatarResult { Name = "Avatar test data" },
            new AvatarResult { Name = "Avatar test data" },
            new AvatarResult { Name = "Avatar test data" },
            new AvatarResult { Name = "Avatar test data" },
        };
    }

    [Benchmark]
    public void IterateList()
    {
        foreach (var avatar in avatars)
        {
            //avatar.Name = string.Join(Constants.BlobStorageEndpoint, avatar.Name);
        }
    }

    [Benchmark]
    public void IterateListWithMarshal()
    {
        foreach (var avatar in CollectionsMarshal.AsSpan(avatars))
        {
            //avatar.Name = string.Join(Constants.BlobStorageEndpoint, avatar.Name);
        }
    }
}