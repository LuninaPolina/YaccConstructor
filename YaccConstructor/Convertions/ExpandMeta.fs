﻿//  Module ExpandMeta contains:
//  - function, which expands metarules 
//
//  Copyright 2009,2010 Ilia Chemodanov
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

module Yard.Core.Convertions.ExpandMeta 

open Yard.Core
open Yard.Core.IL
open Production
open Yard.Core.Namer
open TransformAux

type Dictionary<'a,'b> = System.Collections.Generic.Dictionary<'a,'b>

/// find metarule with given name in hash map of collected metarules
let findMetaRule (tbl:Dictionary<string,Rule.t<Source.t,Source.t>>) mName = 
    try tbl.Item mName with
    | :?System.Collections.Generic.KeyNotFoundException ->
        failwith <| sprintf "undeclared metarule %s" mName;
    

/// Create pair (formal argument name, actual argument name)
let addBindingPair attrs (*binding*) = function
    | None -> attrs
    | Some b -> if List.exists (fun x -> snd x = b) attrs  then attrs
                else (createNewName ("arg", (0,0)), b)::attrs

/// Bind all heritable attributes of rule
let getRuleBindings (rule : Rule.t<Source.t,Source.t>) init =
    let rec accBindings res = function
    | (h : Source.t)::t ->
        let splitted =
            (fst h).Split([|' '; '\t'; '\r'; '\n'|])
            |> Array.toList
            |> List.filter (fun s -> s <> "")
        let curRes =
            List.fold
                (fun res x -> addBindingPair res (Some (x,(0,0))))
                [] splitted
        accBindings (curRes @ res) t
    | [] -> res
    accBindings init rule.args

/// <summary>
/// <para> Replace rule with new one, replacing references to metarules, </para>
/// <para> and generate new rules for every such reference </para>
/// <para> body: t - production which can contain metareferences </para>
/// <para> metaRules: Dictionary&lt;string,Rule.t&lt;Source.t,Source.t&gt;&gt; - table, which contains expanding metareferences rules </para>
/// <para> expanded: Dictionary&lt;string,Production.t&gt; - map of metareference with actual params to generated rule name </para>
/// <para> res: Rule.t list - currently generated rules </para>
/// <para> returns (new body, generated rules + old rules) </para>
/// </summary>
let expandMeta body globalAttrs metaRules expanded res =
    /// Replace formal parameter with its actual value or (if it is not to be replaced) 
    let tryReplaceActual (formalToAct : (string * Production.t<'a,'b>) list) formal prev = 
        match List.tryFind (fun x -> fst x = formal) formalToAct with
        | None -> prev
        | Some res -> snd res

    /// <summary>
    /// <para> Replace rule body, expanding references to metarules, </para>
    /// <para> and generate new rules for every such reference </para>
    /// <para> body: t - production which can contain metareferences </para>
    /// <para> globalAttrs: list of heritable attributes in format (formal name, actual name) </para>
    /// <para> metaRules: Dictionary&lt;string,Rule.t&lt;Source.t,Source.t&gt;&gt; - table, which contains expanding metareferences rules </para>
    /// <para> expanded: Dictionary&lt;string,Production.t&gt; - map of metareference with actual params to generated rule name </para>
    /// <para> res: Rule.t list - currently generated rules </para>
    /// <para> returns (new body, generated rules + old rules) </para>
    /// </summary>
    let rec expandBody body globalAttrs (metaRules: Dictionary<string,Rule.t<Source.t,Source.t> >)
            (expanded : Dictionary<string, Production.t<'a,'b>>) res =
        //printfn "b: %A" body
        /// Returns key for table of expanded rules
        let getKey body globalAttrs =
            let attrsToString = 
                String.concat "#$%^"
                    (globalAttrs
                     |> List.map
                        (fun (f,a) -> "(" + Source.toString f + "," + a.ToString() + ")")
                    )
            (body.ToString()) + "^^" + attrsToString
            //|> (fun x -> printfn "k = %s" x; x)

        let key = getKey body globalAttrs
        if expanded.ContainsKey(key) then 
//            printfn "ok %A\n: \t%A\n\n:\t%A\n=========================\n" body (expanded.Item key) res
            (expanded.Item key, res)
        else
            let rule, res =
                /// Expand rule with the previous parameters
                let simpleExpand body = expandBody body globalAttrs metaRules expanded res

                /// Applyes function to the first element of result
                let applyToRes f (a,b) = (f a, b)

                let expandMetaRef name attrs metaArgs =
                    let (newMetaArgs, newRes) =
                        metaArgs
                        |> List.fold
                            (fun (accMeta, accRes) body ->
                                expandBody body globalAttrs metaRules expanded accRes
                                |> (fun (r, accRes) ->
                                        let newMetaArgName = (createNewName ("rule", (0,0)))
                                        let newMetaArg = PRef(newMetaArgName, list2opt (createParams (List.map fst globalAttrs)))
                                        let (newRule: Rule.t<_,_>) =
                                            {name = Source.toString newMetaArgName;
                                            args = List.map snd globalAttrs;
                                            metaArgs = [];
                                            _public = false;
                                            body = r}
                                        //printfn "%A" newRule
                                        (newMetaArg::accMeta, newRule::accRes))
                            )
                            ([], res)
                        |> applyToRes (List.rev)
                    // TODO catch exception
                    let metaRule = findMetaRule metaRules name
                    let newRuleName = (createNewName ("rule_" + name, (0,0)))
                    let actualArgs =
                        match attrs with
                        | None -> (List.map snd globalAttrs)
                        | Some h -> h::(List.map snd globalAttrs)
                    let formalArgs = (metaRule.args)@(List.map fst globalAttrs)
                    let substitution = PRef(newRuleName, list2opt (createParams actualArgs))
                    let newKey = getKey (PMetaRef(createSource name, attrs, newMetaArgs)) globalAttrs
                    if not (expanded.ContainsKey key) then
                        expanded.Add(key, substitution)
                    if not (expanded.ContainsKey newKey) then
                        expanded.Add(newKey, substitution)
                    let newFormalToAct = List.zip (metaRule.metaArgs |> List.map Source.toString) newMetaArgs
                    // TODO There can be a bug
                    let newGlobalAttrs = getRuleBindings metaRule globalAttrs
                
                    let metaExp = expandBody (replaceMeta newFormalToAct metaRule.body) (*newFormalToAct*)
                                        newGlobalAttrs metaRules expanded newRes 
                    let (newRule: Rule.t<_,_>) =
                        {name = Source.toString newRuleName;
                        args = formalArgs;
                        metaArgs = [];
                        _public = false;
                        body = fst metaExp}
                    (substitution, newRule::(snd metaExp))

                match body with
                | PSome body -> applyToRes PSome (simpleExpand body)
                | POpt body  -> applyToRes POpt  (simpleExpand body)
                | PMany body -> applyToRes PMany (simpleExpand body)
                | PRef(name, attrs) as x -> (x, res)
                | PAlt (l, r) ->
                    (expandBody l globalAttrs metaRules expanded [], simpleExpand r)
                    |> (fun (x, y) -> (PAlt (fst x, fst y), (snd x)@(snd y)))
                | PSeq (ruleList, actionCode) ->
                    ruleList
                    |> List.fold
                        (fun ((curSeq, accRes), attrs) elem' ->
                            let bodyExp = expandBody elem'.rule attrs metaRules expanded accRes
                            (applyToRes (fun h -> {elem' with rule = h}::curSeq) bodyExp, addBindingPair attrs elem'.binding)
                        ) (([], res), globalAttrs)
                    |> fst
                    |> applyToRes (fun x -> PSeq (List.rev x, actionCode))
                | PLiteral _ as literal -> (literal, res)
                | PToken _ as token -> (token, res)
                | PMetaRef (name, attrs, metaArgs) as x -> 
                    if (metaArgs.IsEmpty) then (x, res)
                    else expandMetaRef (Source.toString name) attrs metaArgs 
                | PPerm (_) -> failwith "Unrealised meta-expanding of permutation"
                | PRepet (_) -> failwith "Unrealised meta-expanding of permutation"
            if not (expanded.ContainsKey key) then
                expanded.Add(key, rule)
//            printfn "%A\n: \t%A\n\n:\t%A\n=========================\n" body rule res
            (rule, res)

    /// <para> Replace all rules to be replaced (meta-arguments substitution) in production. </para>
    /// <para> formalToAct - list of (metaArg name, its actual value) </para>
    /// <para> body - production to replace.  </para>
    and replaceMeta formalToAct body = 
        let replace = replaceMeta formalToAct
        match body with
        | PSome body -> PSome (replace body)
        | POpt body  -> POpt  (replace body)
        | PMany body -> PMany (replace body)
        | PRef(name, attrs) as prev ->
            (tryReplaceActual formalToAct (Source.toString name) prev)
        | PAlt (l, r) -> PAlt(replace l, replace r)
        | PSeq (ruleList, actionCode) ->
            PSeq (List.map (fun x ->  {x with rule = replace x.rule}) ruleList, actionCode)
        | PLiteral _ as literal -> literal
        | PToken _ as token -> token
        | PMetaRef (name, attrs, metaArgs) as prev -> 
            if (metaArgs.IsEmpty) then (tryReplaceActual formalToAct (Source.toString name) prev)
            else PMetaRef(name, attrs, List.map replace metaArgs )
        | PPerm (_) -> failwith "Unrealised meta-expanding of permutation"
        | PRepet (_) -> failwith "Unrealised meta-expanding of permutation"

    expandBody body globalAttrs metaRules expanded res

(** grammar processing:
 *  - collect metarules
 *  - expand metarules
 *  - call metarules expanding 
 *)

(** main function 
 *  - create hash tables
 *  - call process grammar to expand metarules
 *)
let expandMetaRules rules =
    /// if rule has metaArgs then it's a metarule
    let isMetaRule (r:Rule.t<'a,'b>) = r.metaArgs <> []

    /// collect a set of declared metaRules
    let rec collectMeta rules ((metaRulesTbl:Dictionary<string,Rule.t<Source.t,Source.t> >),refsTbl) = 
        match rules with 
        | [] -> ()
        | h::t -> 
            if (isMetaRule h) then 
                metaRulesTbl.Add(h.name,h)        
            collectMeta t (metaRulesTbl,refsTbl)
    
    /// Replace existing meta-rules. Suppose that all high-level meta-rules are in metaRulesTbl
    let rec replaceMeta rules ((metaRulesTbl:Dictionary<string,Rule.t<Source.t,Source.t> >), refsTbl) res = 
        match rules with 
        | [] -> res
        | h::t -> 
            if (isMetaRule h) then 
                replaceMeta t (metaRulesTbl, refsTbl) res      
            else 
                let (b, rules) = expandMeta (h:Rule.t<Source.t,Source.t>).body
                                        (getRuleBindings h []) metaRulesTbl refsTbl res
                let r = { h with Rule.body = b }
                replaceMeta t (metaRulesTbl,refsTbl) (r::rules)

    (** hash table for metarules *)
    let metaRulesTbl = new Dictionary<string,Rule.t<Source.t,Source.t> >(200)
    (** hash table for references to expanded metarules *)
    let refsTbl = new Dictionary<string, Production.t<_,_> >(200)
    collectMeta rules (metaRulesTbl, refsTbl)
    replaceMeta rules (metaRulesTbl, refsTbl) []
    |> List.rev

type ExpandMeta() = 
    inherit Convertion()
        override this.Name = "ExpandMeta"
        override this.ConvertList ruleList = expandMetaRules ruleList
        override this.EliminatedProductionTypes = [""]

