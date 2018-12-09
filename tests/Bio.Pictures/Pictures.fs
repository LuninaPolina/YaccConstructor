module Pictures

open System.IO
open System.Drawing

open Drawing
open BioParser
open DataPreprocessing
open Yard.Frontends.YardFrontend
open Yard.Core.IL

open Yard.Core
open System.Collections.Generic


type drawExamples() =

    member x.fromFasta (inputPath, grammar, len, ?isGpu, (?legend:(string*Color) list), ?sortNum, ?format) =
        let parser = new BioParser(grammar)
        let sortNum = defaultArg sortNum 1
        let isGpu = defaultArg isGpu true
        let legend = defaultArg legend [(parser.StartNonTerm, Color.Black)]
        let data = getDataFrom16sBase inputPath sortNum
        let format = defaultArg format ".bmp"
        let dir = "../../out/" + len.ToString() + "/"
        Directory.CreateDirectory(dir) |> ignore
        for el in fst data do 
            let (id, gen) = el       
            let path = dir + ([for i in 1..sortNum - 1 -> id.Split().[i]] |> String.concat("/")) + "/" 
            printfn "path=%A"  path
            Directory.CreateDirectory(path) |> ignore
            let picture = new ParsingPicture (len, parser.Parse isGpu gen)
            match format with
            |".bmp" -> picture.Draw(legend, path)
            |".tex" -> picture.DrawInTex(parser.StartNonTerm,gen,path)
            |_ -> failwith "Unsupported output format"
        

    member x.fromGenome (inputPaths, grammar, len, ?isGpu, ?legend) =
        let parser = new BioParser(grammar)
        let isGpu = defaultArg isGpu true
        let legend = defaultArg legend [(parser.StartNonTerm, Color.Black)]
        let dir = "../../out/" + len.ToString() + "/"
        Directory.CreateDirectory(dir) |> ignore
        let random = System.Random()
        for f in inputPaths do
            let id, gen, intervals16s = getCompleteGenomeData f
            let filteredGen = removeIntervals gen intervals16s
            for i in 0 .. 10 .. filteredGen.Length - len - 1 do
                let name = sprintf "%s_%i_%i.bmp" id i len
                let picture = new ParsingPicture(len, parser.Parse isGpu filteredGen.[i .. i + len - 2])
                picture.Draw(legend, (dir + name))