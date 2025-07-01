namespace Graphoscope.PerformanceComparison


open BenchmarkDotNet.Attributes
open Graphoscope.PerformanceComparison.Fixtures.QuikGraph


[<MemoryDiagnoser>]
type QuikGraphBenchmark() =

    [<Benchmark>]
    member _.agn20e27DFS10000Times() =
        for i = 1 to 10000 do
            AdjacencyGraph.computeDFS (fun _ -> ()) 1 AdjGraphs.n20e27

    [<Benchmark>]
    member _.agn200e270DFS10000Times() =
        for i = 1 to 10000 do
            AdjacencyGraph.computeDFS (fun _ -> ()) 1 AdjGraphs.n200e270

    [<Benchmark>]
    member _.agn2000e2700DFS10000Times() =
        for i = 1 to 10000 do
            AdjacencyGraph.computeDFS (fun _ -> ()) 1 AdjGraphs.n2000e2700


    [<Benchmark>]
    member _.agn20e27BFS10000Times() =
        for i = 1 to 10000 do
            AdjacencyGraph.computeBFS (fun _ -> ()) 1 AdjGraphs.n20e27

    [<Benchmark>]
    member _.agn200e270BFS10000Times() =
        for i = 1 to 10000 do
            AdjacencyGraph.computeBFS (fun _ -> ()) 1 AdjGraphs.n200e270

    [<Benchmark>]
    member _.agn2000e2700BFS10000Times() =
        for i = 1 to 10000 do
            AdjacencyGraph.computeBFS (fun _ -> ()) 1 AdjGraphs.n2000e2700