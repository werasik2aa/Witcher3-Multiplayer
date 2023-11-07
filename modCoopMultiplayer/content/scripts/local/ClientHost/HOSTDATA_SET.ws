exec function SetHostDebugMode(safa : int)
{
    var data : Storage;
    data = getStorage();
    data.DebugMode = (safa == 1);
    Log("[WITCHER3MP] Debug Enabled: " + data.DebugMode);
}
exec function SetMaxPlayers(a:int)
{
    var data : Storage;
    data = getStorage();
    data.MaxPlayers = a;
    if(getStorage().DebugMode)
        Log("[WITCHER3MP] Succesfully changed MaxPlayers TO: " + a);
}
exec function SetCurPlayers(a:int)
{
    var data : Storage;
    data = getStorage();
    data.CurPlayers = a;
    if(getStorage().DebugMode)
        Log("[WITCHER3MP] Succesfully changed MaxPlayers TO: " + a);
}
exec function OnDisconnect(clid:int)
{
    var data : Storage;
    data = getStorage();
    data.removePlayer(clid);
    GetWitcherPlayer().DisplayHudMessage("PlayerID: " + clid + " Disconnected from game");
}
exec function SetPlayerCurrentHP(clid:int, health:int)
{
    var data : Storage;
    var entity : CNewNPC;
    data = getStorage();
    entity = data.getPlayerOBJ(clid);
    entity.SetHealthPerc(health);
    if(entity.IsAlive() && health <= 0) entity.SetAlive(false);
    if(!entity.IsAlive() && health > 0) entity.SetAlive(true);
}
exec function SetAlivePOBJ(clid:int)
{
    var data : Storage;
    var entity : CNewNPC;
    data = getStorage();
    entity = data.getPlayerOBJ(clid);
    if(entity.IsAlive() && (int)entity.GetCurrentHealth() <= 0) entity.SetAlive(false);
    if(!entity.IsAlive() && (int)entity.GetCurrentHealth() > 0) entity.SetAlive(true);
}
exec function SetPlayerCurrentLevel(clid:int, lvl:int)
{
    var data : Storage;
    var entity : CNewNPC;
    data = getStorage();
    entity = data.getPlayerOBJ(clid);
	entity.SetLevel(lvl);
}
exec function KillNearEntityByPosition(xp:int, yp:int, zp:int) //KILL Actors In position
{
    var actors : array<CActor>;
    var actor	: CActor;
    var i : int;
    var action : W3DamageAction;
    var entities : array<CGameplayEntity>;
    FindGameplayEntitiesInSphere(entities, Vector(xp, yp, zp), 10, 1000000, '', FLAG_OnlyAliveActors);
    ArrayOfActorsAppendArrayOfGameplayEntities(actors, entities);
    for( i = 0; i < actors.Size(); i+= 1 )
    {
        actor = actors[i];
        actor.SignalGameplayEvent('Death');
	    actor.SetAlive(false);
	    actor.OnDeath(action);
    }
    if(getStorage().DebugMode)
        Log("[WITCHER3MP] Got Near In pos: " + VecToString(Vector(xp, yp, zp)));
}