namespace Graphoscope.PerformanceComparison


open QuikGraph
open QuikGraph.Algorithms.Search


module AdjacencyGraph =

    let computeBFS predicate root (adjGraph : AdjacencyGraph<_,_>) =
        let bfs = BreadthFirstSearchAlgorithm<_,_>(adjGraph)
        let handler = VertexAction<_>(predicate)
        bfs.add_DiscoverVertex(handler)
        bfs.Compute(root)

    let computeDFS predicate root (adjGraph : AdjacencyGraph<_,_>) =
        let dfs = DepthFirstSearchAlgorithm<_,_>(adjGraph)
        let handler = VertexAction<_>(predicate)
        dfs.add_DiscoverVertex(handler)
        dfs.Compute(root)