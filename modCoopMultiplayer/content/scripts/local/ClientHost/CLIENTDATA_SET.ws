exec function SetClientDebugMode(safa : int)
{
    var data : Storage;
    data = getStorage();
    data.DebugMode = (safa == 1);
    Log("[WITCHER3MP] Debug Enabled: " + data.DebugMode);
}
exec function TeleportMe(xp:int, yp:int, zp:int)
{
	var data: Storage;
	data = getStorage();
	GetWitcherPlayer().Teleport(Vector(xp, yp, zp));
	if(data.DebugMode)
        Log("[WITCHER3MP] TeleportMe " + VecToString(Vector(xp, yp, zp)));
}
exec function SwitchCharacter(charatempl:string)
{
	var data: Storage;
	data = getStorage();
	_SwitchCharacter(charatempl);
	if(data.DebugMode)
        Log("[WITCHER3MP] Character Switched TO: " + charatempl);
}
exec function TestA(cname:name)
{
	var journalManager : CWitcherJournalManager;
	var questItem	: CJournalQuest;
	journalManager.ActivateEntryByScriptTag(cname, JS_Active);
}
exec function AddQuest(qestname:string)
{
	var journalManager : CWitcherJournalManager;
	var questItem	: CJournalQuest;
	var jourMenu : CR4JournalQuestMenu;
	var res : CResource;
	var jres : CJournalResource;
    var jbase : CJournalBase;
    var jquest : CJournalQuest;
	res = LoadResource(qestname);
    jres = (CJournalResource)res;
    jbase = jres.GetEntry();
    jquest = (CJournalQuest)jbase;
	//WTF?(CJournalQuestObjective)m_journalManager.GetEntryByTag( tag );
}
exec function GetRewardQuest(qestname:string)
{
	var i : int;
    var res : CResource;
    var jres : CJournalResource;
    var jbase : CJournalBase;
    var jquest : CJournalQuest;
    var rewards : array< name >;
    var rewrd : SReward;

    res = LoadResource(qestname);
    jres = (CJournalResource)res;
    jbase = jres.GetEntry();
    jquest = (CJournalQuest)jbase;
    rewards = theGame.GetJournalManager().GetQuestRewards(jquest);
    for (i = 0; i < rewards.Size(); i += 1) if (theGame.GetReward(rewards[i], rewrd))
	Log("[WITCHER3MP] Reward: " + rewrd.gold + " " + rewrd.experience + " " + rewrd.items.Size());
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
exec function ResurrectMe()
{
	thePlayer.CheatResurrect();
}
exec function Test_Custom_DeathMenu()
{
	theGame.RequestMenu('DeathScreenMenu');
}