exec function SetCharacter(npct:string)
{
    _SwitchCharacter((CActor)thePlayer, npct);
}
exec function BlockAllActions(block:bool)
{
	var guiManager : CR4GuiManager;
  	if (block){
    	thePlayer.BlockAction(EIAB_OpenPreparation, 'tutorial_inventory');
	  	thePlayer.BlockAction(EIAB_OpenMap, 'being_replacer');
	  	thePlayer.BlockAction(EIAB_OpenInventory, 'tutorial_inventory');
	  	thePlayer.BlockAction(EIAB_OpenJournal, 'tutorial_inventory');
	  	thePlayer.BlockAction(EIAB_OpenCharacterPanel, 'tutorial_inventory');
	  	thePlayer.BlockAction(EIAB_OpenGlossary, 'tutorial_inventory');
	  	thePlayer.BlockAction(EIAB_OpenAlchemy, 'tutorial_inventory');
	  	thePlayer.BlockAction(EIAB_OpenMeditation, 'tutorial_inventory');
	  	thePlayer.BlockAction(EIAB_MeditationWaiting, 'tutorial_inventory');
	  	thePlayer.BlockAction(EIAB_DismountVehicle, 'tutorial_inventory');
    	thePlayer.BlockAction(EIAB_OpenFastMenu, 'tutorial_inventory');
    	thePlayer.BlockAction(EIAB_FastTravel, 'tutorial_inventory');
		guiManager = theGame.GetGuiManager();
		if (guiManager && guiManager.IsAnyMenu()) guiManager.GetRootMenu().CloseMenu();
	}
	else
	{
		thePlayer.UnblockAction(EIAB_OpenPreparation, 'tutorial_inventory');
	  	thePlayer.UnblockAction(EIAB_OpenMap, 'tutorial_inventory');
	  	thePlayer.UnblockAction(EIAB_OpenInventory, 'tutorial_inventory');
	  	thePlayer.UnblockAction(EIAB_OpenJournal, 'tutorial_inventory');
	  	thePlayer.UnblockAction(EIAB_OpenCharacterPanel, 'tutorial_inventory');
	  	thePlayer.UnblockAction(EIAB_OpenGlossary, 'tutorial_inventory');
	  	thePlayer.UnblockAction(EIAB_OpenAlchemy, 'tutorial_inventory');
	  	thePlayer.UnblockAction(EIAB_OpenMeditation, 'tutorial_inventory');
	  	thePlayer.UnblockAction(EIAB_MeditationWaiting, 'tutorial_inventory');
	  	thePlayer.UnblockAction(EIAB_DismountVehicle, 'tutorial_inventory');
    	thePlayer.UnblockAction(EIAB_OpenGwint, 'tutorial_inventory');
    	thePlayer.UnblockAction(EIAB_OpenFastMenu, 'tutorial_inventory');
    	thePlayer.UnblockAction(EIAB_FastTravel, 'tutorial_inventory');
	}
}
exec function OnConnectedMSG(nick:string)
{
    GetWitcherPlayer().DisplayHudMessage("Player: " + nick + " Joined to game");
}
exec function OnDisConnectedMSG(ID:int)
{
    var data: MP_NPCSTORAGE;
    data = getStorage().getPLAYERStorage();
    GetWitcherPlayer().DisplayHudMessage("Player: " + data.GetByID(ID).Name + " Disconnected");
	data.RemoveByID(ID);
}
exec function TeleportClient(x:int, y:int, z:int, IsOnHorse:bool)
{
	var data: MP_NPCSTORAGE;
    data = getStorage().getPLAYERStorage();
	((CActor)thePlayer).Teleport(Vector(x, y, z));
}
exec function SetDifficulty(lvl:int)
{
	theGame.SetDifficultyLevel(lvl);
}
