﻿namespace Yard.Generators.GLL.ParserCommon

open AbstractAnalysis.Common
open Yard.Generators.Common.ASTGLL
open FSharpx.Collections.Experimental
open System.Collections.Generic

[<Measure>] type vertexMeasure
[<Measure>] type nodeMeasure
[<Measure>] type labelMeasure
[<Measure>] type positionInInput
[<Measure>] type state
[<Measure>] type length

module CommonFuns = 

    let inline pack left right : int64 =  ((int64 left <<< 32) ||| int64 right)
    let inline pack3 l m r : int64 =  ((int64 l <<< 42) ||| (int64 m <<< 21) ||| int64 r)
    let inline getRight (long : int64) = int <| ((int64 long) &&& 0xffffffffL)
    let inline getLeft (long : int64)  = int <| ((int64 long) >>> 32)

    let inline packVertex level label: int64<vertexMeasure>  =  LanguagePrimitives.Int64WithMeasure ((int64 level <<< 32) ||| int64 label)
    let inline getIndex1Vertex (long : int64<vertexMeasure>) = int <| ((int64 long) &&& 0xffffffffL)
    let inline getIndex2Vertex (long : int64<vertexMeasure>) = int <| ((int64 long) >>> 32)

    let inline packVertexFSA position state: int64<vertexMeasure>  =  LanguagePrimitives.Int64WithMeasure ((int64 position <<< 32) ||| int64 state)
    let inline getPosition (packed : int64<vertexMeasure>) = int <| ((int64 packed) &&& 0xffffffffL)
    let inline getState (packed : int64<vertexMeasure>) = int <| ((int64 packed) >>> 32)

    let inline packEdgePos edge position : int<positionInInput>  = LanguagePrimitives.Int32WithMeasure((int position <<< 16) ||| int edge)
    let inline getEdge (packedValue : int<positionInInput>)      = int (int packedValue &&& 0xffff)
    let inline getPosOnEdge (packedValue : int<positionInInput>) = int packedValue >>> 16 

    let inline packLabelNew rule position : int<labelMeasure>   = LanguagePrimitives.Int32WithMeasure((int rule <<< 16) ||| int position)                               
    let inline getRuleNew (packedValue : int<labelMeasure>)     = int packedValue >>> 16
    let inline getPositionNew (packedValue : int<labelMeasure>) = int (int packedValue &&& 0xffff)

[<Struct>]
[<System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 1)>]
type Vertex =
    /// Position in input graph (Packed edge+position)
    val Level            : int<positionInInput>
    /// Nonterminal
    val NontermLabel     : int<labelMeasure>
    new (level, nonterm) = {Level = level; NontermLabel = nonterm}

type GSSVertexNFA =
    /// Position in input graph (Packed edge+position)
    val PositionInInput  : int<positionInInput>
    /// Nonterminal
    val NontermState     : int<state>
    new (positionInInput, nonterm) = {PositionInInput = positionInInput; NontermState = nonterm}

[<Struct>]
type Context(*<'TokenType>*) =
    val Index         : int
    val Label         : int<labelMeasure>
    val Vertex        : Vertex
    val Ast           : int<nodeMeasure>
    val Probability   : float
    val SLength       : int   
    //val Path          : List<ParserEdge<'TokenType*ref<bool>>>
    new (index, label, vertex, ast, prob, sLength) = {Index = index; Label = label; Vertex = vertex; Ast = ast; Probability = prob; SLength = sLength} // Path = List.empty<ParserEdge<'TokenType*ref<bool>>>
    new (index, label, vertex, ast) = {Index = index; Label = label; Vertex = vertex; Ast = ast; Probability = 1.0; SLength = 1}
    //new (index, label, vertex, ast, path) = {Index = index; Label = label; Vertex = vertex; Ast = ast; Path = path}

[<Struct>]
[<System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 1)>]
type Context2 =
    /// Position in input graph (packed edge+position).
    val Index         : int<positionInInput>
    /// Current rule and position in it (packed rule+position).
    val Label         : int<labelMeasure>
    /// Current GSS node.
    val Vertex        : Vertex
    /// 4 values packed in one int64: leftEdge, leftPos, rightEdge, rightPos.
    val Extension     : int64<extension>
    /// Length of current result
    val Length        : uint16
    new (index, label, vertex, ext, len) = {Index = index; Label = label; Vertex = vertex; Extension = ext; Length = len}
    override this.ToString () = "Edge:" + (CommonFuns.getEdge(this.Index).ToString()) +
                                "; PosOnEdge:" + (CommonFuns.getPosOnEdge(this.Index).ToString()) +
                                "; Rule:" + (CommonFuns.getRuleNew(this.Label).ToString()) +
                                "; PosInRule:" + (CommonFuns.getPositionNew(this.Label).ToString()) +
                                "; Ext:" + (this.Extension.ToString()) +
                                "; Len:" + (this.Length.ToString())

[<Struct>]
[<System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 1)>]
type ContextFSA =
    /// Position in input graph (packed edge+position).
    val Index         : int<positionInInput>
    /// Current state of FSA.
    val State         : int<state>
    /// Current GSS node.
    val Vertex        : GSSVertexNFA
    /// 4 values packed in one int64: leftEdge, leftPos, rightEdge, rightPos.
    val Extension     : int64<extension>
    /// Length of current result
    val Length        : uint16
    new (index, state, vertex, ext, len) = {Index = index; State = state; Vertex = vertex; Extension = ext; Length = len}
    override this.ToString () = "Edge:" + (CommonFuns.getEdge(this.Index).ToString()) +
                                "; PosOnEdge:" + (CommonFuns.getPosOnEdge(this.Index).ToString()) +
                                "; State:" + (this.State.ToString()) +
                                "; Ext:" + (this.Extension.ToString()) +
                                "; Len:" + (this.Length.ToString())

type ParseResult<'a> =
    | Success of Tree
    | Success1 of 'a[]
    | Error of string

type CompressedArray<'t>(l : int[], f : _ -> 't, shift) =
    let a = Array.init l.Length (fun i -> Array.init (l.[i]+1) f)
    member this.Item         
        with get (i:int<positionInInput>) = 
            let edg = (CommonFuns.getEdge i)
            let pos = (CommonFuns.getPosOnEdge i)
            a.[edg].[shift + pos]
        and set i v = a.[(CommonFuns.getEdge i)].[shift + (CommonFuns.getPosOnEdge i)] <- v

      
type ParserStructures<'TokenType> (currentRule : int)=
    let sppfNodes = new BlockResizeArray<INode>()
    let dummyAST = new TerminalNode(-1, packExtension -1 -1)
    let setP = new Dictionary<int64, Yard.Generators.Common.DataStructures.ResizableUsualOne<int<nodeMeasure>>>(500)//list<int<nodeMeasure>>> (500)
    let epsilonNode = new TerminalNode(-1, packExtension 0 0)
    let setR = new System.Collections.Generic. Queue<Context>(100)  
    let dummy = 0<nodeMeasure>
    let currentN = ref <| dummy
    let currentR = ref <| dummy
    let resultAST = ref None
    do 
        sppfNodes.Add dummyAST
        sppfNodes.Add epsilonNode

    let currentLabel = ref <| (CommonFuns.packLabelNew currentRule 0)
    
    let getTreeExtension (node : int<nodeMeasure>) =
        match sppfNodes.Item (int node) with
        | :? TerminalNode as t ->
            t.Extension
        | :? IntermidiateNode as i ->
            i.Extension
        | :? NonTerminalNode as n ->
            n.Extension
        | _ -> failwith "Bad type for tree node"   

    let getNodeP 
        findSppfNode 
        (findSppfPackedNode : _ -> _ -> _ -> _ -> INode -> INode -> int<nodeMeasure>) 
        dummy (label : int<labelMeasure>) (left : int<nodeMeasure>) (right : int<nodeMeasure>) : int<nodeMeasure> =
            let currentRight = sppfNodes.Item (int right)  
            let rightExt = getTreeExtension right           
            if left <> dummy
            then
                let currentLeft = sppfNodes.Item (int left)
                let leftExt = getTreeExtension left
                let y = findSppfNode label (getLeftExtension leftExt) (getRightExtension rightExt)
                ignore <| findSppfPackedNode y label leftExt rightExt currentLeft currentRight
                y
            else
                let y = findSppfNode label (getLeftExtension rightExt) (getRightExtension rightExt)
                ignore <| findSppfPackedNode y label rightExt rightExt dummyAST currentRight 
                y
      //CompressedArray<Dictionary<_, Dictionary<_, ResizeArray<_>>>>                           
    let containsContext (setU : Dictionary<_, Dictionary<_, ResizeArray<_>>>[]) inputIndex (label : int<labelMeasure>) (vertex : Vertex) (ast : int<nodeMeasure>) =
        let vertexKey = CommonFuns.pack vertex.Level vertex.NontermLabel
        if setU.[inputIndex] <> Unchecked.defaultof<_>
        then
            let cond, current = setU.[inputIndex].TryGetValue(int label) 
            if  cond
            then
                if current.ContainsKey vertexKey
                then
                    let trees = current.[vertexKey]
                    if not <| trees.Contains ast
                    then 
                        trees.Add ast
                        false
                    else
                        true
                else 
                    let arr = new ResizeArray<int<nodeMeasure>>()
                    arr.Add ast
                    current.Add(vertexKey, arr)                    
                    false
            else 
                let dict = new Dictionary<_, ResizeArray<_>>()
                setU.[inputIndex].Add(int label, dict)
                let arr = new ResizeArray<int<nodeMeasure>>()
                arr.Add ast
                dict.Add(vertexKey, arr) 
                false
        else 
            let dict1 = new Dictionary<_, _>()
            setU.[inputIndex] <- dict1
            let dict2 = new Dictionary<_, ResizeArray<_>>()
            dict1.Add(int label, dict2)
            let arr = new ResizeArray<int<nodeMeasure>>()
            arr.Add ast
            dict2.Add(vertexKey, arr)
            false
        //else true
//CompressedArray<System.Collections.Generic.Dictionary<_, System.Collections.Generic.Dictionary<_, ResizeArray<_>>>>
    let addContext (setU ) (inputVertex : int) (label : int<labelMeasure>) vertex ast =
        if not <| containsContext setU inputVertex label vertex ast
        then
            setR.Enqueue(new Context(inputVertex, label, vertex, ast (*, currentPath*)))

    let containsEdge (dict1 : Dictionary<_, Dictionary<_, ResizeArray<_>>>) ast (e : Vertex) =
        if dict1 <> Unchecked.defaultof<_>
            then
                if dict1.ContainsKey(ast)
                then
                    let dict2 = dict1.[ast]
                    if dict2.ContainsKey(e.NontermLabel)
                    then
                        let t = dict2.[e.NontermLabel]
                        if t.Contains(e.Level) 
                        then true, None
                        else 
                            t.Add(e.Level) 
                            false, None 
                    else
                        let arr = new ResizeArray<_>()
                        arr.Add(e.Level) 
                        dict2.Add(e.NontermLabel, arr)
                        false, None
                else
                    let d = new Dictionary<_, ResizeArray<_>>()
                    dict1.Add(ast, d)
                    let l = new ResizeArray<_>()
                    l.Add(e.Level)
                    d.Add(e.NontermLabel, l)
                    false, None
            else
                let newDict1 = new Dictionary<int<nodeMeasure>, Dictionary<_, ResizeArray<_>>>()
                let newDict2 = new Dictionary<_, ResizeArray<_>>()
                let newArr = new ResizeArray<_>()
                newArr.Add(e.Level)
                newDict2.Add(e.NontermLabel, newArr)
                newDict1.Add(ast, newDict2)
                false, Some newDict1   

    let finalMatching (curRight : INode) nontermName finalExtensions findSppfNode findSppfPackedNode currentGSSNode currentVertexInInput (pop : Vertex -> int -> int<nodeMeasure> -> unit)  = 
        match curRight with
        | :? TerminalNode as t ->
            currentN := getNodeP findSppfNode findSppfPackedNode dummy !currentLabel !currentR !currentN
            let r = (sppfNodes.Item (int !currentN)) :?> NonTerminalNode 
            pop !currentGSSNode !currentVertexInInput !currentN
        | :? NonTerminalNode as r ->
            if (r.Name = nontermName) && (Array.exists ((=) r.Extension) finalExtensions)
            then 
                match !resultAST with
                | None ->  resultAST := Some r
                | Some a -> a.AddChild r.First
                         
            pop !currentGSSNode !currentVertexInInput !currentN
        | x -> failwithf "Unexpected node type in ASTGLL: %s" <| x.GetType().ToString()
          
                 
             
    member this.GetNodeP = getNodeP
    member this.SetP = setP
    member this.EpsilonNode = epsilonNode
    member this.SetR = setR
    member this.SppfNodes = sppfNodes
    member this.DummyAST = dummyAST
    member this.AddContext = addContext
    member this.ContainsEdge = containsEdge
    member this.GetTreeExtension = getTreeExtension
    member this.Dummy = dummy
    member this.CurrentN = currentN
    member this.CurrentR = currentR 
    member this.ResultAST = resultAST
    member this.CurrentLabel = currentLabel
    member this.FinalMatching = finalMatching