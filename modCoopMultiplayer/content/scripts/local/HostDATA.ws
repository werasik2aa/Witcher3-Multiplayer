exec function SetClientState(safa : int)
{
    var data : MyModData;
    data = GetServerSettings();
    data.ClientState = safa;
    UpdateServerSettings(data);
    if(data.DebugMode)
        Log("[WITCHER3MP] Succesfully changed: " + safa);
}
exec function SetClientDebugMode(safa : int)
{
    var data : MyModData;
    data = GetServerSettings();
    data.DebugMode = (safa == 1);
    UpdateServerSettings(data);
    Log("[WITCHER3MP] Debug Enabled: " + (safa == 1));
}
exec function GetMaxPlayers()
{
    var data : MyModData;
    data = GetServerSettings();
    Log(data.MaxPlayers);
}
exec function GetCurrentPlayers()
{
    var data : MyModData;
    data = GetServerSettings();
    Log(data.CurPlayers);
}
exec function Joined(charid:int,xp:int, yp:int, zp:int, xr:int, yr:int, zr:int)
{
    var data : MyModData;
    data = GetServerSettings();
    SpawnNpcEntity("living_world\enemy_templates\nml_deserters_axe_normal.w2ent", xp, yp, zp, xr, yr, zr);
    if(data.DebugMode)
        Log("[WITCHER3MP] Player Spawned: " + charid);
}