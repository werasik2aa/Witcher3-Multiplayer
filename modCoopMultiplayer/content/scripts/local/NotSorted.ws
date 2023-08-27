exec function eforceaniv2(idcl:int)
{
  var l_aiTree : CAIExecuteAttackAction;
  var attack_anim_names : array<EAttackType>;
  var ent: CNewNPC;
  var data : Storage;
  data = getStorage();
  ent = data.getPlayerOBJ(idcl);
  l_aiTree = new CAIExecuteAttackAction in ((CActor)ent);
  l_aiTree.OnCreated();
  attack_anim_names.Clear();
  attack_anim_names.PushBack(EAT_Attack1);
  attack_anim_names.PushBack(EAT_Attack2);
  attack_anim_names.PushBack(EAT_Attack3);
  attack_anim_names.PushBack(EAT_Attack4);
  attack_anim_names.PushBack(EAT_Attack5);
  attack_anim_names.PushBack(EAT_Attack6);
  l_aiTree.attackParameter = attack_anim_names[RandRange(attack_anim_names.Size())];
  ((CActor)ent).ForceAIBehavior(l_aiTree, BTAP_AboveCombat2);
  theGame.GetBehTreeReactionManager().CreateReactionEventIfPossible(((CActor)ent), 'AttackAction', 1.0, 1.0f, 999.0f, 1, true);
}
exec function eforceani(animation_name: name, idcl:int)
{
    var actor : CActor; 
    var enemyAnimatedComponent : CAnimatedComponent;
    var data : Storage;
    data = getStorage();
    actor = (CActor)data.getPlayerOBJ(idcl);
    enemyAnimatedComponent = (CAnimatedComponent)actor.GetComponentByClassName('CAnimatedComponent');
    enemyAnimatedComponent.PlaySlotAnimationAsync( animation_name, 'NPC_ANIM_SLOT', SAnimatedComponentSlotAnimationSettings(0.25f, 0.875f));
}
