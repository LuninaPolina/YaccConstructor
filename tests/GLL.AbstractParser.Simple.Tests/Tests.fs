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
open Yard.Generators.GLL.AbstractParser
open Yard.Generators.GLL.AbstractParserWithoutTree
open YC.Tests.Helper
open Yard.Generators.Common.ASTGLL
open Yard.Generators.GLL.ParserCommon

open GLL.SimpleRightRecursion
open GLL.BadLeftRecursion
open GLL.SimpleAmb

open GLL.SimpleRightNull
open GLL.SimpleLeftRecursion
open GLL.ParseSimpleBranch

open GLL.Brackets
open GLL.CroppedBrackets
open GLL.Eps

open GLL.FirstEps
open GLL.List
open GLL.NotAmbigousSimpleCalc

open GLL.NotAmbigousSimpleCalcWith2Ops
open GLL.ParseCalc
open GLL.ParseSimpleCalc

open GLL.PrettySimpleCalc
open GLL.SimpleCalcWithNTerm
open GLL.SimpleCalcWithNTerms_2

open GLL.SimpleCalcWithNTerms_3
open GLL.SimpleCalcWithNTerms_4
open GLL.Stars

open GLL.StrangeBrackets
open GLL.Stars2

open GLL.ParseAttrs
open GLL.ParseCalc
open GLL.ParseCond
open GLL.ParseCounter
open GLL.ParseCycle
open GLL.ParseEps2
open GLL.ParseEpsilon
open GLL.ParseExpr
open GLL.ParseFirst
open GLL.ParseListEps
open GLL.ParseLolCalc
open GLL.ParseLongCycle
open GLL.ParseLongCycle_BAD
open GLL.ParseLongest
open GLL.ParseMixed
open GLL.ParseOmit


let outputDir = @"../../../src/GLL.AbstractParser.SimpleTest/"

let lbl tokenId = tokenId
let edg f t l = new ParserEdge<_>(f,t,lbl l)

let perfTest2 parse graph =    
    for i = 10 to 200 do
        let g = graph (1 + i) 2 
        let start = System.DateTime.Now
        System.Runtime.GCSettings.LatencyMode <- System.Runtime.GCLatencyMode.LowLatency
        let r = parse g
        System.GC.Collect()        
        let finish = System.DateTime.Now - start
        printfn "%i  : %A" (i+1) finish.TotalSeconds
        System.GC.Collect()
        match r with
        | Error _ ->
            printfn "Error"     
        | Success tree->
            ()//printfn "%s" "sss"

let test buildAbstractAst qGraph  (intToString : int -> string) (fileName : string) nodesCount edgesCount termsCount ambiguityCount tokenData tokenToNum = 

    let r = buildAbstractAst qGraph 
    printfn "%A" r
    match r with
        | Error str ->
            printfn "Error"
            Assert.Fail("")
        | Success tree ->
            tree.AstToDot intToString tokenToNum tokenData (outputDir + fileName)
            let n, e, t, amb = tree.CountCounters
            printfn "%d %d %d %d" n e t amb
            Assert.AreEqual(nodesCount, n, "Nodes count mismatch")
            Assert.AreEqual(edgesCount, e, "Edges count mismatch")
            Assert.AreEqual(termsCount, t, "Terms count mismatch") 
            Assert.AreEqual(ambiguityCount, amb, "Ambiguities count mismatch")
            Assert.Pass()
     
let f arr tokenToNumber = Array.map (fun e -> tokenToNumber e) arr
let len (edges : BioParserEdge[]) : int[] = edges |> Array.map (fun e -> e.Tokens.Length + 1) 

//[<TestFixture>]
//type ``GLL abstract parser tests`` () =
//    [<Test>]
//    member this.``01_PrettySimpleCalc_SequenceInput`` () =
//        let qGraph = new ParserInputGraph(0, 4)
//        qGraph.AddVerticesAndEdgeRange
//            [edg 0 1 (GLL.PrettySimpleCalc.NUM 1)
//             edg 1 2 (GLL.PrettySimpleCalc.PLUS 2)
//             edg 2 3 (GLL.PrettySimpleCalc.NUM 3)
//             edg 3 4 (GLL.PrettySimpleCalc.RNGLR_EOF 0)
//             ] |> ignore
//
//        test GLL.PrettySimpleCalc.buildAbstractAst qGraph GLL.PrettySimpleCalc.numToString "PrettySimpleCalcSeq.dot" 21 24 5 0 GLL.PrettySimpleCalc.tokenData GLL.PrettySimpleCalc.tokenToNumber
//
//    [<Test>]
//    member this.``06_NotAmbigousSimpleCalc_Loop`` () =
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
//    member this.``07_NotAmbigousSimpleCalc_LoopInLoop`` () =
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
////
////    [<Test>]
////    member this._14_NotAmbigousSimpleCalcWith2Ops_Loop () =
////        let qGraph = new ParserInputGraph<_>(0, 7)
////        qGraph.AddVerticesAndEdgeRange
////            [edg 0 1 (GLL.NotAmbigousSimpleCalcWith2Ops.NUM  1)
////             edg 1 2 (GLL.NotAmbigousSimpleCalcWith2Ops.PLUS 2)
////             edg 2 3 (GLL.NotAmbigousSimpleCalcWith2Ops.NUM 3)
////             edg 3 4 (GLL.NotAmbigousSimpleCalcWith2Ops.PLUS 4)
////             edg 4 5 (GLL.NotAmbigousSimpleCalcWith2Ops.NUM 5)
////             edg 5 2 (GLL.NotAmbigousSimpleCalcWith2Ops.MULT 6)
////             edg 4 6 (GLL.NotAmbigousSimpleCalcWith2Ops.NUM 7)
////             edg 6 7 (GLL.NotAmbigousSimpleCalcWith2Ops.RNGLR_EOF 0)
////             ] |> ignore
////        
////        test GLL.NotAmbigousSimpleCalcWith2Ops.buildAbstractAst qGraph GLL.NotAmbigousSimpleCalcWith2Ops.numToString "NotAmbigousSimpleCalcWith2Ops.dot" 0 0 0 0
////
////    [<Test>]
////    member this._15_NotAmbigousSimpleCalcWith2Ops_Loops () =
////        let qGraph = new ParserInputGraph<_>(0, 8)
////        qGraph.AddVerticesAndEdgeRange
////            [edg 0 1 (GLL.NotAmbigousSimpleCalcWith2Ops.NUM  1)
////             edg 1 2 (GLL.NotAmbigousSimpleCalcWith2Ops.PLUS 2)
////             edg 2 3 (GLL.NotAmbigousSimpleCalcWith2Ops.PLUS 3)
////             edg 2 4 (GLL.NotAmbigousSimpleCalcWith2Ops.NUM 4)
////             edg 3 4 (GLL.NotAmbigousSimpleCalcWith2Ops.NUM 5)
////             edg 4 5 (GLL.NotAmbigousSimpleCalcWith2Ops.PLUS 6)
////             edg 5 2 (GLL.NotAmbigousSimpleCalcWith2Ops.NUM 7)
////             edg 4 6 (GLL.NotAmbigousSimpleCalcWith2Ops.PLUS 8)
////             edg 6 7 (GLL.NotAmbigousSimpleCalcWith2Ops.NUM 9)
////             edg 7 8 (GLL.NotAmbigousSimpleCalcWith2Ops.RNGLR_EOF 0)
////             ] |> ignore
////        
////        test GLL.NotAmbigousSimpleCalcWith2Ops.buildAbstractAst qGraph GLL.NotAmbigousSimpleCalcWith2Ops.numToString "NotAmbigousSimpleCalcWith2Ops2.dot" 0 0 0 0
////
//    [<Test>]
//    member this.``16_Stars_Loop`` () =
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
//    member this.``17_Stars2_Loop`` () =
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
//    member this.``19_FirstEps`` () =
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
//    member this.``20_CroppedBrackets`` () =
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
//    member this.``21_Brackets`` () =
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
//    member this.``22_Brackets_BackEdge`` () =
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
//    member this.``24_UnambiguousBrackets_Circle`` () =
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
//    member this.``25_UnambiguousBrackets_BiggerCircle`` () =
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
//    member this.``26_UnambiguousBrackets_Inf`` () =
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
//    member this.``29_Attrs`` () =
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
//    member this.``30_Condition`` () =
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
//    member this.``31_Counter`` () =
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
//    member this.``32_Cycle`` () =
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
//    member this.``33_Epsilon2_with_eps2_yrd`` () =
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
//    member this.``34_Epsilon`` () =
//        let qGraph = new ParserInputGraph<_>(0, 1)
//        qGraph.AddVerticesAndEdgeRange
//           [edg 0 1 (GLL.ParseEpsilon.RNGLR_EOF 0)
//            ] |> ignore
//
//        test GLL.ParseEpsilon.buildAbstractAst qGraph GLL.ParseEpsilon.numToString "Epsilon.dot" 21 24 5 0 GLL.ParseEpsilon.tokenData GLL.ParseEpsilon.tokenToNumber
//        
//    [<Test>]
//    member this.``35_Expression`` () =
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
//    member this.``36_First`` () =
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
//    member this.``37_ListEps`` () =
//        let qGraph = new ParserInputGraph<_>(0, 3)
//        qGraph.AddVerticesAndEdgeRange
//           [edg 0 1 (GLL.ParseListEps.NUM 1)
//            edg 1 2 (GLL.ParseListEps.NUM 2)
//            edg 2 3 (GLL.ParseListEps.RNGLR_EOF 0)
//            ] |> ignore
//
//        test GLL.ParseListEps.buildAbstractAst qGraph GLL.ParseListEps.numToString "ListEps.dot" 72 93 13 3 GLL.ParseListEps.tokenData GLL.ParseListEps.tokenToNumber
////    
////    [<Test>]
////    member this._38_LolCalc () =
////        let qGraph = new ParserInputGraph<_>(0, 12)
////        qGraph.AddVerticesAndEdgeRange
////           [edg 0 1 (GLL.ParseLolCalc.A 1)
////            edg 1 2 (GLL.ParseLolCalc.MUL 2)
////            edg 2 3 (GLL.ParseLolCalc.B 3)
////            edg 3 4 (GLL.ParseLolCalc.ADD 4)
////            edg 4 5 (GLL.ParseLolCalc.A 5)
////            edg 5 6 (GLL.ParseLolCalc.MUL 6)
////            edg 6 7 (GLL.ParseLolCalc.B 7)
////            edg 7 8 (GLL.ParseLolCalc.ADD 8)
////            edg 8 9 (GLL.ParseLolCalc.B 9)
////            edg 9 10 (GLL.ParseLolCalc.MUL 10)
////            edg 10 11 (GLL.ParseLolCalc.A 11)
////            edg 11 12 (GLL.ParseLolCalc.RNGLR_EOF 0)
////            ] |> ignore
////
////        test GLL.ParseLolCalc.buildAbstractAst qGraph GLL.ParseLolCalc.numToString "LolCalc.dot" 0 0 0 0
////    
//    [<Test>]
//    member this.``39_LongCycle`` () =
//        let qGraph = new ParserInputGraph<_>(0, 2)
//        qGraph.AddVerticesAndEdgeRange
//           [edg 0 1 (GLL.ParseLongCycle.A 1)
//            edg 1 2 (GLL.ParseLongCycle.RNGLR_EOF 0)
//            ] |> ignore
//
//        test GLL.ParseLongCycle.buildAbstractAst qGraph GLL.ParseLongCycle.numToString "LongCycle.dot" 14 18 3 1 GLL.ParseLongCycle.tokenData GLL.ParseLongCycle.tokenToNumber
//    
//    [<Test>]
//    member this.``41_Longest`` () =
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
//    member this.``42_Mixed`` () =
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
//    member this.``43_Omit`` () =
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
////    [<Test>]
////    member this._44_Order () =
////        let qGraph = new ParserInputGraph<_>(0, 9)
////        qGraph.AddVerticesAndEdgeRange
////           [edg 0 1 (GLL.ParseOrder.A 1)
////            edg 1 2 (GLL.ParseOrder.A 2)
////            edg 2 3 (GLL.ParseOrder.A 3)
////            edg 3 4 (GLL.ParseOrder.A 4)
////            edg 4 5 (GLL.ParseOrder.A 5)
////            edg 5 6 (GLL.ParseOrder.A 6)
////            edg 6 7 (GLL.ParseOrder.A 7)
////            edg 7 8 (GLL.ParseOrder.A 8)
////            edg 8 9 (GLL.ParseOrder.RNGLR_EOF 0)
////            ] |> ignore
////
////        test GLL.ParseOrder.buildAbstractAst qGraph GLL.ParseOrder.numToString "Order.dot"
//
//    [<Test>]
//    member this.``45_SimpleRightRecursion`` () =
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
//    member this.``46_BadLeftRecursion`` () =
//        let qGraph = new ParserInputGraph<_>(0, 4)
//        qGraph.AddVerticesAndEdgeRange
//            [edg 0 1 (GLL.BadLeftRecursion.B 1)
//             edg 1 2 (GLL.BadLeftRecursion.B 2)
//             edg 2 3 (GLL.BadLeftRecursion.B 3)
//             edg 3 4 (GLL.BadLeftRecursion.RNGLR_EOF 0)
//             ] |> ignore
//
//        test GLL.BadLeftRecursion.buildAbstractAst qGraph GLL.BadLeftRecursion.numToString "BadLeftRecursion.dot" 33 45 5 1 GLL.BadLeftRecursion.tokenData GLL.BadLeftRecursion.tokenToNumber
//
//    [<Test>]
//    member this.``47_SimpleAmb`` () =
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
//    member this.``48_SimpleRightNull`` () =
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
//    member this.``49_SimpleLeftRecursion`` () =
//        let qGraph = new ParserInputGraph<_>(0, 4)
//        qGraph.AddVerticesAndEdgeRange
//            [edg 0 1 (GLL.SimpleLeftRecursion.B 1)
//             edg 1 2 (GLL.SimpleLeftRecursion.B 2)
//             edg 2 3 (GLL.SimpleLeftRecursion.B 3)
//             edg 3 4 (GLL.SimpleLeftRecursion.RNGLR_EOF 0)
//             ] |> ignore
//
//        test GLL.SimpleLeftRecursion.buildAbstractAst qGraph GLL.SimpleLeftRecursion.numToString "SimpleLeftRecursion.dot" 19 21 5 0 GLL.SimpleLeftRecursion.tokenData GLL.SimpleLeftRecursion.tokenToNumber
//
////    [<Test>]
////    member this.``50_SimpleBranch`` () =
////        let qGraph = new ParserInputGraph<_>(0, 3)
////        qGraph.AddVerticesAndEdgeRange
////            [edg 0 1 (GLL.ParseSimpleBranch.A 1)
////             edg 1 2 (GLL.ParseSimpleBranch.C 2)
////             edg 1 2 (GLL.ParseSimpleBranch.B 3)
////             edg 2 3 (GLL.ParseSimpleBranch.RNGLR_EOF 0)
////             ] |> ignore
////
////        test GLL.ParseSimpleBranch.buildAbstractAst qGraph GLL.ParseSimpleBranch.numToString "SimpleBranch.dot" 14 15 5 1 GLL.ParseSimpleBranch.tokenData GLL.ParseSimpleBranch.tokenToNumber
//
//    [<Test>]
//    member this.``TSQL performance test for GLL`` () =  
//        let graphGenerator numberOfBlocks numberOfPath =
//            let final = 100
//            let qGraph = new ParserInputGraph<_>(0, final)
//            let mutable b = 1
//            let mutable e = 2
//            let mutable curB = 1
//            let mutable curE = 3
//            let chains = Array.zeroCreate 5
//            let ra1 = new ResizeArray<_>()
//            ra1.Add(GLL.MsSqlParser.DEC_NUMBER (0))
//            ra1.Add(GLL.MsSqlParser.L_plus_ (1))
//            ra1.Add(GLL.MsSqlParser.IDENT (2))
//            let ra2 = new ResizeArray<_>()
//            ra2.Add(GLL.MsSqlParser.IDENT (3))
//            ra2.Add(GLL.MsSqlParser.L_plus_ (4))
//            ra2.Add(GLL.MsSqlParser.IDENT (5))
//            let ra3 = new ResizeArray<_>()
//            ra3.Add(GLL.MsSqlParser.L_left_bracket_ (6))
//            ra3.Add(GLL.MsSqlParser.IDENT (7))
//            ra3.Add(GLL.MsSqlParser.L_plus_ (8))
//            ra3.Add(GLL.MsSqlParser.IDENT (9))
//            ra3.Add(GLL.MsSqlParser.L_right_bracket_ (10))
//            let ra4 = new ResizeArray<_>()
//            ra4.Add(GLL.MsSqlParser.L_null (11))
//            ra4.Add(GLL.MsSqlParser.L_null (12))
//            let ra5 = new ResizeArray<_>()
//            ra5.Add(GLL.MsSqlParser.STRING_CONST (13))
//            ra5.Add(GLL.MsSqlParser.L_plus_ (14))
//            ra5.Add(GLL.MsSqlParser.IDENT (15))
//            chains.[0] <- ra1
//            chains.[1] <- ra2
//            chains.[2] <- ra3
//            chains.[3] <- ra4
//            chains.[4] <- ra5    
//            (qGraph.AddVerticesAndEdge <| edg 0 1 (GLL.MsSqlParser.L_select (16))) |> ignore
//            for blocks = 0 to numberOfBlocks - 1 do
//                for i = 0 to numberOfPath - 1 do
//                    let curChain = chains.[i]
//                    for k = 0 to curChain.Count - 1 do
//                        if k <> curChain.Count - 1 then
//                            qGraph.AddVerticesAndEdge <| edg curB curE (curChain.[k]) |> ignore  
//                            curB <- curE
//                            curE <- curE + 1
//                        else
//                            qGraph.AddVerticesAndEdge <| edg curB e (curChain.[k]) |> ignore
//                            if i <> numberOfPath - 1 then
//                                curE <- curE
//                                curB <- b
//                if blocks <> numberOfBlocks - 1 then
//                    b <- e
//                    e <- curE               
//                    qGraph.AddVerticesAndEdge <| edg b e (GLL.MsSqlParser.L_comma_ (17)) |> ignore
//                    b <- e
//                    e <- e + 1
//                    curB <- b
//                    curE <- e + 1
//            b <- e
//            e <- curE               
//            qGraph.AddVerticesAndEdge <| edg b e (GLL.MsSqlParser.L_from (18)) |> ignore
//            b <- e
//            e <- e + 1
//            qGraph.AddVerticesAndEdge <| edg b e (GLL.MsSqlParser.IDENT (19)) |> ignore
//            b <- e
//            e <- e + 1
//            qGraph.AddVerticesAndEdge <| edg b e (GLL.MsSqlParser.RNGLR_EOF (20)) |> ignore
//            qGraph.FinalStates <- [|e|]
//            //qGraph.PrintToDot "input.dot" (GLL.MsSqlParser.tokenToNumber >> GLL.MsSqlParser.numToString)
//            qGraph
//
//        let parse = GLL.MsSqlParser.buildAbstractAst
//        perfTest2 parse graphGenerator

let RunTests () =
//    let t = new ``GLL abstract parser tests``()
//    t.``01_PrettySimpleCalc_SequenceInput`` ()
//    t.``07_NotAmbigousSimpleCalc_LoopInLoop`` ()
//    t.``16_Stars_Loop`` ()
//    t.``17_Stars2_Loop`` ()
//    t.``19_FirstEps`` ()
//    t.``20_CroppedBrackets`` ()
//    t.``21_Brackets`` ()
//    t.``22_Brackets_BackEdge`` ()
//    t.``24_UnambiguousBrackets_Circle`` ()
//    t.``25_UnambiguousBrackets_BiggerCircle`` ()
//    t.``26_UnambiguousBrackets_Inf`` ()
//    t.``29_Attrs`` ()
//    t.``30_Condition`` ()
//    t.``31_Counter`` ()
//    t.``32_Cycle`` ()
//    t.``33_Epsilon2_with_eps2_yrd`` ()
//    t.``34_Epsilon`` ()
//    t.``35_Expression`` ()
//    t.``36_First`` ()
//    t.``37_ListEps`` ()
//    t.``39_LongCycle`` ()
//    t.``41_Longest`` ()
//    t.``42_Mixed`` ()
//    t.``43_Omit`` ()
//    t.``45_SimpleRightRecursion`` ()
//    t.``46_BadLeftRecursion`` ()
//    t.``47_SimpleAmb`` ()
//    t.``48_SimpleRightNull`` ()
//    t.``49_SimpleLeftRecursion`` ()
//   // t.``50_SimpleBranch`` ()
//    //t.``TSQL performance test for GLL`` ()
//    t.``06_NotAmbigousSimpleCalc_Loop`` ()
//               
    ()
    