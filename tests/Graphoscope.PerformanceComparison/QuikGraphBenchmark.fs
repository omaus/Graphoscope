namespace Graphoscope.PerformanceComparison


open BenchmarkDotNet.Attributes
open Graphoscope.PerformanceComparison.Fixtures.QuikGraph


[<MemoryDiagnoser>]
type QuikGraphBenchmark() =

    [<Benchmark>]
    member _.agn20e27DFS1000Times() =
        for i = 1 to 1000 do
            AdjacencyGraph.computeDFS (fun _ -> ()) 1 AdjGraphs.n20e27

    [<Benchmark>]
    member _.agn200e270DFS1000Times() =
        for i = 1 to 1000 do
            AdjacencyGraph.computeDFS (fun _ -> ()) 1 AdjGraphs.n200e270

    //[<Benchmark>]
    //member _.agn2000e2700DFS1000Times() =
    //    for i = 1 to 1000 do
    //        AdjacencyGraph.computeDFS (fun _ -> ()) 1 AdjGraphs.n2000e2700


    [<Benchmark>]
    member _.agn20e27BFS1000Times() =
        for i = 1 to 1000 do
            AdjacencyGraph.computeBFS (fun _ -> ()) 1 AdjGraphs.n20e27

    [<Benchmark>]
    member _.agn200e270BFS1000Times() =
        for i = 1 to 1000 do
            AdjacencyGraph.computeBFS (fun _ -> ()) 1 AdjGraphs.n200e270

    //[<Benchmark>]
    //member _.agn2000e2700BFS1000Times() =
    //    for i = 1 to 1000 do
    //        AdjacencyGraph.computeBFS (fun _ -> ()) 1 AdjGraphs.n2000e2700