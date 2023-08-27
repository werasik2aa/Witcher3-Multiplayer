exec function SetClientState(safa:int)
{
    var data : Storage;
    data = getStorage();
    data.ClientState = safa;
    if(getStorage().DebugMode)
        Log("[WITCHER3MP] Succesfully ClientState changed: " + safa);
}
exec function LoadServerSave(saveListIndex:int)
{
    var startGameStatus : ENewGamePlusStatus;
    var saveGameRef : SSavegameInfo;
	var saveGames	: array< SSavegameInfo >;
    theGame.ListSavedGames( saveGames );
    saveGameRef = saveGames[saveListIndex];
    startGameStatus = theGame.StartNewGamePlus(saveGameRef);
    if (startGameStatus == NGP_Success)
    {
        theGame.GetGuiManager().RequestMouseCursor(false);
        theGame.GetGuiManager().GetRootMenu().CloseMenu();
    }
    if(getStorage().DebugMode)
        Log("W3MP [WITCHER3MP] Loading SAVE");
}
//SPAWN NEW NPC
exec function Spawn_Player(nick:string, clientid:int, npc_templ:string, xp:int, yp:int, zp:int, xp2:int, yp2:int, zp2:int)
{
	AddNewPlayerToStorage(_SetupPlayerObjectDefault(_SpawnIT(nick, npc_templ, Vector(xp, yp, zp), true)), _SpawnIT("Of player: " + nick + ":" + clientid, "horse", Vector(xp2, yp2, zp2), false), clientid);
	GetWitcherPlayer().DisplayHudMessage("Player: " + nick + " Joined to game");
    if(getStorage().DebugMode)
        Log("[WITCHER3MP] Spawned Entity With template " + npc_templ + " \nPosition X:" + xp + " Y:" + yp + " Z:" + zp);
}
exec function Spawn_NPC(npc_templ:string, xp:int, yp:int, zp:int, guid:int)
{
    _Data_AddNPC(_SpawnITNPC(npc_templ, Vector(xp, yp, zp), true), guid); // TODO: SPAWN NPC
    if(getStorage().DebugMode)
        Log("[WITCHER3MP] Spawned Entity With template " + npc_templ + " \nPosition X:" + xp + " Y:" + yp + " Z:" + zp);
}
exec function Spawn_CORPSE(npc_templ:string, xp:int, yp:int, zp:int)
{
    _SpawnITCORPSE(npc_templ, Vector(xp, yp, zp));
    if(getStorage().DebugMode)
        Log("[WITCHER3MP] Spawned Entity With template " + npc_templ + " \nPosition X:" + xp + " Y:" + yp + " Z:" + zp);
}
//CHAT AND OTHER
exec function NotifyChatMSG(msg : String)
{
    thePlayer.DisplayHudMessage(msg);
    if(getStorage().DebugMode)
        Log("W3MP [WITCHER3MP] Message Sended");
}
exec function UnpauseGame()
{
    theGame.Unpause("menus");
    theGame.Unpause("IngameMenu");
    if(getStorage().DebugMode)
        Log("[WITCHER3MP] GameUnpaused ");
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