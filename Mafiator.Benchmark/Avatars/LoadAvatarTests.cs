using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using Mafiator.Common.Data.Dtos.Avatars;
using Mafiator.Data;
using Mafiator.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace Mafiator.Benchmark.Avatars;

[SimpleJob(RunStrategy.ColdStart, launchCount: 20)]
public class LoadAvatarTests
{
    private List<AvatarResult> MockAvatarData;
    private IUnitOfWork _unitOfWork;

    [GlobalSetup]
    public void Setup()
    {
        MockAvatarData = new List<AvatarResult>()
        {
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
        _unitOfWork = ServiceProviderFactory.BuildServiceProvider().GetRequiredService<IUnitOfWork>();
    }

    //[Benchmark]
    public async Task GetAllAvatarsFast()
    {
        await _unitOfWork.Avatar.GetAllDtosFast();
    }

    //[Benchmark]
    public async Task GetAllAvatars()
    {
        await _unitOfWork.Avatar.GetAllDtos();
    }

    [Benchmark]
    public void IterateAvatars()
    {
        foreach (var avatar in MockAvatarData)
        {
            avatar.Name.Insert(0, Constants.BlobStorageEndpoint);
        }
    }

    [Benchmark]
    public void IterateAvatarsFast()
    {
        foreach (var avatar in MockAvatarData)
        {
            avatar.Name = string.Join(Constants.BlobStorageEndpoint,avatar.Name);
        }
    }
}