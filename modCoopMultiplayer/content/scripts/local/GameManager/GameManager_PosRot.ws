//exec
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
//FUNCTIONS
function _SetHeading(entity:CNewNPC, targpos:Vector)
{
  ((CActor)entity).GetMovingAgentComponent().SetGameplayMoveDirection(VecHeading(targpos - entity.GetWorldPosition()));
  if(getStorage().DebugMode)
        Log("[WITCHER3MP] Setted Heading: " + VecToString(targpos - entity.GetWorldPosition()));
}
function _Teleport(entity:CNewNPC, pos:Vector)
{
  ((CActor)entity).Teleport(pos);
  if(getStorage().DebugMode)
        Log("[WITCHER3MP] Teleported TO: " + VecToString(pos));
}
function _MoveEntity(Entity:CNewNPC, targpos:Vector, speed:int, MinDistance:int, MaxDistance:int)
{
    var dist:float;
    var entpos:Vector;
    entpos = Entity.GetWorldPosition();
    _SetHeading(Entity, targpos);
    dist = VecDistance(entpos, targpos);
    if(dist > MinDistance)
        if(speed < 0) Entity.SetBehaviorVariable('Editor_MoveSpeed', -(dist*speed));
        else Entity.SetBehaviorVariable('Editor_MoveSpeed', speed);
    else 
        Entity.SetBehaviorVariable('Editor_MoveSpeed', 0);
    if(dist > MaxDistance)
        _Teleport(Entity, targpos);
}
function _SetTypeMove(entity:CNewNPC, st:int)
{
  ((CActor)entity).GetMovingAgentComponent().SetGameplayRelativeMoveSpeed(st); //0 stay in place. 1 is for Walking, 2 jogging, 4 sprinting.
  if(getStorage().DebugMode)
        Log("[WITCHER3MP] SetTypeMove: " + st + " ofEntityGUID: " + entity.GetGuidHash());
}