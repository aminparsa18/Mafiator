using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Validators;
using Mafiator.Benchmark;
using Mafiator.Common.Data.Dtos.Avatars;
using MessagePack;
using RepoDb;
using System.Diagnostics;
using System.Text.Json;

var _mockAvatarData = new List<AvatarResult>()
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
var sw = new Stopwatch();
sw.Start();
JsonSerializer.Serialize(_mockAvatarData);
var dd = sw.ElapsedMilliseconds;
MessagePackSerializer.Serialize(_mockAvatarData);
var gg = sw.ElapsedMilliseconds;
SqlServerBootstrap.Initialize();
ServiceProviderFactory.BuildServiceProvider();

var config = new ManualConfig()
      .WithOptions(ConfigOptions.DisableOptimizationsValidator)
      .AddValidator(JitOptimizationsValidator.DontFailOnError)
      .AddLogger(ConsoleLogger.Default)
      .AddColumnProvider(DefaultColumnProviders.Instance);

BenchmarkRunner.Run<SerializationBenchmark>(config);