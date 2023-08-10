//SET TARGET COMBAT
exec function Attack(idcl:int, attackint:int, GuidHash:int)
{
    var data : Storage;
    var entity : CNewNPC;
    var entityAT : CNewNPC;
    var daction : EBufferActionType;
    data = getStorage();
    entity = data.getPlayerOBJ(idcl);
    if(GuidHash != 0 && attackint != 0) {
        entityAT = data.getNPC(GuidHash);
        if(entity.IsUsingHorse()){
            if(attackint == 1)
                ((CActor)entity).SetBehaviorVariable( 'actionType', (int)EHCA_Attack );
            if(attackint == 2)
                ((CActor)entity).SetBehaviorVariable( 'actionType', (int)EHCA_ShootCrossbow );
            if(attackint == 3)
                ((CActor)entity).SetBehaviorVariable( 'actionType', (int)EHCA_ShootCrossbow );
            if(attackint == 4)
               ((CActor)entity).SetBehaviorVariable( 'actionType', (int)EHCA_CastSign );
            if(attackint == 5)
                ((CActor)entity).SetBehaviorVariable( 'actionType', (int)EHCA_ThrowBomb );
        } else {
            if(attackint == 0)
                ((CActor)entity).SetBehaviorVariable('combatActionType', (int)(daction = EBAT_EMPTY));
            if(attackint == 1)
                ((CActor)entity).SetBehaviorVariable('combatActionType', (int)(daction = EBAT_LightAttack));
            if(attackint == 2)
                ((CActor)entity).SetBehaviorVariable('combatActionType', (int)(daction = EBAT_HeavyAttack));
            if(attackint == 3)
                ((CActor)entity).SetBehaviorVariable('combatActionType', (int)(daction = EBAT_SpecialAttack_Light));
            if(attackint == 4)
                ((CActor)entity).SetBehaviorVariable('combatActionType', (int)(daction = EBAT_SpecialAttack_Heavy));
            if(attackint == 5)
                ((CActor)entity).SetBehaviorVariable('combatActionType', (int)(daction = EBAT_Parry));
            if(attackint == 6)
                ((CActor)entity).SetBehaviorVariable('combatActionType', (int)(daction = EBAT_Dodge));
        }
        _SetCombatTarget(entity, entityAT);
    }
    else
        _ResetCombatTarget(entity);
}