open BenchmarkDotNet.Running
open Graphoscope.PerformanceComparison


[<EntryPoint>]
let main _ =
    BenchmarkRunner.Run<GraphoscopeBenchmark>(BenchmarkSettings.config) |> ignore
    BenchmarkRunner.Run<QuikGraphBenchmark>(BenchmarkSettings.config) |> ignore
    0