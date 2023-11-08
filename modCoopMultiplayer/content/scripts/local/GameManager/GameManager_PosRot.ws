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
    var mvADJ : CMovementAdjustor; 
    var ticket : SMovementAdjustmentRequestTicket;
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
function _MoveEntity(entity:CNewNPC, targpos:Vector, SpeedOnDist:int, MinDistance:int, MaxDistance:int, DirectCtrl:bool)
{
    var dist : float;
    var entpos : Vector;
    var mvADJ : CMovementAdjustor; 
    var ticket : SMovementAdjustmentRequestTicket; 
    entpos = entity.GetWorldPosition();
    dist = VecDistance(entpos, targpos); //Than far - then faster.
    mvADJ = entity.GetMovingAgentComponent().GetMovementAdjustor();
    if(SpeedOnDist > 0) dist = SpeedOnDist;//SET CUSTOM SPEED

    if(!DirectCtrl)
        _SetHeading(entity, targpos); //SET heading
    _SetTypeMove(entity, (int)(dist*2));

    if(dist > MinDistance && DirectCtrl) //MOVE HERE
    {
        mvADJ.Cancel(mvADJ.GetRequest('W3MP_MOVEMENT'));
        ticket = mvADJ.CreateNewRequest('W3MP_MOVEMENT');
        mvADJ.AdjustmentDuration(ticket, 0.5);
        mvADJ.ShouldStartAt(ticket, entity.GetWorldPosition());
        mvADJ.MaxRotationAdjustmentSpeed(ticket, 10000);
        mvADJ.MaxLocationAdjustmentSpeed(ticket, dist * 2);
        mvADJ.AdjustLocationVertically(ticket, true);
        mvADJ.ScaleAnimationLocationVertically(ticket, true);
        mvADJ.RotateTo(ticket, VecHeading(targpos - entity.GetWorldPosition()));
        mvADJ.SlideTo(ticket, targpos);
    }
    if(dist > MaxDistance) // TELEPORT IF hight range between object
        _Teleport(entity, targpos);
}
function _SetTypeMove(entity:CNewNPC, st:int)
{
  ((CActor)entity).GetMovingAgentComponent().SetGameplayRelativeMoveSpeed(st); //0 stay in place. 1 is for Walking, 2 jogging, 4 sprinting.
  if(getStorage().DebugMode)
        Log("[WITCHER3MP] SetTypeMove: " + st + " ofEntityGUID: " + entity.GetGuidHash());
}