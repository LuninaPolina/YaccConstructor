//this tables was generated by GNESCC
//source grammar:../../../Tests/GNESCC/test_opt/test_opt.yrd
//date:10/13/2011 18:52:15

module Yard.Generators.GNESCCGenerator.Tables_opt

open Yard.Generators.GNESCCGenerator
open Yard.Generators.GNESCCGenerator.CommonTypes

type symbol =
    | T_PLUS
    | T_NUMBER
    | NT_s
    | NT_gnesccStart
let getTag smb =
    match smb with
    | T_PLUS -> 6
    | T_NUMBER -> 5
    | NT_s -> 4
    | NT_gnesccStart -> 2
let getName tag =
    match tag with
    | 6 -> T_PLUS
    | 5 -> T_NUMBER
    | 4 -> NT_s
    | 2 -> NT_gnesccStart
    | _ -> failwith "getName: bad tag."
let prodToNTerm = 
  [| 1; 0 |];
let symbolIdx = 
  [| 2; 3; 1; 3; 0; 1; 0 |];
let startKernelIdxs =  [0]
let isStart =
  [| [| true; true |];
     [| false; false |];
     [| false; false |];
     [| false; false |]; |]
let gotoTable =
  [| [| Some 1; None |];
     [| None; None |];
     [| None; None |];
     [| None; None |]; |]
let actionTable = 
  [| [| [Error]; [Shift 2]; [Error]; [Error] |];
     [| [Accept]; [Accept]; [Accept]; [Accept] |];
     [| [Shift 3]; [Reduce 1]; [Reduce 1]; [Reduce 1] |];
     [| [Reduce 1]; [Reduce 1]; [Reduce 1]; [Reduce 1] |]; |]
let tables = 
  {StartIdx=startKernelIdxs
   SymbolIdx=symbolIdx
   GotoTable=gotoTable
   ActionTable=actionTable
   IsStart=isStart
   ProdToNTerm=prodToNTerm}
