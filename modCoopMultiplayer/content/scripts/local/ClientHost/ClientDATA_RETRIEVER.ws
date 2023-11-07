exec function GetMODVersion()
{
    Log("W3MP:!MODVER!{@VERS@:" + getStorage().Version + "}");
}
exec function GetWorldData()
{
    var output: string;
    output += "@GAMETIME@: " + theGame.GetHoursPerMinute();
    output += "@GAMEDIFFICULT@: " + theGame.GetDifficultyLevel();
    Log("W3MP:!WORLDDATA!{" + output + "}");
}
exec function GetPlayerData()
{
    var pos : Vector;
    var pos2 : Vector;
    var output: string;
    pos = GetWitcherPlayer().GetWorldPosition();
    pos2 = GetWitcherPlayer().GetHorseWithInventory().GetWorldPosition();
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
    //output += ",@ANSTATENAMECUR@: " + thePlayer.GetStateCur();
    //output += ",@ANSTATENAME@: " + thePlayer.GetStateName();
    output += ",@COMBHEADING@: " + GetWitcherPlayer().GetCombatActionHeading();
    Log("W3MP:!PLAYERDATA!{" + output + "}");
}
exec function GetTargetData()
{
    var pos3 : Vector;
    var data : Storage;
    var output: string;
    data = getStorage();
    pos3 = GetWitcherPlayer().GetTarget().GetWorldPosition();
    output += "@COMBATTARGPOS@: @"  + (int)pos3.X + " " + (int)pos3.Y + " " + (int)pos3.Z + "@";
    output += ",@COMBATTARGNAME@: @" + GetWitcherPlayer().GetTarget().GetAppearance() + "@";
    output += ",@COMBATTARGGUID@: " + GetWitcherPlayer().GetTarget().GetGuidHash();
    output += ",@COMBATTHP@: " + (int)GetWitcherPlayer().GetTarget().GetCurrentHealth();
    output += ",@ISBOSS@: " + (thePlayer.GetBossTag() != '');
    output += ",@ISDEAD@: " + GetWitcherPlayer().GetTarget().isDead;
    output += ",@FIGHTACTION@: " + (int)GetWitcherPlayer().GetCombatAction();
    if(GetWitcherPlayer().GetTarget() && !data.getNPC(GetWitcherPlayer().GetTarget().GetGuidHash())) //ADD NEW NPC TO LOCAL STORAGE FOR RETRIEVE INFO
        data.addNPC((CNewNPC)GetWitcherPlayer().GetTarget());
    Log("W3MP:!COMBATDATA!{" + output + "}");
}
exec function GetTargetDataByID(id:int) //GET NPC STATE DEAD OR NO
{
    var pos3 : Vector;
    var data : Storage;
    var entity: CNewNPC;
    var output: string;
    data = getStorage();
    entity = data.getNPC(id);
    pos3 = entity.GetWorldPosition();
    output += "@COMBATTARGPOS@: @"  + (int)pos3.X + " " + (int)pos3.Y + " " + (int)pos3.Z + "@";
    output += ",@COMBATTARGNAME@: @" + (CActor)entity+ "@";
    output += ",@COMBATTARGGUID@: " + entity.GetGuidHash();
    output += ",@COMBATTHP@: " + (int)entity.GetCurrentHealth();
    output += ",@ISBOSS@: " + (thePlayer.GetBossTag() != '');
    output += ",@ISDEAD@: " + entity.isDead;
    if(!data.getNPC(entity.GetGuidHash()))
        data.addNPC((CNewNPC)GetWitcherPlayer().GetTarget());
    Log("W3MP:!COMBATDATA!{" + output + "}");
}
exec function GetClientState()
{
    Log("W3MP " + getStorage().ClientState);
}
exec function GetCurrentLevelId()
{
    Log("W3MP " + theGame.GetCommonMapManager().GetCurrentArea());
}
exec function GetCurrentHP()
{
    Log("W3MP " + GetWitcherPlayer().GetCurrentHealth());
}
exec function GetCurrentHeading()
{
    Log("W3MP " + GetWitcherPlayer().GetHeading());
}
exec function GetCurrentPLevel()
{
    Log("W3MP " + GetWitcherPlayer().GetLevel());
}
exec function GetCurrentPosition()
{
    var pos:Vector;
    pos = GetWitcherPlayer().GetWorldPosition();
    Log("W3MP " + (int)pos.X + " " + (int)pos.Y + " " + (int)pos.Z);
}
exec function GetCurrentHorsePosition()
{
    Log("W3MP " + VecToString(GetWitcherPlayer().GetHorseWithInventory().GetWorldPosition()));
}
exec function IsOnHorse()
{
    Log("W3MP " + GetWitcherPlayer().IsUsingHorse());
}
exec function GetCurrentRotation()
{
    var eu : EulerAngles;
    eu = GetWitcherPlayer().GetWorldRotation();
    Log("W3MP " + eu.Yaw + " " + eu.Pitch + " " + eu.Roll);
}
exec function GetStateAnimInt()
{
    Log("W3MP " + GetWitcherPlayer().GetActorAnimState());
}
exec function GetStateAnimName()
{
    Log("W3MP " + GetWitcherPlayer().GetCurrentStateName());
}
exec function IsInMenu()
{
    var currentMenu : CR4Menu;
    currentMenu = theGame.GetGuiManager().GetRootMenu();
    if (currentMenu)
        Log("W3MP True");
    else
        Log("W3MP False");
}
exec function IsGameStoped()
{
   Log("W3MP " + !theGame.IsStopped());
}
exec function IsGamePaused()
{
    Log("W3MP " + !theGame.IsPaused());
}
exec function GetCombatTargetName()
{
    Log("W3MP " + GetWitcherPlayer().GetTarget());
}
exec function GetFightSwStateName()
{
    Log((int)GetWitcherPlayer().GetCombatAction());
}
exec function IsCombatTargetDead(guid:int)
{
    var data : Storage;
    data = getStorage();
    Log("W3MP " + data.getNPC(guid).isDead);
}
exec function IsInCombat()
{
    Log("W3MP " + GetWitcherPlayer().IsInCombatAction());
}
exec function GetAllNPCSList()
{
    var e:string;
    var i:int;
    var data : Storage;
    var pos:Vector;
    data = getStorage();
    for (i=0; i < data.NpcEntities.Size(); i+=1)
    {
        pos = data.NpcEntities[i].GetWorldPosition();
        e+= "\n|POSITION:" + (int)pos.X + " " + (int)pos.Y + " " + (int)pos.Z + "|GUID:" + data.NpcEntities[i].GetGuidHash() + "|TEMPLNAME:"+ (CActor)data.NpcEntities[i];
    }
    Log("W3MP:!NPCSDATA!" +e);
}
exec function GetActiveQuest()
{
    var i, j : int;
    var journalManager : CWitcherJournalManager;
    var questItem	: CJournalQuest;
    var output, phases : string;
    var CurrentObjectives : array<SJournalQuestObjectiveData>;
    var questPhase : CJournalQuestPhase;
    var Objective	: CJournalQuestObjective;
    journalManager = theGame.GetJournalManager();
    questItem = (CJournalQuest)journalManager.GetTrackedQuest();
	for(i = 0; i < questItem.GetNumChildren(); i += 1)
	{
		questPhase = (CJournalQuestPhase)questItem.GetChild(i);
        Objective = (CJournalQuestObjective)questItem.GetChild(i);
		if(questPhase)
			for(j = 0; j < questPhase.GetNumChildren(); j += 1)
				phases += ",@PHASE_" + j + "@: " + j + ",@STATUS_" + j +"@: @" + journalManager.GetEntryStatus(Objective) + "@," + "@TAGOFPAHASE_" + j + "@: @" + questPhase.GetUniqueScriptTag() + "@";
	}
	output += "@" + questItem.GetTitleStringId() + "@" + ":{@TAG@: @" + questItem.GetUniqueScriptTag() + "@, @TYPE@: @" + questItem.GetType() + "@,@CONTENTTYPE@: @" + questItem.GetContentType() + "@" + phases + "}";
	Log("W3MP:!QUESTDATA![{" + output + "}]");
}
exec function GetQuestCount()
{
    var allQuests : array<CJournalBase>;
    var journalManager : CWitcherJournalManager;
    journalManager = theGame.GetJournalManager();
	journalManager.GetActivatedOfType('CJournalQuest', allQuests);
    Log("W3MP " + allQuests.Size());
}
exec function GetQuestInfo(id:int)
{
	var i, j: int;
	var allQuests : array<CJournalBase>;
	var journalManager : CWitcherJournalManager;
	var questItem	: CJournalQuest;
	var output, phases : string;
	var questPhase : CJournalQuestPhase;
	var CurrentObjectives : array<SJournalQuestObjectiveData>;
	var Objective	: CJournalQuestObjective;
	journalManager = theGame.GetJournalManager();
	journalManager.GetActivatedOfType('CJournalQuest', allQuests);
	theGame.GetJournalManager().GetTrackedQuestObjectivesData(CurrentObjectives);
	questItem = (CJournalQuest)allQuests[id];
    Log("W3MP:!QUESTDATA![{" + output + "@" + questItem.GetTitleStringId() + "@" + ":{@TAG@: @" + questItem.GetUniqueScriptTag() + "@, @TYPE@: @" + questItem.GetType() + "@,@CONTENTTYPE@: @" + questItem.GetContentType() + "@");
	for( i = 0; i < questItem.GetNumChildren(); i += 1 )
	{
		questPhase = (CJournalQuestPhase)questItem.GetChild(i);
           Objective = (CJournalQuestObjective)questItem.GetChild(i);
	    if(questPhase)
		    for( j = 0; j < questPhase.GetNumChildren(); j += 1 )
            {
			    Log(",@PHASE_" + j + "@: " + j + ",@STATUS_" + j +"@: @" + journalManager.GetEntryStatus(Objective) + "@,");
                Log("@TAGPH" + j + "@: ");
                Log("@" + questPhase.GetUniqueScriptTag() + "@");
            }
    }
	Log("}}]");
}