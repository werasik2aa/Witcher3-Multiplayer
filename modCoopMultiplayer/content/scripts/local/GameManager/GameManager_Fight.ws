//EXTERNAL EXEC
exec function Attack(idcl:int, attackint:int, health:int)
{
    var data : Storage;
    var entity : CNewNPC;
    var entity2 : CNewNPC;
    data = getStorage();
    entity = data.getPlayerOBJ(idcl);
    entity2 = (CNewNPC)entity.GetTarget();
    if(attackint != 0 && health > 0) { //ATTACK NEARAREST ENTITY ?FORWARD?
        entity2 = _GetNearTarget(entity);
        if(_IsOnHorse(entity))
            ((CActor)entity).SetBehaviorVariable('actionType', attackint);
        else
            ((CActor)entity).SetBehaviorVariable('combatActionType', attackint);
        _SetCombatTarget(entity, entity2);//SetTarg
    }
    else
        _ResetCombatTarget(entity, entity2);
    _CalculateHealthEnemy(entity2, health);
    if(data.DebugMode)
        Log("[WITCHER3MP] AttackID: " + attackint + " PlayerID: " + idcl + " EntityGUID: " + entity.GetGuidHash());
}
exec function ResetCurrentCombatTarget(idcl:int)
{
    var data : Storage;
    var entity : CNewNPC;
    var entity2 : CNewNPC;
    data = getStorage();
    entity = data.getPlayerOBJ(idcl);
    entity2 = (CNewNPC)entity.GetTarget();
    _ResetCombatTarget(entity, entity2);
    if(data.DebugMode)
        Log("[WITCHER3MP] PlayerID: " + idcl + " EntityGUID: " + entity.GetGuidHash());
}
exec function KillEntity(idcl:int)
{
    var data : Storage;
    var entity : CActor;
    var action : W3DamageAction;
    data = getStorage();
    entity = data.getPlayerOBJ(idcl).GetTarget();
    entity.SignalGameplayEvent('Death');
	entity.SetAlive(false);
	entity.OnDeath(action);
    if(data.DebugMode)
        Log("[WITCHER3MP] Kill Entity Name: " + (CActor)entity + " GUID: " + entity.GetGuidHash());
}
//FUNCTIONS
function _CalculateHealthEnemy(entity:CNewNPC, ClientSHealth:int)
{
    var data : Storage;
    data = getStorage();
    if(thePlayer.GetTarget().GetGuidHash() == entity.GetGuidHash() && thePlayer.IsInCombat()) // IF PLAYER 1, 2,.. OTHER ATTACK ONE OBJECT
        ((CActor)entity).SetHealthPerc(entity.GetCurrentHealth() - ClientSHealth/entity.GetCurrentHealth());
    else
        ((CActor)entity).SetHealthPerc(ClientSHealth); //If one play target one object
}
function _SetCombatTarget(entity:CNewNPC, entity2:CNewNPC)
{
  var dist:float;
  dist = VecDistance(entity.GetWorldPosition(), entity2.GetWorldPosition());
  if(dist >= 10) 
    _ResetCombatTarget(entity, entity2); //RESET IF IT FAR
  else {
    if (dist > 2) _Teleport(entity, entity2.GetWorldPosition()); //TELEPORT SOME FORWARD FOR ATTACK ACTION
    ((CActor)entity).SetAttitude((CActor)entity2, AIA_Hostile); //ATTIDUED FOR FIGHT
  }
  if(getStorage().DebugMode)
        Log("[WITCHER3MP] Set Hostile Attitude to: " + entity2.GetGuidHash());
}
function _ResetCombatTarget(entity:CNewNPC, entity2:CNewNPC)
{
  ((CActor)entity).ResetAttitude(entity2);//RESET ATTITUDE
  ((CActor)entity).SetAttitude((CActor)entity2, AIA_Friendly); //SET IT FRIENDLY I THINK THIS BETTER
  if(getStorage().DebugMode)
        Log("[WITCHER3MP] Set Friendly Attitude to: " + entity2.GetGuidHash());
}
function _GetNearTarget(entity:CNewNPC) : CNewNPC //NEAR TARGET IN RANGER
{
    var actors : array<CActor>;
    var actor	: CActor;
    var prevdist : float;
    var dist : float;
    var i : int;
    prevdist = 99;
    actors = GetActorsInRange(entity, 100);
    for( i = 0; i < actors.Size(); i+= 1 )
    {
        dist = VecDistance(actors[i].GetWorldPosition(), entity.GetWorldPosition());
        if(dist < prevdist && actors[i] != entity && actors[i] != thePlayer)
        {
          actor = actors[i];
          prevdist = dist;
        }
  }
  if(getStorage().DebugMode)
        Log("[WITCHER3MP] Got Near NPC Distance: " + dist);
  return (CNewNPC)actor;
}