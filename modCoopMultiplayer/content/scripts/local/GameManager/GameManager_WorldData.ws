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
exec function GetNearNPCS()
{
    var e : string;
    var i : int;
    var actors	: array<CActor>;
    var currhash : int;
    var actor : CNewNPC;
    actors = GetActorsInRange(thePlayer, 5);
	for	( i = 0; i < actors.Size(); i+= 1 )
	{
		actor = (CNewNPC)actors[i];
		currhash = actor.GetGuidHash();
        e += " ID: " + currhash + " | NAME: " + actor;
	}
    Log("W3MP " + e);
}