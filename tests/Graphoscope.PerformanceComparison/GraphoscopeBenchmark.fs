namespace Graphoscope.PerformanceComparison


open BenchmarkDotNet.Attributes
open Graphoscope
open Graphoscope.Algorithms
open Graphoscope.PerformanceComparison.Fixtures.Graphoscope


[<MemoryDiagnoser>]
type GraphoscopeBenchmark() =

    [<Benchmark>]
    member _.fgn20e27DFS10000Times() =
        let nk1 = FGraph.getNodes FGraphs.n20e27 |> Seq.head |> fst
        for i = 1 to 10000 do
            DFS.ofFGraph nk1 FGraphs.n20e27 |> ignore

    [<Benchmark>]
    member _.fgn200e270DFS10000Times() =
        let nk1 = FGraph.getNodes FGraphs.n200e270 |> Seq.head |> fst
        for i = 1 to 10000 do
            DFS.ofFGraph nk1 FGraphs.n200e270 |> ignore

    [<Benchmark>]
    member _.fgn2000e2700DFS10000Times() =
        let nk1 = FGraph.getNodes FGraphs.n2000e2700 |> Seq.head |> fst
        for i = 1 to 10000 do
            DFS.ofFGraph nk1 FGraphs.n2000e2700 |> ignore


    [<Benchmark>]
    member _.fgn20e27BFS10000Times() =
        let nk1 = FGraph.getNodes FGraphs.n20e27 |> Seq.head |> fst
        for i = 1 to 10000 do
            BFS.ofFGraph nk1 FGraphs.n20e27 |> ignore

    [<Benchmark>]
    member _.fgn200e270BFS10000Times() =
        let nk1 = FGraph.getNodes FGraphs.n200e270 |> Seq.head |> fst
        for i = 1 to 10000 do
            BFS.ofFGraph nk1 FGraphs.n200e270 |> ignore

    [<Benchmark>]
    member _.fgn2000e2700BFS10000Times() =
        let nk1 = FGraph.getNodes FGraphs.n2000e2700 |> Seq.head |> fst
        for i = 1 to 10000 do
            BFS.ofFGraph nk1 FGraphs.n2000e2700 |> ignore



    [<Benchmark>]
    member _.dgn20e27DFS10000Times() =
        let nk1 = DiGraph.getNodes DiGraphs.n20e27 |> Seq.head
        for i = 1 to 10000 do
            DFS.ofDiGraph nk1 DiGraphs.n20e27 |> ignore

    [<Benchmark>]
    member _.dgn200e270DFS10000Times() =
        let nk1 = DiGraph.getNodes DiGraphs.n200e270 |> Seq.head
        for i = 1 to 10000 do
            DFS.ofDiGraph nk1 DiGraphs.n200e270 |> ignore

    [<Benchmark>]
    member _.dgn2000e2700DFS10000Times() =
        let nk1 = DiGraph.getNodes DiGraphs.n2000e2700 |> Seq.head
        for i = 1 to 10000 do
            DFS.ofDiGraph nk1 DiGraphs.n2000e2700 |> ignore