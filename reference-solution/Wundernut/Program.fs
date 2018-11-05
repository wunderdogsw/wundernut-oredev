module Wundernut
open System

let getNeighbors (str: string) (index: int) =
    let minIndex = max 0 (index - 2)
    let maxIndex = min (str.Length - 1) (index + 2)
    str |> Seq.mapi (fun i el -> el, i) 
        |> Seq.filter (fun (el, i) -> i >= minIndex && i <= maxIndex && i <> index) 
        |> Seq.map fst
        |> System.String.Concat

let getNextLine (rows: List<string>) =
    let lastRow = Seq.last rows
    let next = lastRow 
                |> Seq.mapi (fun i el ->
                    let count = (getNeighbors lastRow i) |> Seq.filter ((=) '#') |> Seq.length
                    match el with 
                    | '.' -> if ((=) 2 count) || ((=) 3 count) then '#' else '.'
                    | '#' -> if ((=) 2 count) || ((=) 4 count) then '#' else '.'
                    | _ -> el) 
                |> System.String.Concat
    rows@[next]

let getType (rows: List<string>) = 
    let blinking (item: string) (el: string) = ((=) el item)
    let gliding (item: string) (el: string) = (el + el).Contains(item)
    let isVanishing last = last |> Seq.forall(fun el -> ((=) el '.'))
    let exists last fn = Seq.filter (fn last) rows |> Seq.length > 1
    match Seq.last rows with
    | row when exists row blinking -> "blinking"
    | row when exists row gliding -> "gliding"
    | row when isVanishing row -> "vanishing"
    | _ -> "other"

let addPadding (rows: List<string>) =
    let reverse (str: string) = System.String(Array.rev (str.ToCharArray()))
    let dots = Seq.takeWhile ((=) '.') >> Seq.length
    let lastRow = Seq.last rows
    let padding = String.init ((-) 3 (min (dots lastRow.[..2]) (dots (reverse lastRow).[..2]))) (fun i -> ".")
    List.map (fun row -> padding + row + padding) rows

let rec processPattern (rows: List<string>) =
    match getType rows with 
    | "other" when rows.Length < 100 -> processPattern (rows |> (addPadding >> getNextLine))
    | pattern -> pattern

[<EntryPoint>]
let main argv =
    System.IO.File.ReadAllLines("patterns.txt")  |> Array.iter (fun p -> printfn "%s" (processPattern [p]))
    0 // return an integer exit code