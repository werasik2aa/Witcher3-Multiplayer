function _KillNPC(Emtity:CActor )
{
    var action : W3DamageAction;
    ((CActor)Emtity).SignalGameplayEvent('Death');
	((CActor)Emtity).SetAlive(false);
	((CActor)Emtity).OnDeath(action);
}
function _ResetCombatTarget(entity:CActor , entity2:CActor )
{
    ((CActor)entity).ResetAttitude(entity2);//RESET ATTITUDE
    ((CActor)entity).SetAttitude((CActor)entity2, AIA_Friendly); //SET IT FRIENDLY I THINK THIS BETTER
    if(getStorage().DebugMode)
        Log("[WITCHER3MP] Set Friendly Attitude to: " + entity2.GetGuidHash());
}
function _SetCombatTarget(entity:CActor , entity2:CActor )
{
    var dist:float;
    dist = VecDistance(entity.GetWorldPosition(), entity2.GetWorldPosition());
    ((CActor)entity).SetAttitude((CActor)entity2, AIA_Hostile); //ATTIDUED FOR FIGHT
    if(getStorage().DebugMode)
        Log("[WITCHER3MP] Set Hostile Attitude to: " + entity2.GetGuidHash());
}