exec function GetMaxPlayers()
{
    Log("WITCHER-3MP " + GetServerSettings().MaxPlayers);
}
exec function GetCurrentPlayers()
{
    var data : array<PlayerDataStruct>;
    data = GetServerSettings().PlayersDATAS;
    Log("WITCHER-3MP " + data.Size());
}
exec function GetNPCCount()
{
    var data : MyData;
    data = GetServerSettings();
    Log("WITCHER-3MP " + data.NpcEntities.Size());
}
