﻿//   Copyright 2013, 2014 YaccConstructor Software Foundation
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.


module GLLAbstractParserTests

open System.IO
open QuickGraph
open NUnit.Framework
open AbstractAnalysis.Common
//open Yard.Generators.GLL.AbstractParser
open Yard.Generators.Common.ASTGLL
open Yard.Generators.GLL.ParserCommon

open System.Collections.Generic
open System.Linq
//open GLL.ParseOrder

let outputDir = ""//@"../../../src/GLL.AbstractParser.SimpleTest/"

type elt = T of char | N of char


let lbl tokenId = tokenId
let edg f t l = new ParserEdge<_>(f,t,lbl l)

let rnd = new System.Random()

//let newRandGraph (initialGraph:ParserInputGraph<_>) (tokens:array<_>) addEdges =
//    let vertices = initialGraph.VertexCount
//    for i in 0 .. addEdges - 1 do
//        let token = tokens.[rnd.Next(0,tokens.Length-1)]
//        let visited = Array.init vertices false
//        let fromV = rnd.Next(0,vertices-1)
//        let rec addEdg  =            
//            if initialGraph.OutEdges(fromV) |> Seq.exists(fun e -> e.Tag = token) |> not
//            then initialGraph.




//let test buildAbstractAst qGraph (intToString : int -> string) (fileName : string) nodesCount edgesCount termsCount ambiguityCount tokenData tokenToNum = 
//    let r = buildAbstractAst qGraph
//    printfn "%A" r
//    match r with
//        | Error str ->
//            printfn "Error"
//            Assert.Fail("")
//        | Success tree ->
//            tree.AstToDot intToString tokenToNum tokenData (outputDir + fileName)
//            let n, e, t, amb = tree.CountCounters
//            printfn "%d %d %d %d" n e t amb
//            Assert.AreEqual(nodesCount, n, "Nodes count mismatch")
//            Assert.AreEqual(edgesCount, e, "Edges count mismatch")
//            Assert.AreEqual(termsCount, t, "Terms count mismatch") 
//            Assert.AreEqual(ambiguityCount, amb, "Ambiguities count mismatch")
//            Assert.Pass()

//let isParsed parserSource input = 
//    Yard.Generators.GLL.AbstractParserWithoutTree.isParsed parserSource input
//
//let shouldBeTrue res = 
//    Assert.AreEqual(res, true, "Error.")


//let test edges tokenizer parserSource = 
//    let edg b e (tag: string) = 
//        [| new TaggedEdge<_,_>(b, e, (tokenizer tag) * 1<token>) |]
//
//    let allVs = edges |> Array.collect (fun (b,e,_) -> [|b * 1<positionInGrammar>; e * 1<positionInGrammar>|]) |> Set.ofArray |> Array.ofSeq
//
//    let g = new SimpleGraphInput<_>(allVs, id)
//
//    [|for (b,e,t) in edges -> edg b e t |]
//    |> Array.concat
//    |> g.AddVerticesAndEdgeRange
//    |> ignore
//
//    let res = Yard.Generators.GLL.AbstractParserWithoutTree.isParsed parserSource g
//
//    shouldBeTrue res
//
//        let edges = [|0, 1, "NUM";
//                      1, 2, "PLUS";
//                      2, 3, "NUM"|]
//        
//        test edges GLL.PrettySimpleCalc.stringToNumber GLL.PrettySimpleCalc.parserSource
//
//        
[<TestFixture>]
type ``GLL abstract parser tests``() =  
    member this._01_PrettySimpleCalc_SequenceInput () =
        Assert.True(true);
//        let input = 
//            new LinearInput(
//                Array.map (fun x -> GLL.PrettySimpleCalc.tokenToNumber.[x] * 1<token>)
//                    [|GLL.PrettySimpleCalc.NUM 1;
//                      GLL.PrettySimpleCalc.PLUS 2;
//                      GLL.PrettySimpleCalc.NUM 3|])
//
//        let res = isParsed GLL.PrettySimpleCalc.parserSource input
//
//        shouldBeTrue res

//    [<Test>]
//    member this._06_NotAmbigousSimpleCalc_Loop () =
//        let qGraph = new ParserInputGraph<_>([|0|] , [|4|] )
//        qGraph.AddVerticesAndEdgeRange
//            [edg 0 1 (GLL.NotAmbigousSimpleCalc.NUM  1)
//             edg 1 2 (GLL.NotAmbigousSimpleCalc.PLUS 2)
//             edg 2 3 (GLL.NotAmbigousSimpleCalc.NUM 3)
//             edg 3 4 (GLL.NotAmbigousSimpleCalc.RNGLR_EOF 4)
//             edg 3 0 (GLL.NotAmbigousSimpleCalc.PLUS 5)
//             ] |> ignore
//        
//        test GLL.NotAmbigousSimpleCalc.buildAbstractAst qGraph GLL.NotAmbigousSimpleCalc.numToString "NotAmbigousSimpleCalc2.dot" 25 30 6 1 GLL.NotAmbigousSimpleCalc.tokenData GLL.NotAmbigousSimpleCalc.tokenToNumber
//
//    [<Test>]
//    member this._07_NotAmbigousSimpleCalc_LoopInLoop () =
//        let qGraph = new ParserInputGraph<_>(0, 6)
//        qGraph.AddVerticesAndEdgeRange
//            [edg 0 1 (GLL.NotAmbigousSimpleCalc.NUM  1)
//             edg 1 2 (GLL.NotAmbigousSimpleCalc.PLUS 2)
//             edg 2 3 (GLL.NotAmbigousSimpleCalc.NUM 3)
//             edg 3 4 (GLL.NotAmbigousSimpleCalc.PLUS 4)
//             edg 4 5 (GLL.NotAmbigousSimpleCalc.NUM 5)
//             edg 5 0 (GLL.NotAmbigousSimpleCalc.PLUS 6)
//             edg 5 2 (GLL.NotAmbigousSimpleCalc.STAR 7)
//             edg 5 6 (GLL.NotAmbigousSimpleCalc.RNGLR_EOF 8)
//             ] |> ignore
//        
//        test  
//            GLL.NotAmbigousSimpleCalc.buildAbstractAst 
//            qGraph 
//            GLL.NotAmbigousSimpleCalc.numToString 
//            "NotAmbigousSimpleCalcLoopLoop.dot" 
//            39 48 9 2 
//            GLL.NotAmbigousSimpleCalc.tokenData
//            GLL.NotAmbigousSimpleCalc.tokenToNumber
//
//
//    [<Test>]
//    member this._14_NotAmbigousSimpleCalcWith2Ops_Loop () =
//        let qGraph = new ParserInputGraph<_>(0, 7)
//        qGraph.AddVerticesAndEdgeRange
//            [edg 0 1 (GLL.NotAmbigousSimpleCalcWith2Ops.NUM  1)
//             edg 1 2 (GLL.NotAmbigousSimpleCalcWith2Ops.PLUS 2)
//             edg 2 3 (GLL.NotAmbigousSimpleCalcWith2Ops.NUM 3)
//             edg 3 4 (GLL.NotAmbigousSimpleCalcWith2Ops.PLUS 4)
//             edg 4 5 (GLL.NotAmbigousSimpleCalcWith2Ops.NUM 5)
//             edg 5 2 (GLL.NotAmbigousSimpleCalcWith2Ops.MULT 6)
//             edg 4 6 (GLL.NotAmbigousSimpleCalcWith2Ops.NUM 7)
//             edg 6 7 (GLL.NotAmbigousSimpleCalcWith2Ops.RNGLR_EOF 0)
//             ] |> ignore
//        
//        test GLL.NotAmbigousSimpleCalcWith2Ops.buildAbstractAst qGraph GLL.NotAmbigousSimpleCalcWith2Ops.numToString "NotAmbigousSimpleCalcWith2Ops.dot" 0 0 0 0
//
//    [<Test>]
//    member this._15_NotAmbigousSimpleCalcWith2Ops_Loops () =
//        let qGraph = new ParserInputGraph<_>(0, 8)
//        qGraph.AddVerticesAndEdgeRange
//            [edg 0 1 (GLL.NotAmbigousSimpleCalcWith2Ops.NUM  1)
//             edg 1 2 (GLL.NotAmbigousSimpleCalcWith2Ops.PLUS 2)
//             edg 2 3 (GLL.NotAmbigousSimpleCalcWith2Ops.PLUS 3)
//             edg 2 4 (GLL.NotAmbigousSimpleCalcWith2Ops.NUM 4)
//             edg 3 4 (GLL.NotAmbigousSimpleCalcWith2Ops.NUM 5)
//             edg 4 5 (GLL.NotAmbigousSimpleCalcWith2Ops.PLUS 6)
//             edg 5 2 (GLL.NotAmbigousSimpleCalcWith2Ops.NUM 7)
//             edg 4 6 (GLL.NotAmbigousSimpleCalcWith2Ops.PLUS 8)
//             edg 6 7 (GLL.NotAmbigousSimpleCalcWith2Ops.NUM 9)
//             edg 7 8 (GLL.NotAmbigousSimpleCalcWith2Ops.RNGLR_EOF 0)
//             ] |> ignore
//        
//        test GLL.NotAmbigousSimpleCalcWith2Ops.buildAbstractAst qGraph GLL.NotAmbigousSimpleCalcWith2Ops.numToString "NotAmbigousSimpleCalcWith2Ops2.dot" 0 0 0 0

//    [<Test>]
//    member this._16_Stars_Loop () =
//        let qGraph = new ParserInputGraph<_>(0, 2)
//        qGraph.AddVerticesAndEdgeRange
//            [edg 0 0 (GLL.Stars.STAR 1)
//             edg 0 1 (GLL.Stars.SEMI 2)
//             edg 1 2 (GLL.Stars.RNGLR_EOF 3)
//             ] |> ignore
//        
//        test 
//            GLL.Stars.buildAbstractAst 
//            qGraph 
//            GLL.Stars.numToString 
//            "Stars_Loop.dot" 
//            19 24 4 1 
//            GLL.Stars.tokenData
//            GLL.Stars.tokenToNumber
//
//    [<Test>]
//    member this._17_Stars2_Loop () =
//        let qGraph = new ParserInputGraph<_>(0, 1)
//        qGraph.AddVerticesAndEdgeRange
//            [edg 0 0 (GLL.Stars2.STAR 1)
//             edg 0 1 (GLL.Stars2.RNGLR_EOF 2)
//             ] |> ignore
//        
//        test 
//            GLL.Stars2.buildAbstractAst 
//            qGraph 
//            GLL.Stars2.numToString 
//            "Stars2.dot" 
//            23 33 3 2
//            GLL.Stars2.tokenData
//            GLL.Stars2.tokenToNumber
//
//    [<Test>]
//    member this._19_FirstEps () =
//        let qGraph = new ParserInputGraph<_>(0, 3)
//        qGraph.AddVerticesAndEdgeRange
//           [edg 0 1 (GLL.FirstEps.Z 1)
//            edg 1 2 (GLL.FirstEps.N 2)
//            edg 2 3 (GLL.FirstEps.RNGLR_EOF 3)
//            ] |> ignore
//
//        test 
//            GLL.FirstEps.buildAbstractAst
//            qGraph
//            GLL.FirstEps.numToString 
//            "FirstEps.dot" 
//            26 30 6 0
//            GLL.FirstEps.tokenData
//            GLL.FirstEps.tokenToNumber
//    
//    [<Test>]
//    member this._20_CroppedBrackets () =
//        let qGraph = new ParserInputGraph<_>(0, 2)
//        qGraph.AddVerticesAndEdgeRange
//           [edg 0 0 (GLL.CroppedBrackets.LBR 1)
//            edg 0 1 (GLL.CroppedBrackets.NUM 2)
//            edg 1 1 (GLL.CroppedBrackets.RBR 3)
//            edg 1 2 (GLL.CroppedBrackets.RNGLR_EOF 0)
//            ] |> ignore
//
//        test GLL.CroppedBrackets.buildAbstractAst qGraph GLL.CroppedBrackets.numToString "CroppedBrackets.dot" 14 15 5 1 GLL.CroppedBrackets.tokenData GLL.CroppedBrackets.tokenToNumber
//
//    [<Test>]
//    member this._21_Brackets () =
//        let qGraph = new ParserInputGraph<_>(0, 2)
//        qGraph.AddVerticesAndEdgeRange
//           [edg 0 0 (GLL.Brackets.LBR 1)
//            edg 0 1 (GLL.Brackets.NUM 2)
//            edg 1 1 (GLL.Brackets.RBR 3)
//            edg 1 2 (GLL.Brackets.RNGLR_EOF 0)
//            ] |> ignore
//
//        test GLL.Brackets.buildAbstractAst qGraph GLL.Brackets.numToString "Brackets.dot" 14 15 5 1 GLL.Brackets.tokenData GLL.Brackets.tokenToNumber    
//
//    [<Test>]
//    member this._22_Brackets_BackEdge () =
//        let qGraph = new ParserInputGraph<_>(0, 2)
//        qGraph.AddVerticesAndEdgeRange
//           [edg 0 0 (GLL.Brackets.LBR 1)
//            edg 0 1 (GLL.Brackets.NUM 2)
//            edg 1 1 (GLL.Brackets.RBR 3)
//            edg 1 0 (GLL.Brackets.NUM 4)
//            edg 1 2 (GLL.Brackets.RNGLR_EOF 0)
//            ] |> ignore
//
//        test GLL.Brackets.buildAbstractAst qGraph GLL.Brackets.numToString "Brackets_backEdge.dot" 35 54 6 4 GLL.Brackets.tokenData GLL.Brackets.tokenToNumber
//
//    [<Test>]
//    member this._24_UnambiguousBrackets_Circle () =
//        let qGraph = new ParserInputGraph<_>(0, 2)
//        qGraph.AddVerticesAndEdgeRange
//           [edg 0 1 (GLL.StrangeBrackets.LBR 0)
//            edg 1 0 (GLL.StrangeBrackets.RBR 1)
//            edg 0 2 (GLL.StrangeBrackets.RNGLR_EOF 0)
//            ] |> ignore
//
//        test GLL.StrangeBrackets.buildAbstractAst qGraph GLL.StrangeBrackets.numToString "StrangeBrackets2.dot" 19 21 6 1 GLL.StrangeBrackets.tokenData GLL.StrangeBrackets.tokenToNumber
//
//    [<Test>]
//    member this._25_UnambiguousBrackets_BiggerCircle () =
//        let qGraph = new ParserInputGraph<_>(0, 4)
//        qGraph.AddVerticesAndEdgeRange
//           [edg 0 1 (GLL.StrangeBrackets.LBR 0)
//            edg 1 2 (GLL.StrangeBrackets.RBR 1)
//            edg 2 3 (GLL.StrangeBrackets.LBR 2)
//            edg 3 0 (GLL.StrangeBrackets.RBR 3)
//            edg 0 4 (GLL.StrangeBrackets.RNGLR_EOF 0)
//            ] |> ignore
//
//        test GLL.StrangeBrackets.buildAbstractAst qGraph GLL.StrangeBrackets.numToString "StrangeBrackets3.dot" 30 33 9 1 GLL.StrangeBrackets.tokenData GLL.StrangeBrackets.tokenToNumber
//
//    [<Test>]
//    member this._26_UnambiguousBrackets_Inf () =
//        let qGraph = new ParserInputGraph<_>(0, 1)
//        qGraph.AddVerticesAndEdgeRange
//           [edg 0 0 (GLL.StrangeBrackets.LBR 0)
//            edg 0 0 (GLL.StrangeBrackets.RBR 1)
//            edg 0 1 (GLL.StrangeBrackets.RNGLR_EOF 0)
//            ] |> ignore
//
//        test GLL.StrangeBrackets.buildAbstractAst qGraph GLL.StrangeBrackets.numToString "StrangeBrackets4.dot" 16 18 5 1 GLL.StrangeBrackets.tokenData GLL.StrangeBrackets.tokenToNumber
//
//    
//    [<Test>]
//    member this._29_Attrs () =
//        let qGraph = new ParserInputGraph<_>(0, 6)
//        qGraph.AddVerticesAndEdgeRange
//           [edg 0 1 (GLL.ParseAttrs.A 1)
//            edg 1 2 (GLL.ParseAttrs.A 2)
//            edg 2 3 (GLL.ParseAttrs.A 3)
//            edg 3 4 (GLL.ParseAttrs.A 4)
//            edg 4 5 (GLL.ParseAttrs.A 5)
//            edg 5 6 (GLL.ParseAttrs.RNGLR_EOF 0)
//            ] |> ignore
//
//        test GLL.ParseAttrs.buildAbstractAst qGraph GLL.ParseAttrs.numToString "Attrs.dot" 29 33 7 0 GLL.ParseAttrs.tokenData GLL.ParseAttrs.tokenToNumber
//
//    [<Test>]
//    member this._30_Condition () =
//        let qGraph = new ParserInputGraph<_>(0, 6)
//        qGraph.AddVerticesAndEdgeRange
//           [edg 0 1 (GLL.ParseCond.IF 1)
//            edg 1 2 (GLL.ParseCond.IF 2)
//            edg 2 3 (GLL.ParseCond.A 3)
//            edg 3 4 (GLL.ParseCond.ELSE 4)
//            edg 4 5 (GLL.ParseCond.A 5)
//            edg 5 6 (GLL.ParseCond.RNGLR_EOF 0)
//            ] |> ignore
//
//        test GLL.ParseCond.buildAbstractAst qGraph GLL.ParseCond.numToString "Cond.dot" 44 57 7 1 GLL.ParseCond.tokenData GLL.ParseCond.tokenToNumber
//
//    [<Test>]
//    member this._31_Counter () =
//        let qGraph = new ParserInputGraph<_>(0, 6)
//        qGraph.AddVerticesAndEdgeRange
//           [edg 0 1 (GLL.ParseCounter.A 1)
//            edg 1 2 (GLL.ParseCounter.A 2)
//            edg 2 3 (GLL.ParseCounter.A 3)
//            edg 3 4 (GLL.ParseCounter.A 4)
//            edg 4 5 (GLL.ParseCounter.A 5)
//            edg 5 6 (GLL.ParseCounter.RNGLR_EOF 0)
//            ] |> ignore
//
//        test GLL.ParseCounter.buildAbstractAst qGraph GLL.ParseCounter.numToString "Counter.dot" 21 21 7 0 GLL.ParseCounter.tokenData GLL.ParseCounter.tokenToNumber
//    
//    [<Test>]
//    member this._32_Cycle () =
//        let qGraph = new ParserInputGraph<_>(0, 3)
//        qGraph.AddVerticesAndEdgeRange
//           [edg 0 1 (GLL.ParseCycle.A 1)
//            edg 1 2 (GLL.ParseCycle.B 2)
//            edg 2 3 (GLL.ParseCycle.RNGLR_EOF 0)
//            ] |> ignore
//
//        test GLL.ParseCycle.buildAbstractAst qGraph GLL.ParseCycle.numToString "Cycle.dot" 15 18 4 1 GLL.ParseCycle.tokenData GLL.ParseCycle.tokenToNumber
//         
//    [<Test>]
//    member this._33_Epsilon2_with_eps2_yrd () =
//        let qGraph = new ParserInputGraph<_>(0, 3)
//        qGraph.AddVerticesAndEdgeRange
//           [edg 0 1 (GLL.ParseEps2.Z 1)
//            edg 1 2 (GLL.ParseEps2.N 2)
//            edg 2 3 (GLL.ParseEps2.RNGLR_EOF 0)
//            ] |> ignore
//
//        test GLL.ParseEps2.buildAbstractAst qGraph GLL.ParseEps2.numToString "Eps2.dot" 26 30 6 0 GLL.ParseEps2.tokenData GLL.ParseEps2.tokenToNumber
//
//    [<Test>]
//    member this._34_Epsilon () =
//        let qGraph = new ParserInputGraph<_>(0, 1)
//        qGraph.AddVerticesAndEdgeRange
//           [edg 0 1 (GLL.ParseEpsilon.RNGLR_EOF 0)
//            ] |> ignore
//
//        test GLL.ParseEpsilon.buildAbstractAst qGraph GLL.ParseEpsilon.numToString "Epsilon.dot" 21 24 5 0 GLL.ParseEpsilon.tokenData GLL.ParseEpsilon.tokenToNumber
//        
//    [<Test>]
//    member this._35_Expression () =
//        let qGraph = new ParserInputGraph<_>(0, 6)
//        qGraph.AddVerticesAndEdgeRange
//           [edg 0 1 (GLL.ParseExpr.N 1)
//            edg 1 2 (GLL.ParseExpr.P 2)
//            edg 2 3 (GLL.ParseExpr.N 3)
//            edg 3 4 (GLL.ParseExpr.P 4)
//            edg 4 5 (GLL.ParseExpr.N 5)
//            edg 5 6 (GLL.ParseExpr.RNGLR_EOF 0)
//            ] |> ignore
//
//        test GLL.ParseExpr.buildAbstractAst qGraph GLL.ParseExpr.numToString "Expr.dot" 36 45 7 1 GLL.ParseExpr.tokenData GLL.ParseExpr.tokenToNumber
//
//    [<Test>]
//    member this._36_First () =
//        let qGraph = new ParserInputGraph<_>(0, 6)
//        qGraph.AddVerticesAndEdgeRange
//           [edg 0 1 (GLL.ParseFirst.A 1)
//            edg 1 2 (GLL.ParseFirst.A 2)
//            edg 2 3 (GLL.ParseFirst.A 3)
//            edg 3 4 (GLL.ParseFirst.A 4)
//            edg 4 5 (GLL.ParseFirst.B 5)
//            edg 5 6 (GLL.ParseFirst.RNGLR_EOF 0)
//            ] |> ignore
//
//        test GLL.ParseFirst.buildAbstractAst qGraph GLL.ParseFirst.numToString "First.dot" 21 21 7 0 GLL.ParseFirst.tokenData GLL.ParseFirst.tokenToNumber
//
//    [<Test>]
//    member this._37_ListEps () =
//        let qGraph = new ParserInputGraph<_>(0, 3)
//        qGraph.AddVerticesAndEdgeRange
//           [edg 0 1 (GLL.ParseListEps.NUM 1)
//            edg 1 2 (GLL.ParseListEps.NUM 2)
//            edg 2 3 (GLL.ParseListEps.RNGLR_EOF 0)
//            ] |> ignore
//
//        test GLL.ParseListEps.buildAbstractAst qGraph GLL.ParseListEps.numToString "ListEps.dot" 72 93 13 3 GLL.ParseListEps.tokenData GLL.ParseListEps.tokenToNumber
//    
//    [<Test>]
//    member this._38_LolCalc () =
//        let qGraph = new ParserInputGraph<_>(0, 12)
//        qGraph.AddVerticesAndEdgeRange
//           [edg 0 1 (GLL.ParseLolCalc.A 1)
//            edg 1 2 (GLL.ParseLolCalc.MUL 2)
//            edg 2 3 (GLL.ParseLolCalc.B 3)
//            edg 3 4 (GLL.ParseLolCalc.ADD 4)
//            edg 4 5 (GLL.ParseLolCalc.A 5)
//            edg 5 6 (GLL.ParseLolCalc.MUL 6)
//            edg 6 7 (GLL.ParseLolCalc.B 7)
//            edg 7 8 (GLL.ParseLolCalc.ADD 8)
//            edg 8 9 (GLL.ParseLolCalc.B 9)
//            edg 9 10 (GLL.ParseLolCalc.MUL 10)
//            edg 10 11 (GLL.ParseLolCalc.A 11)
//            edg 11 12 (GLL.ParseLolCalc.RNGLR_EOF 0)
//            ] |> ignore
//
//        test GLL.ParseLolCalc.buildAbstractAst qGraph GLL.ParseLolCalc.numToString "LolCalc.dot" 0 0 0 0
//    
//    [<Test>]
//    member this._39_LongCycle () =
//        let qGraph = new ParserInputGraph<_>(0, 2)
//        qGraph.AddVerticesAndEdgeRange
//           [edg 0 1 (GLL.ParseLongCycle.A 1)
//            edg 1 2 (GLL.ParseLongCycle.RNGLR_EOF 0)
//            ] |> ignore
//
//        test GLL.ParseLongCycle.buildAbstractAst qGraph GLL.ParseLongCycle.numToString "LongCycle.dot" 14 18 3 1 GLL.ParseLongCycle.tokenData GLL.ParseLongCycle.tokenToNumber
//    
//    [<Test>]
//    member this._41_Longest () =
//        let qGraph = new ParserInputGraph<_>(0, 6)
//        qGraph.AddVerticesAndEdgeRange
//           [edg 0 1 (GLL.ParseLongest.A 1)
//            edg 1 2 (GLL.ParseLongest.A 2)
//            edg 2 3 (GLL.ParseLongest.A 3)
//            edg 3 4 (GLL.ParseLongest.A 4)
//            edg 4 5 (GLL.ParseLongest.A 5)
//            edg 5 6 (GLL.ParseLongest.RNGLR_EOF 0)
//            ] |> ignore
//
//        test GLL.ParseLongest.buildAbstractAst qGraph GLL.ParseLongest.numToString "Longest.dot" 91 123 14 1 GLL.ParseLongest.tokenData GLL.ParseLongest.tokenToNumber
//         
//    [<Test>]
//    member this._42_Mixed () =
//        let qGraph = new ParserInputGraph<_>(0, 5)
//        qGraph.AddVerticesAndEdgeRange
//           [edg 0 1 (GLL.ParseMixed.B 1)
//            edg 1 2 (GLL.ParseMixed.A 2)
//            edg 2 3 (GLL.ParseMixed.B 3)
//            edg 3 4 (GLL.ParseMixed.A 4)
//            edg 4 5 (GLL.ParseMixed.RNGLR_EOF 0)
//            ] |> ignore
//
//        test GLL.ParseMixed.buildAbstractAst qGraph GLL.ParseMixed.numToString "Mixed.dot" 24 27 6 0 GLL.ParseMixed.tokenData GLL.ParseMixed.tokenToNumber
//    
//    [<Test>]
//    member this._43_Omit () =
//        let qGraph = new ParserInputGraph<_>(0, 4)
//        qGraph.AddVerticesAndEdgeRange
//           [edg 0 1 (GLL.ParseOmit.A 1)
//            edg 1 2 (GLL.ParseOmit.B 2)
//            edg 2 3 (GLL.ParseOmit.A 3)
//            edg 3 4 (GLL.ParseOmit.RNGLR_EOF 0)
//            ] |> ignore 
//
//        test GLL.ParseOmit.buildAbstractAst qGraph GLL.ParseOmit.numToString "Omit.dot" 26 30 6 0 GLL.ParseOmit.tokenData GLL.ParseOmit.tokenToNumber
//    
//    [<Test>]
//    member this._44_Order () =
//        let qGraph = new ParserInputGraph<_>(0, 9)
//        qGraph.AddVerticesAndEdgeRange
//           [edg 0 1 (GLL.ParseOrder.A 1)
//            edg 1 2 (GLL.ParseOrder.A 2)
//            edg 2 3 (GLL.ParseOrder.A 3)
//            edg 3 4 (GLL.ParseOrder.A 4)
//            edg 4 5 (GLL.ParseOrder.A 5)
//            edg 5 6 (GLL.ParseOrder.A 6)
//            edg 6 7 (GLL.ParseOrder.A 7)
//            edg 7 8 (GLL.ParseOrder.A 8)
//            edg 8 9 (GLL.ParseOrder.RNGLR_EOF 0)
//            ] |> ignore
//
//        test GLL.ParseOrder.buildAbstractAst qGraph GLL.ParseOrder.numToString "Order.dot"
//


//    [<Test>]
//    member this._45_SimpleRightRecursion () =
//        let qGraph = new ParserInputGraph<_>(0, 4)
//        qGraph.AddVerticesAndEdgeRange
//            [edg 0 1 (GLL.SimpleRightRecursion.B 1)
//             edg 1 2 (GLL.SimpleRightRecursion.B 2)
//             edg 2 3 (GLL.SimpleRightRecursion.B 3)
//             edg 3 4 (GLL.SimpleRightRecursion.RNGLR_EOF 0)
//             ] |> ignore
//
//        test GLL.SimpleRightRecursion.buildAbstractAst qGraph GLL.SimpleRightRecursion.numToString "SimpleRightRecursion.dot" 15 15 5 0 GLL.SimpleRightRecursion.tokenData GLL.SimpleRightRecursion.tokenToNumber
//
//    [<Test>]
//    member this._46_BadLeftRecursion () =
//        let input = 
//            new LinearInput(
//                Array.map (fun x -> GLL.BadLeftRecursion.tokenToNumber.[x] * 1<token>)
//                    [|GLL.BadLeftRecursion.B 1;
//                      GLL.BadLeftRecursion.B 2;
//                      GLL.BadLeftRecursion.B 3|])
//
//        let res = isParsed GLL.BadLeftRecursion.parserSource input
//
//        shouldBeTrue res
//
//    [<Test>]
//    member this._47_SimpleAmb () =
//        let qGraph = new ParserInputGraph<_>(0, 4)
//        qGraph.AddVerticesAndEdgeRange
//            [edg 0 1 (GLL.SimpleAmb.A 1)
//             edg 1 2 (GLL.SimpleAmb.D 2)
//             edg 2 3 (GLL.SimpleAmb.B 3)
//             edg 3 4 (GLL.SimpleAmb.RNGLR_EOF 0)
//             ] |> ignore
//
//        test GLL.SimpleAmb.buildAbstractAst qGraph GLL.SimpleAmb.numToString "SimpleAmb.dot" 18 21 5 1 GLL.SimpleAmb.tokenData GLL.SimpleAmb.tokenToNumber
//    
//    [<Test>]
//    member this._48_SimpleRightNull () =
//        let qGraph = new ParserInputGraph<_>(0, 3)
//        qGraph.AddVerticesAndEdgeRange
//            [edg 0 1 (GLL.SimpleRightNull.A 1)
//             edg 1 2 (GLL.SimpleRightNull.A 1)
//             edg 2 3 (GLL.SimpleRightNull.RNGLR_EOF 0)
//             ] |> ignore
//
//        test GLL.SimpleRightNull.buildAbstractAst qGraph GLL.SimpleRightNull.numToString "SimpleRightNull.dot" 22 24 6 0 GLL.SimpleRightNull.tokenData GLL.SimpleRightNull.tokenToNumber
//
//    [<Test>]
//    member this._49_SimpleLeftRecursion () =
//        let qGraph = new ParserInputGraph<_>(0, 4)
//        qGraph.AddVerticesAndEdgeRange
//            [edg 0 1 (GLL.SimpleLeftRecursion.B 1)
//             edg 1 2 (GLL.SimpleLeftRecursion.B 2)
//             edg 2 3 (GLL.SimpleLeftRecursion.B 3)
//             edg 3 4 (GLL.SimpleLeftRecursion.RNGLR_EOF 0)
//             ] |> ignore
//
//        test GLL.SimpleLeftRecursion.buildAbstractAst qGraph GLL.SimpleLeftRecursion.numToString "SimpleLeftRecursion.dot" 19 21 5 0 GLL.SimpleLeftRecursion.tokenData GLL.SimpleLeftRecursion.tokenToNumber

//    [<Test>]
//    member this._50_SimpleBranch () =
//        let input = 
//            new LinearInput(
//                Array.map (GLL.ParseSimpleBranch.tokenToNumber >> (fun x -> x * 1<token>))
//                    [|GLL.ParseSimpleBranch.Token.A 1;
//                      GLL.ParseSimpleBranch.Token.B 1|])
//
//        let res = isParsed GLL.ParseSimpleBranch.parserSource input
//
//        shouldBeTrue res
        
[<EntryPoint>]
let f x =
    System.Runtime.GCSettings.LatencyMode <- System.Runtime.GCLatencyMode.LowLatency
    let t = new ``GLL abstract parser tests``()   

//         @"C:\gsv\projects\YC\YaccConstructor\tests\data\RDF\1.1.ttl"
//         @"C:\gsv\projects\YC\YaccConstructor\tests\data\RDF\wine.rdf"
//         @"C:\gsv\projects\YC\YaccConstructor\tests\data\RDF\pizza.owl"
//         @"C:\gsv\projects\YC\YaccConstructor\tests\data\RDF\foaf.rdf" 
//         @"C:\gsv\projects\YC\YaccConstructor\tests\data\RDF\people_pets.rdf"
//        ] do
//
//            let f = 
//                YC.GLL.Abstarct.Tests.RDFPerformance.loadFromFile p
//                 
//            printfn "triples in %A: %A" (System.IO.Path.GetFileName p) f.Triples.Count
    //YC.GLL.Abstarct.Tests.RDFPerformance.parse @"C:\gsv\projects\YC\YaccConstructor\tests\data\RDF\foaf.rdf"
    //YC.GLL.Abstarct.Tests.RDFPerformance.parse @"C:\gsv\projects\YC\YaccConstructor\tests\data\RDF\wine.rdf"
    YC.GLL.Abstarct.Tests.RDFPerformance.performTests()
    0


//               t.PerformanceTestLinearBadLeftRec()
//               t.PerformanceTestLinearUnambBraces()
//               t.PerformanceTestLinearAmbBraces()

