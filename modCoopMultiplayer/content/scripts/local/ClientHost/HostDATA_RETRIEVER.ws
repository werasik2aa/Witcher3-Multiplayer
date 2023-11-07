exec function GetMaxPlayers()
{
    Log("W3MP " + getStorage().MaxPlayers);
}
exec function GetCurrentPlayers()
{
    var data : Storage;
    data = getStorage();
    Log("W3MP " + data.PlayerIDS.Size());
}
exec function GetNPCCount()
{
    var data : Storage;
    data = getStorage();
    Log("W3MP " + data.NpcEntities.Size());
}
exec function GetPlayerCount()
{
    var data : Storage;
    data = getStorage();
    Log("W3MP " + data.PlayerOBJECTS.Size());
}