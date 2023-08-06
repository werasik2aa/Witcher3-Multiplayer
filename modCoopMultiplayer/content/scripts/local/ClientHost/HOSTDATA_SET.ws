exec function SetHostDebugMode(safa : int)
{
    var data : MyData;
    data = GetServerSettings();
    data.DebugMode = (safa == 1);
    UpdateServerSettings(data);
    Log("[WITCHER3MP] Debug Enabled: " + data.DebugMode);
}
exec function SetMaxPlayers(a:int)
{
    var data : MyData;
    data = GetServerSettings();
    data.MaxPlayers = a;
    UpdateServerSettings(data);
    if(GetServerSettings().DebugMode)
        Log("[WITCHER3MP] Succesfully changed MaxPlayers TO: " + a);
}
exec function OnLeaveT(a:int)
{
    var data : MyData;
    data = GetServerSettings();
    _RemovePlayer(data.PlayersDATAS[a]);
    if(GetServerSettings().DebugMode)
        Log("[WITCHER3MP] Succesfully RemovePlayer id: " + a);
}
