//PLAYER SYNC POSITION
exec function SetMoveTo_Player(id:int, xp:int, yp:int, zp:int)//agen set
{
    var data : MyData;
    data = GetServerSettings();
    _SetMoveTo(id, MT_Run, Vector(xp, yp, zp));
    if(data.DebugMode)
        Log("[WITCHER3MP] SetMoveTOPlayer: " + VecToString(Vector(xp, yp, zp, 1)));
}
exec function Teleport_Player(id:int, xp:int, yp:int, zp:int)//agen set
{
    var data : MyData;
    data = GetServerSettings();
    _Teleport(data.PlayersDATAS[id], Vector(xp, yp, zp));
    if(data.DebugMode)
        Log("[WITCHER3MP] TeleportingPlayer to: " + VecToString(Vector(xp, yp, zp, 1)));
}