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
exec function TestA(tag:name)
{
	var journalManager : CWitcherJournalManager;
	var questItem	: CJournalResource;
	journalManager = theGame.GetJournalManager();
	//journalManager.ActivateEntryByScriptTag(cname, JS_Active);
	questItem = (CJournalResource)LoadResource(tag);
	journalManager.ActivateEntry(questItem.GetEntry(), JS_Inactive);

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
	//WTF?(CJournalQuestObjective)journalManager.GetEntryByTag(tag);
	//GetEntryByString
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
exec function ResurrectMe()
{
	thePlayer.CheatResurrect();
}
exec function Test_Custom_DeathMenu()
{
	theGame.RequestMenu('DeathScreenMenu');
}
exec function SetDifficulty(lvl:int)
{
	theGame.SetDifficultyLevel(lvl);
}