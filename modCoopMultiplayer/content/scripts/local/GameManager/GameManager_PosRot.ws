exec function SetMoveTo_Player(idcl:int, xp:int, yp:int, zp:int)//agen set
{
    var data : Storage;
    data = getStorage();
    data.setPlayerPos(idcl, Vector(xp, yp, zp));
    if(data.DebugMode)
        Log("[WITCHER3MP] SetMoveTO_PlayerOBJ: " + VecToString(Vector(xp, yp, zp)));
}
exec function SetHeadingTo_Player(idcl:int, xp:int, yp:int, zp:int)//agen set
{
    var data : Storage;
    data = getStorage();
    _SetHeading(data.getPlayerOBJ(idcl), Vector(xp, yp, zp) - data.getPlayerOBJ(idcl).GetWorldPosition());
    if(data.DebugMode)
        Log("[WITCHER3MP] SetMoveTO_PlayerOBJ: " + VecToString(Vector(xp, yp, zp)));
}
exec function SetMoveTo_PlayerHorse(idcl:int, xp:int, yp:int, zp:int)//agen set
{
    var data : Storage;
    data = getStorage();
    data.setPlayerHorsePos(idcl, Vector(xp, yp, zp));
    if(data.DebugMode)
        Log("[WITCHER3MP] SetMoveTO_PlayerHorse: " + VecToString(Vector(xp, yp, zp)));
}
exec function SetHeadingTo_PlayerHorse(idcl:int, xp:int, yp:int, zp:int)//agen set
{
    var data : Storage;
    data = getStorage();
    _SetHeading(data.getPlayerOBJ(idcl), Vector(xp, yp, zp) - data.getPlayerHORSE(idcl).GetWorldPosition());
    if(data.DebugMode)
        Log("[WITCHER3MP] SetMoveTO_PlayerOBJ: " + VecToString(Vector(xp, yp, zp)));
}
exec function Teleport_Player(idcl:int, xp:int, yp:int, zp:int)//agen set
{
    var data : Storage;
    data = getStorage();
    _Teleport(data.getPlayerOBJ(idcl), Vector(xp, yp, zp));
    if(data.DebugMode)
        Log("[WITCHER3MP] TeleportingPlayer to: " + VecToString(Vector(xp, yp, zp, 1)));
}
exec function Teleport_PlayerHorse(idcl:int, xp:int, yp:int, zp:int)//agen set
{
    var data : Storage;
    data = getStorage();
    _Teleport(data.getPlayerHORSE(idcl), Vector(xp, yp, zp));
    if(data.DebugMode)
        Log("[WITCHER3MP] TeleportingPlayer to: " + VecToString(Vector(xp, yp, zp, 1)));
}