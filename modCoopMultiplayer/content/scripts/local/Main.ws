latent function ZaLoopa()
{
    var i:int;
    var dataN: MP_NPCSTORAGE;
    var dataP: MP_NPCSTORAGE;
    var Entity, EntityH: CActor ;
    var combatDataStorage : CExtendedAICombatStorage;

    dataN = getStorage().getNPCStorage();
    dataP = getStorage().getPLAYERStorage();
    
    for (i=0; i < dataP.Entity.Size(); i+=1)
    {
        //DEFINE
        Entity = dataP.GetByIndex(i).Entity;
        EntityH = dataP.GetByIndex(i).H_Entity;

        //POSITION SET _MoveEntity(entity, pos, speed, mindist, maxdist, rot, directCtrl)
        _MoveEntity(Entity, dataP.GetByIndex(i).Position, 0, 2, 3, dataP.GetByIndex(i).Angle, true);
        _MoveEntity(EntityH, dataP.GetByIndex(i).H_Position, 0, 2, 3, dataP.GetByIndex(i).H_Angle, true);

        //FIGHT ID STATE
        if(_IsOnHorse(Entity))
            Entity.SetBehaviorVariable('actionType', dataP.GetByIndex(i).FightID);
        else
            Entity.SetBehaviorVariable('combatActionType', dataP.GetByIndex(i).FightID);

        //FIGHT TARGET
        if(dataP.GetByIndex(i).FightID == 0)
            _ResetCombatTarget(Entity, (CActor)Entity.GetTarget());
        else {
            combatDataStorage.SetIsAttacking(true, 0.1f);
            theGame.GetBehTreeReactionManager().CreateReactionEventIfPossible(Entity, 'NpcAttackAction', 10.0, 15.0f, -1, -1, true);
        }
    }
}
exec function StartMP(templa:string) {
    var data: MP_NPCSTORAGE;
    var ents: array<CActor>;
    var i: int;
    data = getStorage().getPLAYERStorage();
    data.Clear();
    _SwitchCharacter((CActor)thePlayer, templa);
    ents = _GetNearTargets((CActor)thePlayer, 1000);
    for (i=0; i < ents.Size(); i+=1)
        getStorage().getNPCStorage().Add(ents[i], NULL, "FromStart", ents[i]);
    //FillNPC LIST Which near(bug solve?)
}