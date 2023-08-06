exec function GetMaxPlayers()
{
    Log("WITCHER-3MP " + GetServerSettings().MaxPlayers);
}
exec function GetCurrentPlayers()
{
    var data : MyData;
    data = GetServerSettings();
    Log("WITCHER-3MP " + data.PlayersDATAS.Size());
}
exec function GetNPCCount()
{
    var data : MyData;
    data = GetServerSettings();
    Log("WITCHER-3MP " + data.NpcEntities.Size());
}
