exec function Teleport_Player(id:int, xp:int, yp:int, zp:int)
{
    var data : MyModData;
    data = GetServerSettings();
    _Teleport(data.PlayersEntities[id], Vector(xp, yp, zp));
    if(data.DebugMode)
        Log("[WITCHER3MP] TeleportedPlayer: " + VecToString(Vector(xp, yp, zp)));
}
exec function TeleportRotate_Player(id:int, xp:int, yp:int, zp:int, xr:int, yr:int, zr:int)
{
    var data : MyModData;
    data = GetServerSettings();
    _TeleportRotate(data.PlayersEntities[id], Vector(xp, yp, zp), EulerAngles(xr, yr, zr));
    if(data.DebugMode)
        Log("[WITCHER3MP] TeleportedTOAndRotPlayer: " + VecToString(Vector(xp, yp, zp)) + " \nRotation " + "X:" + xr + " Y:" + yr + " Z" + zr);
}
exec function SetMoveTo_Player(id:int, xp:int, yp:int, zp:int)//agen set
{
    var data : MyModData;
    data = GetServerSettings();
    _SetMoveTo(data.PlayersEntities[id], MT_Run, Vector(xp, yp, zp));
    if(data.DebugMode)
        Log("[WITCHER3MP] SetMoveTOPlayer: " + VecToString(Vector(xp, yp, zp)));
}
exec function Rotate_Player(id:int, xp:int, yp:int, zp:int)//agen set
{
    var data : MyModData;
    data = GetServerSettings();
    _SetRotateTo(data.PlayersEntities[id], Vector(xp, yp, zp));
    if(data.DebugMode)
        Log("[WITCHER3MP] Setted RotationPlayer " + "X:" + xp + " Y:" + yp + " Z" + zp);
}

exec function Teleport_Npc(id:int, xp:int, yp:int, zp:int)
{
    var data : MyModData;
    data = GetServerSettings();
    _Teleport(data.NpcEntities[id], Vector(xp, yp, zp));
    if(data.DebugMode)
        Log("[WITCHER3MP] TeleportedNPC: " + VecToString(Vector(xp, yp, zp)));
}
exec function TeleportRotate_NPC(id:int, xp:int, yp:int, zp:int, xr:int, yr:int, zr:int)
{
    var data : MyModData;
    data = GetServerSettings();
    _TeleportRotate(data.NpcEntities[id], Vector(xp, yp, zp), EulerAngles(xr, yr, zr));
    if(data.DebugMode)
        Log("[WITCHER3MP] TeleportedTOAndRotNPC: " + VecToString(Vector(xp, yp, zp)) + " \nRotation " + "X:" + xr + " Y:" + yr + " Z" + zr);
}
exec function SetMoveTo_NPC(id:int, xp:int, yp:int, zp:int)//agen set
{
    var data : MyModData;
    data = GetServerSettings();
    _SetMoveTo(data.NpcEntities[id], MT_Run, Vector(xp, yp, zp));
    if(data.DebugMode)
        Log("[WITCHER3MP] SetMoveTOPlayer: " + VecToString(Vector(xp, yp, zp)));
}
exec function Rotate_NPC(id:int, xp:int, yp:int, zp:int)//agen set
{
    var data : MyModData;
    data = GetServerSettings();
    _SetRotateTo(data.NpcEntities[id], Vector(xp, yp, zp));
    if(data.DebugMode)
        Log("[WITCHER3MP] Setted RotationNPC " + "X:" + xp + " Y:" + yp + " Z" + zp);
}
//SET TARGET COMBAT
exec function SetCombatTargetMP(id:int, id2:int){
    var data: MyModData;
    data = GetServerSettings();
    _SetCombatTarget(data.PlayersEntities[id], data.NpcEntities[id2]);
    if(data.DebugMode)
        Log("[WITCHER3MP] Setted npc " + id + " |TargetToHit: " + id2);
}