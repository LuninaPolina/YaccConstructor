module Vectors
open System.IO
open System.Drawing

open Drawing
open BioParser
open DataPreprocessing
open Yard.Frontends.YardFrontend
open Yard.Core.IL

open Yard.Core
open System.Collections.Generic


let formatOutCSVString meta arr cls =
        [
            yield "\"" + meta + "\""
            for i in arr -> uint32 i |> string
            yield cls
        ]    
        |> String.concat ","
        |> fun x -> x + "\n"


let getClassFromMeta (meta: string) classes = 
    let mutable cls = "unable to get class"
    for el in classes do
        if meta.Contains(el)
        then cls <- el
    cls


type toUIntArray() =

    member x.fromGenome (inputPaths, grammar, len, ?isGpu) =    
        let parser = new BioParser(grammar)
        let isGpu = defaultArg isGpu true
        let path = "../../genome_out_" + len.ToString() + ".csv"
        Directory.CreateDirectory(path) |> ignore
        let random = System.Random()
        let cnt = ref 0
        let cntF = ref 0
        for f in inputPaths do
            incr cntF
            let id, gen, intervals16s = getCompleteGenomeData f
            let filteredGen = removeIntervals gen intervals16s
            for i in 0 .. 50 .. filteredGen.Length - len - 1 do
                let name = sprintf "%s_%i_%i" id i len
                let str = filteredGen.[i .. i + len - 2]
                let start = System.DateTime.Now
                let parsed = parser.Parse isGpu str            
                let vector = toIntArray len parser.StartNonTerm parsed
                formatOutCSVString name vector "\"n\""
                |> fun x -> System.IO.File.AppendAllText(path, x)
                printfn "processing time = %A" (System.DateTime.Now - start)
                printfn "file %A gene %A" !cntF !cnt             
                incr cnt


    member x.fromFasta ((inputPath: string), grammar, len, ?isGpu) = 
        let parser = new BioParser(grammar)
        let isGpu = defaultArg isGpu true
        let path = "../../out/" + len.ToString() + ".csv"
        Directory.CreateDirectory(path) |> ignore   
        let (data, classes) = getDataFrom16sBase inputPath 1
        let mutable start = System.DateTime.Now
        let mutable cnt = 0
        data
        |> fun x -> 
            printfn "L=%A" x.Length
            x
        |> Array.iteri (fun i (meta, gen) ->
            printfn "gene %A" i
            let vector = toIntArray len parser.StartNonTerm (parser.Parse isGpu gen)
            formatOutCSVString meta vector "\"" + (getClassFromMeta meta classes) + "\"" 
            |> fun x -> System.IO.File.AppendAllText(path, x)
            cnt <- cnt + 1
            if cnt % 10 = 0 then 
                printfn "processing time = %A" (System.DateTime.Now - start)
                start <- System.DateTime.Now
            )