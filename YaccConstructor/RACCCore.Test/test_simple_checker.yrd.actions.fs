//this file was generated by RACC
//source grammar:..\Tests\RACC\test_simple_checker\\test_simple_checker.yrd
//date:12/16/2010 17:05:25

module RACC.Actions_Simple_checker

open Yard.Generators.RACCGenerator

let value x = (x:>Lexeme<string>).value

type OpType =
   | Undef
   | Mult
   | Plus
   | Minus

let s0 expr = 
    let inner  = 
        match expr with
        | RESeq [x0] -> 
            let (res:int) =
                let yardElemAction expr = 
                    match expr with
                    | RELeaf e -> (e :?> _ )Undef 
                    | x -> "Unexpected type of node\nType " + x.ToString() + " is not expected in this position\nRELeaf was expected." |> failwith

                yardElemAction(x0)
            (res)
        | x -> "Unexpected type of node\nType " + x.ToString() + " is not expected in this position\nRESeq was expected." |> failwith
    box (inner)
let e1 expr = 
    let inner (prevOp) = 
        match expr with
        | REAlt(Some(x), None) -> 
            let yardLAltAction expr = 
                match expr with
                | RESeq [x0] -> 
                    let (n) =
                        let yardElemAction expr = 
                            match expr with
                            | RELeaf tNUMBER -> tNUMBER :?> 'a
                            | x -> "Unexpected type of node\nType " + x.ToString() + " is not expected in this position\nRELeaf was expected." |> failwith

                        yardElemAction(x0)
                    (value n |> int)
                | x -> "Unexpected type of node\nType " + x.ToString() + " is not expected in this position\nRESeq was expected." |> failwith

            yardLAltAction x 
        | REAlt(None, Some(x)) -> 
            let yardRAltAction expr = 
                match expr with
                | RESeq [x0; x1; x2] -> 
                    let (l) =
                        let yardElemAction expr = 
                            match expr with
                            | RELeaf e -> (e :?> _ )Undef 
                            | x -> "Unexpected type of node\nType " + x.ToString() + " is not expected in this position\nRELeaf was expected." |> failwith

                        yardElemAction(x0)
                    let (op,opType) =
                        let yardElemAction expr = 
                            match expr with
                            | REAlt(Some(x), None) -> 
                                let yardLAltAction expr = 
                                    match expr with
                                    | RESeq [_] -> 

                                        ( (+),Plus )
                                    | x -> "Unexpected type of node\nType " + x.ToString() + " is not expected in this position\nRESeq was expected." |> failwith

                                yardLAltAction x 
                            | REAlt(None, Some(x)) -> 
                                let yardRAltAction expr = 
                                    match expr with
                                    | REAlt(Some(x), None) -> 
                                        let yardLAltAction expr = 
                                            match expr with
                                            | RESeq [_] -> 

                                                ( ( * ),Mult )
                                            | x -> "Unexpected type of node\nType " + x.ToString() + " is not expected in this position\nRESeq was expected." |> failwith

                                        yardLAltAction x 
                                    | REAlt(None, Some(x)) -> 
                                        let yardRAltAction expr = 
                                            match expr with
                                            | RESeq [_] -> 

                                                ( (-),Minus )
                                            | x -> "Unexpected type of node\nType " + x.ToString() + " is not expected in this position\nRESeq was expected." |> failwith

                                        yardRAltAction x 
                                    | x -> "Unexpected type of node\nType " + x.ToString() + " is not expected in this position\nREAlt was expected." |> failwith

                                yardRAltAction x 
                            | x -> "Unexpected type of node\nType " + x.ToString() + " is not expected in this position\nREAlt was expected." |> failwith

                        yardElemAction(x1)
                    if not (match prevOp,opType with | Undef,_ | _,Mult | ((Plus|Minus),(Plus|Minus)) -> true | _,_ -> false) then raise Constants.CheckerFalse

                    let (r) =
                        let yardElemAction expr = 
                            match expr with
                            | RELeaf e -> (e :?> _ )opType 
                            | x -> "Unexpected type of node\nType " + x.ToString() + " is not expected in this position\nRELeaf was expected." |> failwith

                        yardElemAction(x2)
                    (op l r)
                | x -> "Unexpected type of node\nType " + x.ToString() + " is not expected in this position\nRESeq was expected." |> failwith

            yardRAltAction x 
        | x -> "Unexpected type of node\nType " + x.ToString() + " is not expected in this position\nREAlt was expected." |> failwith
    box (inner)

let ruleToAction = dict [|("e",e1); ("s",s0)|]

