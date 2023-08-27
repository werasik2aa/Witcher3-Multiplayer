exec function FillChestWithItem(tag : name, nameit:name)
{
	var container : W3Container;
	var data : Storage;
	data = getStorage();
	container = (W3Container)theGame.GetEntityByTag(tag);
	if (container) container.GetInventory().AddAnItem(nameit);
	if (data.DebugMode)
		Log("[WITCHER3MP] Added Item: " + nameit + " To chest: " + container);
}
exec function GetChestItems()
{
	
}