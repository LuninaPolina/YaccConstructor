open System.IO
open System.Drawing
open Drawing
open BioParser
open Pictures
open DataPreprocessing
open Yard.Frontends.YardFrontend
open Yard.Core.IL

open Yard.Core
open System.Collections.Generic

[<EntryPoint>]
let main argv = 
    let inputPath = argv.[0]
    let grammar = argv.[1]
    let len = argv.[2]
   
    let de = new Vectors.toUIntArray()
    de.fromFasta (inputPath, grammar, 220)

    0