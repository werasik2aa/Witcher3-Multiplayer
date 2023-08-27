//exec
exec function LaunchCustomFramework(templa:string) {
    var storage : Storage;
    var actors : array<CActor>;
    var actor : CActor;
    storage = getStorage();
    storage.clearPlayers();
    _SwitchCharacter(templa);
    if(storage.DebugMode)
        Log("[WITCHER3MP] Player Data Cleared!");
}
exec function SetClientState(safa:int)
{
    var data : Storage;
    data = getStorage();
    data.ClientState = safa;
    if(getStorage().DebugMode)
        Log("[WITCHER3MP] Succesfully ClientState changed: " + safa);
}
exec function NotifyChatMSG(msg : String)
{
    thePlayer.DisplayHudMessage(msg);
    if(getStorage().DebugMode)
        Log("W3MP [WITCHER3MP] Message Sended");
}
//FUNCTIONS
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
	}else
	if(charatempl == "characters\npc_entities\main_npc\roche.w2ent")
	{
		((CAppearanceComponent)appcomp).IncludeAppearanceTemplate((CEntityTemplate)LoadResource("characters\models\main_npc\roche\c_01__roche.w2ent", true));
		((CAppearanceComponent)appcomp).IncludeAppearanceTemplate((CEntityTemplate)LoadResource("characters\models\main_npc\roche\h_01_ma__roche.w2ent", true));
		((CAppearanceComponent)appcomp).IncludeAppearanceTemplate((CEntityTemplate)LoadResource("characters\models\main_npc\roche\body_01__roche.w2ent", true));
	} else
	if(charatempl == "characters\npc_entities\main_npc\lambert.w2ent")
	{
		((CAppearanceComponent)appcomp).IncludeAppearanceTemplate((CEntityTemplate)LoadResource("characters\models\secondary_npc\lambert\h_01_ma__lambert.w2ent", true));
		((CAppearanceComponent)appcomp).IncludeAppearanceTemplate((CEntityTemplate)LoadResource("characters\models\secondary_npc\lambert\body_01_ma__lambert.w2ent", true));
		((CAppearanceComponent)appcomp).IncludeAppearanceTemplate((CEntityTemplate)LoadResource("characters\models\secondary_npc\lambert\s_01_ma__lambert.w2ent", true));
	} else { //standart
		((CAppearanceComponent)appcomp).IncludeAppearanceTemplate((CEntityTemplate)LoadResource("characters\models\main_npc\vesemir\c_01_mb__vesemir.w2ent", true));
		((CAppearanceComponent)appcomp).IncludeAppearanceTemplate((CEntityTemplate)LoadResource("characters\models\main_npc\vesemir\h_01_mb__vesemir.w2ent", true));
		((CAppearanceComponent)appcomp).IncludeAppearanceTemplate((CEntityTemplate)LoadResource("characters\models\main_npc\vesemir\body_01__vesemir.w2ent", true));
	}
	if(data.DebugMode)
        Log("[WITCHER3MP] Player Model Changed to match for: " + charatempl);
}