exec function SetHostDebugMode(safa : int)
{
    var data : MyModData;
    data = GetServerSettings();
    data.DebugMode = (safa == 1);
    UpdateServerSettings(data);
    Log("[WITCHER3MP] Debug Enabled: " + (safa == 1));
}
exec function SetMaxPlayers(a:int)
{
    var data : MyModData;
    data = GetServerSettings();
    data.MaxPlayers = a;
    UpdateServerSettings(data);
    if(data.DebugMode)
        Log("[WITCHER3MP] Succesfully changed MaxPlayers TO: " + a);
}
exec function OnLeaveT(a:int)
{
    var data : MyModData;
    data = GetServerSettings();
    _RemovePlayer(data.PlayersEntities[a]);
    if(data.DebugMode)
        Log("[WITCHER3MP] Succesfully RemovePlayer id: " + a);
}