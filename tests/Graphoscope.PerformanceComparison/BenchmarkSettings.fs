namespace Graphoscope.PerformanceComparison


open BenchmarkDotNet.Configs
open BenchmarkDotNet.Running
open BenchmarkDotNet.Jobs
open System


module BenchmarkSettings =

    let config =
        ManualConfig
            .Create(DefaultConfig.Instance)
            //.AddJob(Job.Default.WithIterationTime(TimeSpan.FromSeconds(1.0)))
            .WithOption(ConfigOptions.DisableOptimizationsValidator, true)
            //.WithMaxIterationCount(50)
            .WithBuildTimeout(TimeSpan.FromMinutes(30.0))