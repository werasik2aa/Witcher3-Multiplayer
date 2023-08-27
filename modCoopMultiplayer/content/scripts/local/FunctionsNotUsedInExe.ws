function _SpawnIT(nameenti:string, npc_templ:string, position:Vector, fullname:bool) : CNewNPC
{
    var template: CEntityTemplate;
    var entity: CNewNPC;
    var data : Storage;
    var tags : array<CName>;
    data = getStorage();
    template = (CEntityTemplate)LoadResource(npc_templ, fullname);
    entity = (CNewNPC)theGame.CreateEntity(template, position,EulerAngles(0, 0, 0),,,,PM_DontPersist, tags);
    ((CActor)entity).SetTemporaryAttitudeGroup('q104_avallach_friendly_to_all', AGP_Default);
    onelinerEntity(nameenti, entity);//nickname
    if(npc_templ == "horse")
    {
      ((CActor)entity).ForceAIBehavior(new CAIHorseDoNothingAction in entity, BTAP_AboveEmergency2, 'AI_Rider_Load_Forced' );
			((CActor)entity).SignalGameplayEventParamInt('RidingManagerMountHorse', MT_instant | MT_fromScript );
    }
    if(getStorage().DebugMode)
        Log("[WITCHER3MP] Spawned Player OBJ: " + entity.GetGuidHash() + " TMPLATE: " + npc_templ + " Name: " + nameenti);
    return entity;
}
function _SpawnITNPC(npc_templ:string, position:Vector, fullname:bool) : CNewNPC
{
    var template: CEntityTemplate;
    var entity: CNewNPC;
    var data : Storage;
    var tags : array<CName>;
    data = getStorage();
    template = (CEntityTemplate)LoadResource(npc_templ, fullname);
    entity = (CNewNPC)theGame.CreateEntity(template, position,EulerAngles(0, 0, 0),,,,PM_DontPersist, tags);
    ((CActor)entity).SetTemporaryAttitudeGroup('q104_avallach_friendly_to_all', AGP_Default);
    entity.SetBehaviorVariable('isOnHorse', 0.0);
    if(getStorage().DebugMode)
        Log("[WITCHER3MP] SpawnedNPC: " + entity.GetGuidHash() + " TMPLATE: " + npc_templ);
    return entity;
}
function _SpawnITNPCENEMY(npc_templ:string, position:Vector, fullname:bool) : CNewNPC
{
    var template: CEntityTemplate;
    var entity: CNewNPC;
    var data : Storage;
    var tags : array<CName>;
    data = getStorage();
    template = (CEntityTemplate)LoadResource(npc_templ, fullname);
    entity = (CNewNPC)theGame.CreateEntity(template, position,EulerAngles(0, 0, 0),,,,PM_DontPersist, tags);
    ((CActor)entity).SetTemporaryAttitudeGroup('q104_avallach_friendly_to_all', AGP_Default);
    if(getStorage().DebugMode)
        Log("[WITCHER3MP] SpawnedNPC Enemy: " + entity.GetGuidHash() + " TMPLATE: " + npc_templ);
    return entity;
}
function _SpawnITCORPSE(npc_templ:string, position:Vector) : CNewNPC
{
  var template: CEntityTemplate;
  var entity: CNewNPC;
  var data : Storage;
  var tags : array<CName>;
  var action : W3DamageAction;
  data = getStorage();
  template = (CEntityTemplate)LoadResource(npc_templ, false);
  entity = (CNewNPC)theGame.CreateEntity(template, position,EulerAngles(0, 0, 0),,,,PM_DontPersist, tags);
  ((CActor)entity).SignalGameplayEvent( 'Death' );
	((CActor)entity).SetAlive(false);
	((CActor)entity).OnDeath(action);
  if(getStorage().DebugMode)
        Log("[WITCHER3MP] Spawned Entity Corpse! TMPLATE: " + npc_templ);
  return entity;
}
function _SetHeading(entity:CNewNPC, targpos:Vector)
{
  //((CActor)entity).SetRotationTargetPos(targpos);
  ((CActor)entity).GetMovingAgentComponent().SetGameplayMoveDirection(VecHeading(targpos));
  if(getStorage().DebugMode)
        Log("[WITCHER3MP] Setted Heading: " + VecToString(targpos));
}
function _Teleport(entity:CNewNPC, pos:Vector)
{
  ((CActor)entity).Teleport(pos);
  if(getStorage().DebugMode)
        Log("[WITCHER3MP] Teleported TO: " + VecToString(pos));
}
function _SetTypeMove(entity:CNewNPC, st:int)
{
  ((CActor)entity).GetMovingAgentComponent().SetGameplayRelativeMoveSpeed(st); //0 stay in place. 1 is for Walking, 2 jogging, 4 sprinting.
  if(getStorage().DebugMode)
        Log("[WITCHER3MP] SetTypeMove: " + st + " ofEntityGUID: " + entity.GetGuidHash());
}
function _SetTypeMoveHorse(entity:CNewNPC, st:int)
{
  entity.GetHorseComponent().SetHorseWalkState(st); //0 stay in place. 1 is for Walking, 2 running.
}
function _IsOnHorse(entity:CNewNPC) : bool
{
  return ((CActor)entity).HasTag('MYisOnHorsetrue');
}
//HOST OPERA
function _Data_AddNPC(e:CNewNPC, guid:int)
{
    var data : Storage;
    data = getStorage();
    e.AddTag('W3MP');
    data.NpcEntities.PushBack(e);
    if(getStorage().DebugMode)
        Log("[WITCHER3MP] Succesfully Added NPC Entity to list");
}
function _SetupPlayerObjectDefault(entity:CNewNPC) : CNewNPC //Add Swords to back of PlayerObj :TODO: CHANGE THE WEARING
{
	var ids : array<SItemUniqueId>;
	var inv : CInventoryComponent;
  var data : Storage;
  data = getStorage();
  inv = entity.GetInventory();
	ids = inv.AddAnItem('Autogen steel sword', 1); entity.EquipItem(ids[0]);
	ids = inv.AddAnItem('Autogen silver sword', 1); entity.EquipItem(ids[0]);
	ids = inv.AddAnItem('Autogen Pants', 1); entity.EquipItem(ids[0]);
	ids = inv.AddAnItem('Autogen Gloves', 1); entity.EquipItem(ids[0]);
	ids = inv.AddAnItem('Autogen Boots', 1); entity.EquipItem(ids[0]);
	ids = inv.AddAnItem('Autogen Armor', 1); entity.EquipItem(ids[0]);
  return entity;
}
function _MountWearing(entity:CNewNPC, citem:name)
{
  var ids : array<SItemUniqueId>;
	var inv : CInventoryComponent;
  var data : Storage;
  data = getStorage();
  inv = entity.GetInventory();
  ids = inv.AddAnItem(citem, 1); entity.EquipItem(ids[0]);
}
function _SwitchCharacter(charatempl:string)
{
	var meshes : array<CComponent>;
	var mesh : CMeshComponent;
	var i : int;
	var appcomp : CComponent;
	var inv : CInventoryComponent;
	var Itemids : array<SItemUniqueId>;
	var item: SItemUniqueId;
  var itemExcept: array<SItemUniqueId>;
	var data: Storage;
	data = getStorage();
  inv = thePlayer.GetInventory();
	inv.GetAllItems(Itemids);
	inv.GetItemEquippedOnSlot( EES_SilverSword, item ); itemExcept.PushBack(item);
	inv.GetItemEquippedOnSlot( EES_SteelSword, item ); itemExcept.PushBack(item);

  inv.GetItemEquippedOnSlot( EES_Armor, item); itemExcept.PushBack(item);
	inv.GetItemEquippedOnSlot( EES_Boots, item ); itemExcept.PushBack(item);
  inv.GetItemEquippedOnSlot( EES_Pants, item ); itemExcept.PushBack(item);
  inv.GetItemEquippedOnSlot( EES_Gloves, item ); itemExcept.PushBack(item);

	if( Itemids.Size() > 0 )
	{
		for( i = 0; i < Itemids.Size(); i+=1 )
		{
			if(!itemExcept.Contains(Itemids[i]))
				if(inv.IsItemMounted(Itemids[i]))
					inv.DespawnItem(Itemids[i]);
		}	
	}
	meshes = thePlayer.GetComponentsByClassName('CMeshComponent');
	appcomp = thePlayer.GetComponentByClassName('CAppearanceComponent');
	if(meshes.Size() > 0)
	{
		for(i=0; i < meshes.Size(); i+=1)
		{
			mesh = (CMeshComponent) meshes[i];
			mesh.SetVisible(false);
		}
	}
	if(charatempl == "characters\npc_entities\main_npc\eskel.w2ent")
	{
		((CAppearanceComponent)appcomp).IncludeAppearanceTemplate((CEntityTemplate)LoadResource("characters\models\secondary_npc\eskel\s_01_ma__eskel.w2ent", true));
		((CAppearanceComponent)appcomp).IncludeAppearanceTemplate((CEntityTemplate)LoadResource("characters\models\secondary_npc\eskel\h_01_ma__eskel.w2ent", true));
		((CAppearanceComponent)appcomp).IncludeAppearanceTemplate((CEntityTemplate)LoadResource("characters\models\secondary_npc\eskel\body_01_ma__eskel.w2ent", true));
	} else
	if(charatempl == "characters\npc_entities\main_npc\lambert.w2ent")
	{
		((CAppearanceComponent)appcomp).IncludeAppearanceTemplate((CEntityTemplate)LoadResource("characters\models\secondary_npc\lambert\h_01_ma__lambert.w2ent", true));
		((CAppearanceComponent)appcomp).IncludeAppearanceTemplate((CEntityTemplate)LoadResource("characters\models\secondary_npc\lambert\body_01_ma__lambert.w2ent", true));
		((CAppearanceComponent)appcomp).IncludeAppearanceTemplate((CEntityTemplate)LoadResource("characters\models\secondary_npc\lambert\s_01_ma__lambert.w2ent", true));
	} else
	if(charatempl == "characters\npc_entities\main_npc\vesemir.w2ent")
	{
		((CAppearanceComponent)appcomp).IncludeAppearanceTemplate((CEntityTemplate)LoadResource("characters\models\main_npc\vesemir\c_01_mb__vesemir.w2ent", true));
		((CAppearanceComponent)appcomp).IncludeAppearanceTemplate((CEntityTemplate)LoadResource("characters\models\main_npc\vesemir\h_01_mb__vesemir.w2ent", true));
		((CAppearanceComponent)appcomp).IncludeAppearanceTemplate((CEntityTemplate)LoadResource("characters\models\main_npc\vesemir\body_01__vesemir.w2ent", true));
	} else
	if(charatempl == "characters\npc_entities\main_npc\zoltan.w2ent")
	{
		((CAppearanceComponent)appcomp).IncludeAppearanceTemplate((CEntityTemplate)LoadResource("characters\models\main_npc\zoltan\h_01_da__zoltan.w2ent", true));
		((CAppearanceComponent)appcomp).IncludeAppearanceTemplate((CEntityTemplate)LoadResource("characters\models\main_npc\zoltan\body_01__zoltan.w2ent", true));
		((CAppearanceComponent)appcomp).IncludeAppearanceTemplate((CEntityTemplate)LoadResource("characters\models\common\dwarf_average\body\g_01_da__body.w2ent", true));
	} else
	if(charatempl == "characters\npc_entities\secondary_npc\letho.w2ent")
	{
		((CAppearanceComponent)appcomp).IncludeAppearanceTemplate((CEntityTemplate)LoadResource("characters\models\main_npc\letho\h_01_ma__letho.w2ent", true));
		((CAppearanceComponent)appcomp).IncludeAppearanceTemplate((CEntityTemplate)LoadResource("characters\models\main_npc\letho\body_01__letho.w2ent", true));
	} else
	if(charatempl == "characters\npc_entities\main_npc\roche.w2ent")
	{
		((CAppearanceComponent)appcomp).IncludeAppearanceTemplate((CEntityTemplate)LoadResource("characters\models\main_npc\roche\c_01__roche.w2ent", true));
		((CAppearanceComponent)appcomp).IncludeAppearanceTemplate((CEntityTemplate)LoadResource("characters\models\main_npc\roche\h_01_ma__roche.w2ent", true));
		((CAppearanceComponent)appcomp).IncludeAppearanceTemplate((CEntityTemplate)LoadResource("characters\models\main_npc\roche\body_01__roche.w2ent", true));
	} else { //standart
		((CAppearanceComponent)appcomp).IncludeAppearanceTemplate((CEntityTemplate)LoadResource("characters\models\main_npc\vesemir\c_01_mb__vesemir.w2ent", true));
		((CAppearanceComponent)appcomp).IncludeAppearanceTemplate((CEntityTemplate)LoadResource("characters\models\main_npc\vesemir\h_01_mb__vesemir.w2ent", true));
		((CAppearanceComponent)appcomp).IncludeAppearanceTemplate((CEntityTemplate)LoadResource("characters\models\main_npc\vesemir\body_01__vesemir.w2ent", true));
	}
	if(data.DebugMode)
        Log("[WITCHER3MP] Player Model Changed to match for: " + charatempl);
}
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
