﻿namespace Graphoscope.Metrics

open Graphoscope.ArrayAdjacencyGraph
open FSharpx.Collections
open System
open System.Collections.Generic



module ofArrayAdjacencyGraph = 
//Much of the logic is taken from the measure formulas detailed in the cytoscape documentation here 
//https://med.bioinf.mpi-inf.mpg.de/netanalyzer/help/2.7/index.html#figure7

    let private infinity = Double.PositiveInfinity 

    // dijkstra implementation is based on and extended from the psuedo code here https://en.wikipedia.org/wiki/Dijkstra%27s_algorithm#Pseudocode
    // Didnt rewrite with recursion and immutability for performance reasons and to keep it close to the psuedo code 
    let private dijkstra(graph: ArrayAdjacencyGraph<'Vertex,'Label,'Edge>) 
                        (edgeFinder: 'Vertex -> 'Vertex array) // used to filter directed/undirected edges
                        (weightCalc: 'Vertex * 'Vertex -> float ) // can be controled for unweighted paths or domain specific use cases.
                        (source: 'Vertex) = 

        let q = new ResizeArray<'Vertex>()
        let dist = new Dictionary<'Vertex, float>()

        graph.GetVertices()
        |> Array.iter(fun v ->   
            q.Add v
            if v <> source then 
                dist.Add(v, infinity) 
            else dist.Add(v, 0.0) )
    
        while q.Count > 0 do
            dist
            |> Seq.filter(fun (KeyValue(v,d)) -> q.Contains v)
            |> Seq.minBy(fun (KeyValue(_ ,d)) -> d)
            |> fun (KeyValue(v,_)) -> 
                q.Remove v |> ignore
                edgeFinder v
                |> Array.iter(fun n -> 
                    let alt = dist.[v] + (weightCalc (v, n))
                    if alt < dist.[n] then dist.[n] <- alt; 
                    ) 
        dist
        |> Seq.map(fun (KeyValue(v,d)) -> v, if d = infinity then None else Some d)
        |> Map 

    // all the weighted functions require Edge to be a float
    let private getWeight (graph: ArrayAdjacencyGraph<'Vertex,'Label,float>) (v, n) = 
        match graph.TryGetWeight(v, n) with
                | Some w -> w
                | None -> infinity 

    let private meanShortestPathBase (vertices: 'Vertex array) (fn: 'Vertex -> Map<'Vertex,option<float>> ) = 
        vertices
        |> Seq.map(fun v ->  
            fn v
            |> Map.toSeq
            |> Seq.choose(fun (_,v) -> v)
        )
        |> Seq.concat
        |> Seq.filter (fun v -> v > 0.0)
        |> Seq.average

    /// Returns a Some of the undirected shortest path from source to target vertices, else None.         
    let tryGetShortestPath  (source: 'Vertex) (target: 'Vertex) (graph: ArrayAdjacencyGraph<'Vertex,'Label, 'Edge>)= 
        (dijkstra graph graph.Neighbours (fun (n, v) -> 1.0) source).[target]

    /// Returns a Some of the outward directed shortest path from source to target vertices, else None.       
    let tryGetShortestPathDirected (source: 'Vertex) (target: 'Vertex) (graph: ArrayAdjacencyGraph<'Vertex,'Label,'Edge>) = 
        (dijkstra graph graph.Successors (fun (n, v) -> 1.0) source).[target]
    
    /// Returns a Some of the sum of edge weights along the outward directed shortest path from source to target vertices, else None.    
    let tryGetShortestPathDirectedhWeighted  (source: 'Vertex) (target: 'Vertex) (graph: ArrayAdjacencyGraph<'Vertex,'Label,float>) = 
        (dijkstra graph graph.Successors (getWeight graph) source).[target]

    /// Returns the average of all the undirected shortest paths between connected vertices in the graph.
    let meanShortestUnDirected (graph: ArrayAdjacencyGraph<'Vertex,'Label,'Edge>) =
        meanShortestPathBase (graph.GetVertices()) (dijkstra graph graph.Neighbours (fun (_, _) -> 1.0))
    
    /// Returns the average of all the directed shortest paths between connected vertices in the graph.
    let meanShortestPathDirected (graph: ArrayAdjacencyGraph<'Vertex,'Label,'Edge>) =
        meanShortestPathBase (graph.GetVertices()) (dijkstra graph graph.Successors (fun (_, _) -> 1.0))

    /// Returns the average of all the summed weights on directed edges on shortest paths between connected vertices in the graph.
    let meanShortestPathDirectedhWeighted (graph: ArrayAdjacencyGraph<'Vertex,'Label,float>) =
        meanShortestPathBase (graph.GetVertices()) (dijkstra graph graph.Successors (getWeight graph))
   
    let private meanShortestPathVertexBase (paths: Map<'Vertex,option<float>>) =
        paths
        |> Map.toSeq
        |> Seq.choose(fun (_,v) -> v)
        |> Seq.filter (fun v -> v > 0.0)
        |> Seq.average

    //Averages Shortest Paths

    /// Returns the average of all the shortest paths from the source vertex to the connected vertices.
    let meanShortestPathUnDirectedVertex (source: 'Vertex) (graph: ArrayAdjacencyGraph<'Vertex,'Label,'Edge>) =
        (dijkstra graph graph.Neighbours (fun (_, _) -> 1.0) source)
        |> meanShortestPathVertexBase

    /// Returns the average of all the outward directed shortest paths from the source vertex to the connected vertices.
    let meanShortestPathDirectedVertex  (source: 'Vertex) (graph: ArrayAdjacencyGraph<'Vertex,'Label,'Edge>)=
        (dijkstra graph graph.Successors (fun (_, _) -> 1.0) source)
        |> meanShortestPathVertexBase

     /// Returns the average of all the summed weights on outward directed edges on shortest paths from the source vertex to the connected vertices.
    let meanShortestPathDirectedhWeightedVertex  (source: 'Vertex) (graph: ArrayAdjacencyGraph<'Vertex,'Label,float>) =
        (dijkstra graph graph.Successors (getWeight graph) source)
        |> meanShortestPathVertexBase
      
    // Closeness
    let private getClosenessBase (graph: ArrayAdjacencyGraph<'Vertex,'Label,'Edge>) 
                    (source: 'Vertex) 
                    (edgeFinder: 'Vertex -> 'Vertex array) = 
        dijkstra graph edgeFinder (fun (_, _) -> 1.0) source
        |> Map.toSeq
        |> Seq.choose(fun (k,v) -> v)
        |> Seq.filter (fun v -> v > 0.0)
        |> fun v -> 
            1.0 / (v |> Seq.average) 
    
    /// Returns closeness centrality of the source vertex.
    let getClosenessUnDirected  (source: 'Vertex) (graph: ArrayAdjacencyGraph<'Vertex,'Label,'Edge>)=
        getClosenessBase graph source (graph.Neighbours)

    /// Returns outward directed closeness centrality of the source vertex.
    let getClosenessOutward  (source: 'Vertex) (graph: ArrayAdjacencyGraph<'Vertex,'Label,'Edge>)=
        getClosenessBase graph source (graph.Successors)

    /// Returns inward directed closeness centrality of the source vertex.
    let getClosenessInward  (source: 'Vertex) (graph: ArrayAdjacencyGraph<'Vertex,'Label,'Edge>) =
        getClosenessBase graph source (graph.Predecessors)

    /// Returns Neighborhood Connectivity as defined in cytoscape documentation for source vertex.
    let getNeighborhoodConnectivity(source: 'Vertex) (graph: ArrayAdjacencyGraph<'Vertex,'Label,'Edge>) =
        graph.Neighbours source
        |> Seq.map(fun v -> graph.Degree v |> float)
        |> Seq.average

    // Clustering Coeffcient 
    let rec private combinations acc size set = seq {
        match size, set with 
        | n, x::xs -> 
            if n > 0 then yield! combinations (x::acc) (n - 1) xs
            if n >= 0 then yield! combinations acc n xs 
        | 0, [] -> yield acc 
        | _, [] -> () }

    /// Returns Clustering Coeffcient as defined in cytoscape documentation for source vertex.
    let getClusteringCoefficient (source: 'Vertex) (graph: ArrayAdjacencyGraph<'Vertex,'Label,'Edge>) =
        graph.Neighbours source
        |> Array.toList
        |> combinations [] 2
                |> Seq.map(fun l -> (l|> List.head), (l |> List.last))
        |> Seq.map(fun (v1, v2) -> 
            if graph.TryGetUndirectedEdge(v1, v2).IsSome then 1.0 else 0.0 
            )
        |> fun s ->  (s |> Seq.sum) /(s |> Seq.length |> float)

    let private depthFirstSearch (source: 'Vertex)  (getNeightbours : 'Vertex -> 'Vertex array) (graph: ArrayAdjacencyGraph<'Vertex,'Label,'Edge>) =
        let vertices = new Dictionary<'Vertex, bool>()
        graph.GetVertices() |> Array.map(fun v -> vertices.Add(v, false)) |> ignore
        let rec dfs (graph: ArrayAdjacencyGraph<'Vertex,'Label,'Edge>) (v:'Vertex)  =
            vertices[v] <- true
            getNeightbours v 
            |> Array.filter(fun w -> not vertices[w])
            |> Array.map(fun x -> dfs graph x ) 
            |> ignore     
        dfs graph source
        vertices
            
    let isStronglyConnected (graph: ArrayAdjacencyGraph<'Vertex,'Label,'Edge>) : bool =
        let firstPass = 
            (depthFirstSearch (graph.GetVertices()[0])  (graph.Successors) graph
            |> Seq.exists(fun (KeyValue(v,b)) -> not b))
            |> not
        let reverseGraphPass =
            (depthFirstSearch (graph.GetVertices()[0])  (graph.Predecessors) graph
            |> Seq.exists(fun (KeyValue(v,b)) -> not b))
            |> not
        firstPass && reverseGraphPass

