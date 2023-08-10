function SetupPlayer(idcl:int)
{
	var ids : array<SItemUniqueId>;
	var inv : CInventoryComponent;
    var data : Storage;
    var actor : CActor;
    data = getStorage();
    actor = (CActor)data.getPlayerOBJ(idcl);
    inv = actor.GetInventory();
    //inv.GetAllItems(Itemids);
	//inv.GetItemEquippedOnSlot( EES_SilverSword, silverSwordId );
	//inv.GetItemEquippedOnSlot( EES_SteelSword, steelSwordId );

	ids = inv.AddAnItem('Autogen steel sword', 1); actor.EquipItem(ids[0]);
	ids = inv.AddAnItem('Autogen silver sword', 1); actor.EquipItem(ids[0]);
	ids = inv.AddAnItem('Autogen Pants', 1); actor.EquipItem(ids[0]);
	ids = inv.AddAnItem('Autogen Gloves', 1); actor.EquipItem(ids[0]);
	ids = inv.AddAnItem('Autogen Boots', 1); actor.EquipItem(ids[0]);
	ids = inv.AddAnItem('Autogen Armor', 1); actor.EquipItem(ids[0]);
}