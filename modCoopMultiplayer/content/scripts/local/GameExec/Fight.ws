exec function KillEntityByID(id:int)
{
    var data : MP_NPCSTORAGE;
    var entity : CActor ;
    var action : W3DamageAction;
    data = getStorage().getNPCStorage();
    entity = data.GetByID(id).Entity;
    _KillNPC(entity);
    if(getStorage().DebugMode)
        Log("[WITCHER3MP] Kill Entity Name: " + (CActor)entity + " GUID: " + id);
}
exec function SetTargetByID(idcl:int, idnp:int)
{
    var data: MP_NPCSTORAGE;
    data = getStorage().getPLAYERStorage();
    if(data.GetByID(idcl).ID != data.GetByID(idnp).ID && data.GetByID(idcl).FightID > 0)
        _SetCombatTarget(data.GetByID(idcl).Entity, data.GetByID(idnp).Entity);

    if(getStorage().DebugMode)
        Log("[WITCHER3MP] PlayerID: " + idcl + " EntityGUID: " + idnp);
}
exec function SetFightStateByID(idcl:int, attackint:int)
{
    var dataP : MP_NPCSTORAGE;

    dataP = getStorage().getPLAYERStorage();
    dataP.SetByIDFightID(idcl, attackint);

    if(getStorage().DebugMode)
        Log("[WITCHER3MP] PlayerID: " + idcl + " AttackID: " + attackint);
}
exec function DamageEntity(idnp:int, ClientSHealth:int)
{
    var dataN : MP_NPCSTORAGE;
    var entity : CActor ;
    dataN = getStorage().getNPCStorage();
    entity = dataN.GetByID(idnp).Entity;
    ((CActor)entity).DrainVitality(entity.GetCurrentHealth() - ClientSHealth); //Percent of client healthenemy and Get value of damage
    if(getStorage().DebugMode)
        Log("[WITCHER3MP] Damage: " + (entity.GetCurrentHealth() - ClientSHealth) + " :ClientHP: " + ClientSHealth + "ServerHP:" + entity.GetCurrentHealth());
}