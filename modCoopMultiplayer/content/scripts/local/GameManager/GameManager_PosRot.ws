exec function SetMoveTo_Player(id:int, xp:int, yp:int, zp:int)//agen set
{
    var data : MyData;
    data = GetServerSettings();
    _SetMoveTo(data.PlayersDATAS[id].Object, MT_Run, Vector(xp, yp, zp));
    if(data.DebugMode)
        Log("[WITCHER3MP] SetMoveTOPlayer: " + VecToString(Vector(xp, yp, zp)));
}
exec function Teleport_Player(id:int, xp:int, yp:int, zp:int)//agen set
{
    var data : MyData;
    data = GetServerSettings();
    _Teleport(data.PlayersDATAS[id].Object, Vector(xp, yp, zp));
    if(data.DebugMode)
        Log("[WITCHER3MP] SetMoveTOPlayer: " + VecToString(Vector(xp, yp, zp)));
}
exec function SetHeading_Player(id:int, xp:int, yp:int, zp:int)//agen set
{
    var data : MyData;
    data = GetServerSettings();
    _SetHeading(data.PlayersDATAS[id].Object, Vector(xp, yp, zp));
    if(data.DebugMode)
        Log("[WITCHER3MP] Setted RotationPlayer " + VecToString(Vector(xp, yp, zp)));
}
//SET TARGET COMBAT
exec function SetCombatTargetMP(id:int, id2:int){
    var data : MyData;
    data = GetServerSettings();
    _SetCombatTarget(data.PlayersDATAS[id].Object, data.NpcEntities[id2]);
    if(data.DebugMode)
        Log("[WITCHER3MP] Setted npc " + id + " |TargetToHit: " + id2);
}