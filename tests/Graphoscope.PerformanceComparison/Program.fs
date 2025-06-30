open BenchmarkDotNet.Running
open Graphoscope.PerformanceComparison


[<EntryPoint>]
let main _ =
    BenchmarkRunner.Run<GraphoscopeBenchmark>() |> ignore
    BenchmarkRunner.Run<QuikGraphBenchmark>() |> ignore
    0