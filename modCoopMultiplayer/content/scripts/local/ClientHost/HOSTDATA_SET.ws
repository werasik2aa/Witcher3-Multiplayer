exec function SetHostDebugMode(safa : int)
{
    var data : Storage;
    data = getStorage();
    data.DebugMode = (safa == 1);
    Log("[WITCHER3MP] Debug Enabled: " + data.DebugMode);
}
exec function SetMaxPlayers(a:int)
{
    var data : Storage;
    data = getStorage();
    data.MaxPlayers = a;
    if(getStorage().DebugMode)
        Log("[WITCHER3MP] Succesfully changed MaxPlayers TO: " + a);
}
exec function SetCurPlayers(a:int)
{
    var data : Storage;
    data = getStorage();
    data.CurPlayers = a;
    if(getStorage().DebugMode)
        Log("[WITCHER3MP] Succesfully changed MaxPlayers TO: " + a);
}
exec function OnDisconnect(clid:int)
{
    var data : Storage;
    data = getStorage();
    data.removePlayer(clid);
    GetWitcherPlayer().DisplayHudMessage("PlayerID: " + clid + " Disconnected from game");
}