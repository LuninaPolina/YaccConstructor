﻿//  Driver.fs contains entry point of MS-SQL parser.
//
//  Copyright 2012 Semen Grigorev <rsdpisuy@gmail.com>
//
//  This file is part of YaccConctructor.
//
//  YaccConstructor is free software:you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

module MSSqlParser

open Microsoft.FSharp.Text.Lexing
open Yard.Generators.RNGLR.AST
open Yard.Examples.MSParser

let justParse (path:string) =
    use reader = new System.IO.StreamReader(path)
    let lexbuf = LexBuffer<_>.FromTextReader reader
    let allTokens = seq{while not lexbuf.IsPastEndOfStream do yield Lexer.tokens lexbuf}

    let translateArgs = {
        tokenToRange = fun x -> 0,0
        zeroPosition = 0
        clearAST = false
        filterEpsilons = true
    }

    buildAst allTokens

let Parse (srcFilePath:string) =    
    match justParse srcFilePath with
    | Yard.Generators.RNGLR.Parser.Error (num, tok, msg,dbg) ->
        printfn "Error in file %s on position %d on Token %A: %s" srcFilePath num tok msg
        dbg.drawGSSDot @"..\..\stack.dot"
    | Yard.Generators.RNGLR.Parser.Success ast ->
        ast.collectWarnings (fun x -> 0,0)
        |> ResizeArray.iter (fun (pos, prods) -> ())
        defaultAstToDot ast @"..\..\ast.dot"
        //ast.ChooseLongestMatch()
        //let translated = translate translateArgs ast : list<Script>            
        //printfn "%A" translated
        //translated.Head  
        
let ParseAllDirectory (directoryName:string) =
    System.IO.Directory.GetFiles directoryName
    |> Array.iter Parse
  
  //do ParseAllDirecrory @"..\..\..\..\..\Tests\Materials\ms-sql\sqlsrvanalysissrvcs

do Parse @"C:\Users\Anastasiya\Desktop\Projects\Reengineering\recursive-ascent\Tests\materials\ms-sql\sysprocs\sp_addserver.sql"
// @"..\..\..\..\..\Tests\Materials\ms-sql\sqlsrvanalysissrvcs\MonitoringSSAS\config_data_server\get_query_text.sql"
