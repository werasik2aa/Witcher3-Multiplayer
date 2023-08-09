exec function GetMaxPlayers()
{
    Log("WITCHER-3MP " + getStorage().MaxPlayers);
}
exec function GetCurrentPlayers()
{
    var data : Storage;
    data = getStorage();
    Log("WITCHER-3MP " + data.PlayerIDS.Size());
}
exec function GetNPCCount()
{
    var data : Storage;
    data = getStorage();
    Log("WITCHER-3MP " + data.NpcEntities.Size());
}
exec function GetPlayerPos(aa:int)
{
    var data : Storage;
    data = getStorage();
    Log("WITCHER-3MP " + VecToString(data.getPlayerPOS(aa)));
}