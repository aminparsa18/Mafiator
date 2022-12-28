using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Validators;
using Mafiator.Benchmark;

ServiceProviderFactory.BuildServiceProvider();

var config = new ManualConfig()
      .WithOptions(ConfigOptions.DisableOptimizationsValidator)
      .AddValidator(JitOptimizationsValidator.DontFailOnError)
      .AddLogger(ConsoleLogger.Default)
      .AddColumnProvider(DefaultColumnProviders.Instance);

BenchmarkRunner.Run<SerializationBenchmark>(config);