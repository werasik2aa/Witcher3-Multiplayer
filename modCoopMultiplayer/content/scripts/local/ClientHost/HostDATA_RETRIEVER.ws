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
exec function GetNPCCount()
{
    var data : MyModData;
    data = GetServerSettings();
    Log(data.NpcEntities.Size());
}