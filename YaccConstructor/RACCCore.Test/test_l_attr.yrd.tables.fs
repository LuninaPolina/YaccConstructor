//this tables was generated by RACC
//source grammar:..\Tests\RACC\test_l_attr\\test_l_attr.yrd
//date:12/16/2010 17:05:24

#light "off"
module Yard.Generators.RACCGenerator.Tables_L_attr

open Yard.Generators.RACCGenerator

let autumataDict = 
dict [|("raccStart",{ 
   DIDToStateMap = dict [|(0,(State 0));(1,(State 1));(2,DummyState)|];
   DStartState   = 0;
   DFinaleStates = Set.ofArray [|1|];
   DRules        = Set.ofArray [|{ 
   FromStateID = 0;
   Symbol      = (DSymbol "s");
   Label       = Set.ofArray [|List.ofArray [|(FATrace (TSmbS 0))|]|];
   ToStateID   = 1;
}
;{ 
   FromStateID = 1;
   Symbol      = Dummy;
   Label       = Set.ofArray [|List.ofArray [|(FATrace (TSmbE 0))|]|];
   ToStateID   = 2;
}
|];
}
);("s",{ 
   DIDToStateMap = dict [|(0,(State 0));(1,(State 1));(2,DummyState)|];
   DStartState   = 0;
   DFinaleStates = Set.ofArray [|1|];
   DRules        = Set.ofArray [|{ 
   FromStateID = 0;
   Symbol      = (DSymbol "e");
   Label       = Set.ofArray [|List.ofArray [|(FATrace (TSeqS 2));(FATrace (TSmbS 1))|]|];
   ToStateID   = 1;
}
;{ 
   FromStateID = 1;
   Symbol      = Dummy;
   Label       = Set.ofArray [|List.ofArray [|(FATrace (TSmbE 1));(FATrace (TSeqE 2))|]|];
   ToStateID   = 2;
}
|];
}
);("e",{ 
   DIDToStateMap = dict [|(0,(State 0));(1,(State 1));(2,DummyState)|];
   DStartState   = 0;
   DFinaleStates = Set.ofArray [|1|];
   DRules        = Set.ofArray [|{ 
   FromStateID = 0;
   Symbol      = (DSymbol "NUMBER");
   Label       = Set.ofArray [|List.ofArray [|(FATrace (TSeqS 4));(FATrace (TSmbS 3))|]|];
   ToStateID   = 1;
}
;{ 
   FromStateID = 1;
   Symbol      = Dummy;
   Label       = Set.ofArray [|List.ofArray [|(FATrace (TSmbE 3));(FATrace (TSeqE 4))|]|];
   ToStateID   = 2;
}
|];
}
)|]

let items = 
List.ofArray [|("raccStart",0);("raccStart",1);("raccStart",2);("s",0);("s",1);("s",2);("e",0);("e",1);("e",2)|]

let gotoSet = 
Set.ofArray [|(-1239003080,("raccStart",2));(-1408241699,("e",1));(-635149922,("raccStart",1));(-635149972,("s",1));(123072121,("s",1));(1800901211,("e",2));(1800920813,("s",2));(1904094654,("e",1));(1904107720,("e",1))|]

