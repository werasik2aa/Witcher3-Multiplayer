exec function GetMODVersion()
{
    Log("W3MP:!MODVER!{@VERS@:" + getStorage().Version + "}");
}
exec function IsInMenu()
{
    var currentMenu : CR4Menu;
    currentMenu = theGame.GetGuiManager().GetRootMenu();
    if (currentMenu)
        Log("W3MP:!INMENUDATA!{ @Value@: true }");
    else
        Log("W3MP:!INMENUDATA!{ @Value@: false }");
}
exec function IsGameStoped()
{
   Log("W3MP:!INSTUPPED!{ @Value@: " + !theGame.IsStopped() + "}");
}
exec function IsGamePaused()
{
    Log("W3MP:!INPAUSED!{ @Value@: " + !theGame.IsPaused() + "}");
}
exec function GetWorldData()
{
    var output: string;
    output += "@GAMETIME@: " + theGame.GetHoursPerMinute();
    output += ",@GAMEDIFFICULT@: " + theGame.GetDifficultyLevel();
    Log("W3MP:!WORLDDATA!{" + output + "}");
}
exec function GetPlayerData()
{
    Log("W3MP:!PLAYERDATA!{" + _GetMyPlayerInfo() + "}");
}
exec function GetTargetData()
{
    Log("W3MP:!COMBATDATA!{" + _GetMyTargetInfo() + "}");
}
exec function GetEntityDataByID(ID:int)
{
    Log("W3MP:!ENTITYDATA!{" + _GetNPC_DataByID(ID) + "}");
}
exec function GetEventsData()
{
    var output: string;
    var events: array<string>;
    var i: int;
    var Key, Value : string;

    events = getStorage().GetEventsData();
    for(i=0; i < events.Size(); i+=1)
    {
        StrSplitFirst(events[i], "::", Key, Value);
        output += "@" + Key + "@: @"  + Value + "@";
    }
    Log("W3MP:!EVENTSDATA!{" + output + "}");
}
exec function GetTargetDataByID(id:int) //GET NPC STATE DEAD OR NO
{
    Log(_GetNPC_DataByID(id));
}

exec function NPCSLen()
{
    var data : MP_NPCSTORAGE;
    var i : int;
    var output : string;
    data = getStorage().getNPCStorage();
    output += "TOTAL: " + data.Entity.Size();
    for(i=0; i < data.Entity.Size(); i+=1)
    {
        if(i==5) break;
        output += "\nID" + data.ID[i] + " : " + VecToString(data.Position[i]) + " : " + data.Template[i];
    }
    Log("W3MP: " + output);
}
exec function PlayersLen()
{
    var data : MP_NPCSTORAGE;
    var i : int;
    var output : string;
    data = getStorage().getPLAYERStorage();
    output += "TOTAL: " + data.Entity.Size();
    for(i=0; i < data.Entity.Size(); i+=1)
    {
        if(i==5) break;
        output += "\nID" + data.ID[i] + " : " + VecToString(data.Position[i]) + " : " + data.Template[i];
    }
    Log("W3MP: " + output);
}