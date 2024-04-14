function _SwitchCharacter(Entity : CActor, charatempl:string)
{
	var meshes : array<CComponent>;
	var i : int;
	var appcomp : CAppearanceComponent;
	var inv : CInventoryComponent;
	var Itemids : array<SItemUniqueId>;
	var item: SItemUniqueId;
    var itemExcept: array<SItemUniqueId>;
	var data: MP_NPCSTORAGE;
    inv = Entity.GetInventory();
	meshes = Entity.GetComponentsByClassName('CMeshComponent');
	appcomp = (CAppearanceComponent)Entity.GetComponentByClassName('CAppearanceComponent');

	inv.GetAllItems(Itemids);
	inv.GetItemEquippedOnSlot( EES_SilverSword, item ); itemExcept.PushBack(item);
	inv.GetItemEquippedOnSlot( EES_SteelSword, item ); itemExcept.PushBack(item);
   	inv.GetItemEquippedOnSlot( EES_Armor, item); itemExcept.PushBack(item);
	inv.GetItemEquippedOnSlot( EES_Boots, item ); itemExcept.PushBack(item);
   	inv.GetItemEquippedOnSlot( EES_Pants, item ); itemExcept.PushBack(item);
   	inv.GetItemEquippedOnSlot( EES_Gloves, item ); itemExcept.PushBack(item);
	if( Itemids.Size() > 0 )
		for( i = 0; i < Itemids.Size(); i+=1 )
			if(!itemExcept.Contains(Itemids[i]))
				if(inv.IsItemMounted(Itemids[i]))
					inv.DespawnItem(Itemids[i]);
	if(meshes.Size() > 0)
		for(i=0; i < meshes.Size(); i+=1)
			((CMeshComponent)meshes[i]).SetVisible(false);
	
	if(charatempl == "characters\npc_entities\main_npc\eskel.w2ent")
	{
		appcomp.IncludeAppearanceTemplate((CEntityTemplate)LoadResource("characters\models\secondary_npc\eskel\s_01_ma__eskel.w2ent", true));
		appcomp.IncludeAppearanceTemplate((CEntityTemplate)LoadResource("characters\models\secondary_npc\eskel\h_01_ma__eskel.w2ent", true));
		appcomp.IncludeAppearanceTemplate((CEntityTemplate)LoadResource("characters\models\secondary_npc\eskel\body_01_ma__eskel.w2ent", true));
	} else if(charatempl == "characters\npc_entities\main_npc\vesemir.w2ent")
	{
			appcomp.IncludeAppearanceTemplate((CEntityTemplate)LoadResource("characters\models\main_npc\vesemir\c_01_mb__vesemir.w2ent", true));
		appcomp.IncludeAppearanceTemplate((CEntityTemplate)LoadResource("characters\models\main_npc\vesemir\h_01_mb__vesemir.w2ent", true));
		appcomp.IncludeAppearanceTemplate((CEntityTemplate)LoadResource("characters\models\main_npc\vesemir\body_01__vesemir.w2ent", true));
	} else if(charatempl == "characters\npc_entities\main_npc\zoltan.w2ent")
	{
		appcomp.IncludeAppearanceTemplate((CEntityTemplate)LoadResource("characters\models\main_npc\zoltan\h_01_da__zoltan.w2ent", true));
		appcomp.IncludeAppearanceTemplate((CEntityTemplate)LoadResource("characters\models\main_npc\zoltan\body_01__zoltan.w2ent", true));
		appcomp.IncludeAppearanceTemplate((CEntityTemplate)LoadResource("characters\models\common\dwarf_average\body\g_01_da__body.w2ent", true));
	} else if(charatempl == "characters\npc_entities\secondary_npc\letho.w2ent")
	{
		appcomp.IncludeAppearanceTemplate((CEntityTemplate)LoadResource("characters\models\main_npc\letho\h_01_ma__letho.w2ent", true));
		appcomp.IncludeAppearanceTemplate((CEntityTemplate)LoadResource("characters\models\main_npc\letho\body_01__letho.w2ent", true));
	}else if(charatempl == "characters\npc_entities\main_npc\roche.w2ent")
	{
		appcomp.IncludeAppearanceTemplate((CEntityTemplate)LoadResource("characters\models\main_npc\roche\c_01__roche.w2ent", true));
		appcomp.IncludeAppearanceTemplate((CEntityTemplate)LoadResource("characters\models\main_npc\roche\h_01_ma__roche.w2ent", true));
		appcomp.IncludeAppearanceTemplate((CEntityTemplate)LoadResource("characters\models\main_npc\roche\body_01__roche.w2ent", true));
	} else
		if(charatempl == "characters\npc_entities\main_npc\lambert.w2ent")
	{
		appcomp.IncludeAppearanceTemplate((CEntityTemplate)LoadResource("characters\models\secondary_npc\lambert\h_01_ma__lambert.w2ent", true));
		appcomp.IncludeAppearanceTemplate((CEntityTemplate)LoadResource("characters\models\secondary_npc\lambert\body_01_ma__lambert.w2ent", true));
		appcomp.IncludeAppearanceTemplate((CEntityTemplate)LoadResource("characters\models\secondary_npc\lambert\s_01_ma__lambert.w2ent", true));
	} else { //def
		appcomp.IncludeAppearanceTemplate((CEntityTemplate)LoadResource("characters\models\main_npc\vesemir\c_01_mb__vesemir.w2ent", true));
		appcomp.IncludeAppearanceTemplate((CEntityTemplate)LoadResource("characters\models\main_npc\vesemir\h_01_mb__vesemir.w2ent", true));
		appcomp.IncludeAppearanceTemplate((CEntityTemplate)LoadResource("characters\models\main_npc\vesemir\body_01__vesemir.w2ent", true));
	}
	if(getStorage().DebugMode)
       	Log("[WITCHER3MP] Player Model Changed to match for: " + charatempl);
}
function _GetMyPlayerInfo() : string
{
	var pos : Vector;
    var pos2 : Vector;
    var output: string;
	var data: MP_NPCSTORAGE;
    pos = GetWitcherPlayer().GetWorldPosition();
    pos2 = GetWitcherPlayer().GetHorseWithInventory().GetWorldPosition();
	data = getStorage().getNPCStorage();

    output += "@PLAYERPOSITION@: @" + (int)pos.X + " " + (int)pos.Y + " " + (int)pos.Z + "@";
    output += ",@HORSEPOSITION@: @" + (int)pos2.X + " " + (int)pos2.Y + " " + (int)pos2.Z + "@";
    output += ",@AREAID@: " + (int)theGame.GetCommonMapManager().GetCurrentArea();
    output += ",@PLEVEL@: " + GetWitcherPlayer().GetLevel();
    output += ",@ISONHORSE@: " + GetWitcherPlayer().IsUsingHorse();
    output += ",@HP@: " + GetWitcherPlayer().GetCurrentHealth();
    output += ",@VERS@: " + getStorage().Version;
    output += ",@ANIMINT@: " + GetWitcherPlayer().GetActorAnimState();
    output += ",@ANIMNAME@: @" + GetWitcherPlayer().GetCurrentStateName() + "@";
    output += ",@ISDEAD@: " + GetWitcherPlayer().isDead;
	output += ",@ANGLEP@: " + GetWitcherPlayer().GetHeading();
	output += ",@ANGLEH@: " + GetWitcherPlayer().GetHorseWithInventory().GetHeading();
    output += ",@FIGHTACTION@: " + (int)GetWitcherPlayer().GetCombatAction();
	output += ",@TARGETGUID@: " + data.GetIDByNPC((CActor )GetWitcherPlayer().GetTarget());
    output += ",@ANSTATENAME@: @" + GetWitcherPlayer().GetCurrentStateName()+ "@";
    output += ",@COMBHEADING@: " + GetWitcherPlayer().GetCombatActionHeading();
	return output;
}
function _GetMyTargetInfo() : string
{
	var pos : Vector;
    var output: string;
    pos = GetWitcherPlayer().GetTarget().GetWorldPosition();

    output += "@COMBATTARGPOS@: @"  + (int)pos.X + " " + (int)pos.Y + " " + (int)pos.Z + "@";
    output += ",@COMBATTARGNAME@: @" + GetWitcherPlayer().GetTarget().GetAppearance() + "@";
    output += ",@COMBATTARGGUID@: " + getStorage().getNPCStorage().GetIDByNPC((CActor )GetWitcherPlayer().GetTarget());
    output += ",@COMBATTHP@: " + (int)GetWitcherPlayer().GetTarget().GetCurrentHealth();
    output += ",@ISBOSS@: " + !GetWitcherPlayer().GetTarget().HasTag('NotBoss');
    output += ",@ISDEAD@: " + (GetWitcherPlayer().GetTarget().GetCurrentHealth() == 0);
	output += ",@ANGLE@: " + GetWitcherPlayer().GetTarget().GetHeading();

	return output;
}
//data = getStorage().getNPCStorage();
//data.Add(entity, GetWorldPosition(), GetWorldPosition(), "WorldGenerated", entityTemplate);