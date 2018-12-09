module DataPreprocessing

open System.Text

let getData path withClasses = 
    let d = new System.Text.StringBuilder()
    let meta = ref ""
    let lst = new ResizeArray<_>()
    let classes = new ResizeArray<_>()
    let mutable isFst = withClasses
    for s in System.IO.File.ReadAllLines(path) do
        if isFst
        then
            classes.AddRange(s.Split(','))
            isFst <- false
        else
            if s.[0] = '>'
            then
                if !meta <> ""
                then 
                    lst.Add(!meta, d.ToString())
                    d.Clear() |> ignore
                meta := s
            else
                d.Append s |> ignore
    lst.Add(!meta, d.ToString())
    lst.ToArray(), classes

let getDataFrom16sBase fastaFile sortNum =
    let data = getData fastaFile true
    fst data
    |> Array.map (fun (meta, gen) -> ([for i in 0..sortNum - 1 -> 
                                                                if i = 0 then meta.Split().[0].[1 ..]
                                                                else meta.Split([|' '; ';'|]).[i]] |> String.concat " ", gen)), snd data


let getCompleteGenomeData fastaFile =
    let data = (fst(getData fastaFile false)).[0]
    let metaParts = (fst data).Split(',') |> Array.map (fun s -> s.Trim())
    let id = metaParts.[0].[1 ..] 
    let intervals16s = 
        metaParts.[3].Split()
        |> Array.map (fun s -> let p = s.Split(':') in (int p.[0], int p.[1]))
    (id, snd data, intervals16s)

let removeIntervals (input: string) toRemove =
    if Array.isEmpty toRemove
    then input
    else
        let builder = new StringBuilder()
        let cur = ref 0
        toRemove
        |> Array.iter 
               (fun (i, j) -> builder.Append input.[!cur .. i] |> ignore; cur := j)
        builder.Append(input.[!cur ..]).ToString()