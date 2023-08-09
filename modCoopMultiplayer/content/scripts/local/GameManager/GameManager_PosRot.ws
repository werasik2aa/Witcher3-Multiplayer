//PLAYER SYNC POSITION
exec function SetMoveTo_Player(idcl:int, xp:int, yp:int, zp:int)//agen set
{
    var data : Storage;
    data = getStorage();
    data.setPlayerPos(idcl, Vector(xp, yp, zp));
    if(data.DebugMode)
        Log("[WITCHER3MP] SetMoveTOPlayer: " + VecToString(Vector(xp, yp, zp)));
}
exec function Teleport_Player(idcl:int, xp:int, yp:int, zp:int)//agen set
{
    var data : Storage;
    data = getStorage();
    _Teleport(data.getPlayerOBJ(idcl), Vector(xp, yp, zp));
    if(data.DebugMode)
        Log("[WITCHER3MP] TeleportingPlayer to: " + VecToString(Vector(xp, yp, zp, 1)));
}