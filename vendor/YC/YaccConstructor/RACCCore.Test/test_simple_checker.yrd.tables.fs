//this tables was generated by RACC
//source grammar:..\Tests\RACC\test_simple_checker\test_simple_checker.yrd
//date:6/7/2011 10:14:45

#light "off"
module Yard.Generators.RACCGenerator.Tables_Simple_checker

open Yard.Generators.RACCGenerator

type symbol =
    | T_MINUS
    | T_MULT
    | T_PLUS
    | T_NUMBER
    | NT_e
    | NT_s
    | NT_raccStart
let getTag smb =
    match smb with
    | T_MINUS -> 6
    | T_MULT -> 5
    | T_PLUS -> 4
    | T_NUMBER -> 3
    | NT_e -> 2
    | NT_s -> 1
    | NT_raccStart -> 0
let getName tag =
    match tag with
    | 6 -> T_MINUS
    | 5 -> T_MULT
    | 4 -> T_PLUS
    | 3 -> T_NUMBER
    | 2 -> NT_e
    | 1 -> NT_s
    | 0 -> NT_raccStart
    | _ -> failwith "getName: bad tag."
let private autumataDict = 
dict [|(0,{ 
   DIDToStateMap = dict [|(0,(State 0));(1,(State 1));(2,DummyState)|];
   DStartState   = 0;
   DFinaleStates = Set.ofArray [|1|];
   DRules        = Set.ofArray [|{ 
   FromStateID = 0;
   Symbol      = (DSymbol 1);
   Label       = Set.ofArray [|List.ofArray [|(FATrace (TSmbS 0))|]
|];
   ToStateID   = 1;
}
;{ 
   FromStateID = 1;
   Symbol      = Dummy;
   Label       = Set.ofArray [|List.ofArray [|(FATrace (TSmbE 0))|]
|];
   ToStateID   = 2;
}
|];
}
);(1,{ 
   DIDToStateMap = dict [|(0,(State 0));(1,(State 1));(2,DummyState)|];
   DStartState   = 0;
   DFinaleStates = Set.ofArray [|1|];
   DRules        = Set.ofArray [|{ 
   FromStateID = 0;
   Symbol      = (DSymbol 2);
   Label       = Set.ofArray [|List.ofArray [|(FATrace (TSeqS 2));(FATrace (TSmbS 1))|]
|];
   ToStateID   = 1;
}
;{ 
   FromStateID = 1;
   Symbol      = Dummy;
   Label       = Set.ofArray [|List.ofArray [|(FATrace (TSmbE 1));(FATrace (TSeqE 2))|]
|];
   ToStateID   = 2;
}
|];
}
);(2,{ 
   DIDToStateMap = dict [|(0,(State 0));(1,(State 1));(2,(State 2));(3,(State 3));(4,(State 4));(5,(State 5));(6,(State 6));(7,DummyState);(8,DummyState)|];
   DStartState   = 0;
   DFinaleStates = Set.ofArray [|1;6|];
   DRules        = Set.ofArray [|{ 
   FromStateID = 0;
   Symbol      = (DSymbol 2);
   Label       = Set.ofArray [|List.ofArray [|(FATrace (TAlt1S 18));(FATrace (TSeqS 4));(FATrace (TSmbS 3))|]
;List.ofArray [|(FATrace (TAlt2S 19));(FATrace (TSeqS 17));(FATrace (TSmbS 5))|]
|];
   ToStateID   = 2;
}
;{ 
   FromStateID = 0;
   Symbol      = (DSymbol 3);
   Label       = Set.ofArray [|List.ofArray [|(FATrace (TAlt1S 18));(FATrace (TSeqS 4));(FATrace (TSmbS 3))|]
;List.ofArray [|(FATrace (TAlt2S 19));(FATrace (TSeqS 17));(FATrace (TSmbS 5))|]
|];
   ToStateID   = 1;
}
;{ 
   FromStateID = 1;
   Symbol      = Dummy;
   Label       = Set.ofArray [|List.ofArray [|(FATrace (TSmbE 3));(FATrace (TSeqE 4));(FATrace (TAlt1E 18))|]
|];
   ToStateID   = 7;
}
;{ 
   FromStateID = 2;
   Symbol      = (DSymbol 4);
   Label       = Set.ofArray [|List.ofArray [|(FATrace (TSmbE 5));(FATrace (TAlt1S 14));(FATrace (TSeqS 7));(FATrace (TSmbS 6))|]
;List.ofArray [|(FATrace (TSmbE 5));(FATrace (TAlt2S 15));(FATrace (TAlt1S 12));(FATrace (TSeqS 9));(FATrace (TSmbS 8))|]
;List.ofArray [|(FATrace (TSmbE 5));(FATrace (TAlt2S 15));(FATrace (TAlt2S 13));(FATrace (TSeqS 11));(FATrace (TSmbS 10))|]
|];
   ToStateID   = 3;
}
;{ 
   FromStateID = 2;
   Symbol      = (DSymbol 5);
   Label       = Set.ofArray [|List.ofArray [|(FATrace (TSmbE 5));(FATrace (TAlt1S 14));(FATrace (TSeqS 7));(FATrace (TSmbS 6))|]
;List.ofArray [|(FATrace (TSmbE 5));(FATrace (TAlt2S 15));(FATrace (TAlt1S 12));(FATrace (TSeqS 9));(FATrace (TSmbS 8))|]
;List.ofArray [|(FATrace (TSmbE 5));(FATrace (TAlt2S 15));(FATrace (TAlt2S 13));(FATrace (TSeqS 11));(FATrace (TSmbS 10))|]
|];
   ToStateID   = 4;
}
;{ 
   FromStateID = 2;
   Symbol      = (DSymbol 6);
   Label       = Set.ofArray [|List.ofArray [|(FATrace (TSmbE 5));(FATrace (TAlt1S 14));(FATrace (TSeqS 7));(FATrace (TSmbS 6))|]
;List.ofArray [|(FATrace (TSmbE 5));(FATrace (TAlt2S 15));(FATrace (TAlt1S 12));(FATrace (TSeqS 9));(FATrace (TSmbS 8))|]
;List.ofArray [|(FATrace (TSmbE 5));(FATrace (TAlt2S 15));(FATrace (TAlt2S 13));(FATrace (TSeqS 11));(FATrace (TSmbS 10))|]
|];
   ToStateID   = 5;
}
;{ 
   FromStateID = 3;
   Symbol      = (DSymbol 2);
   Label       = Set.ofArray [|List.ofArray [|(FATrace (TSmbE 6));(FATrace (TSeqE 7));(FATrace (TAlt1E 14));(FATrace (TSmbS 16))|]
|];
   ToStateID   = 6;
}
;{ 
   FromStateID = 4;
   Symbol      = (DSymbol 2);
   Label       = Set.ofArray [|List.ofArray [|(FATrace (TSmbE 8));(FATrace (TSeqE 9));(FATrace (TAlt1E 12));(FATrace (TAlt2E 15));(FATrace (TSmbS 16))|]
|];
   ToStateID   = 6;
}
;{ 
   FromStateID = 5;
   Symbol      = (DSymbol 2);
   Label       = Set.ofArray [|List.ofArray [|(FATrace (TSmbE 10));(FATrace (TSeqE 11));(FATrace (TAlt2E 13));(FATrace (TAlt2E 15));(FATrace (TSmbS 16))|]
|];
   ToStateID   = 6;
}
;{ 
   FromStateID = 6;
   Symbol      = Dummy;
   Label       = Set.ofArray [|List.ofArray [|(FATrace (TSmbE 16));(FATrace (TSeqE 17));(FATrace (TAlt2E 19))|]
|];
   ToStateID   = 8;
}
|];
}
)|]

let private gotoSet = 
    Set.ofArray [|((0, 0, 1), set [(0, 1)]);((0, 0, 2), set [(1, 1); (2, 2)]);((0, 0, 3), set [(2, 1)]);((1, 0, 2), set [(1, 1); (2, 2)]);((1, 0, 3), set [(2, 1)]);((2, 0, 2), set [(2, 2)]);((2, 0, 3), set [(2, 1)]);((2, 2, 4), set [(2, 3)]);((2, 2, 5), set [(2, 4)]);((2, 2, 6), set [(2, 5)]);((2, 3, 2), set [(2, 2); (2, 6)]);((2, 3, 3), set [(2, 1)]);((2, 4, 2), set [(2, 2); (2, 6)]);((2, 4, 3), set [(2, 1)]);((2, 5, 2), set [(2, 2); (2, 6)]);((2, 5, 3), set [(2, 1)])|]
    |> dict
let tables = { gotoSet = gotoSet; automataDict = autumataDict }

