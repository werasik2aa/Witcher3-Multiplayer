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
	AddNewPlayerToStorage(_SpawnIT(nick, npc_templ, Vector(xp, yp, zp), true), _SpawnIT("Of player: " + nick + ":" + clientid, "horse", Vector(xp2, yp2, zp2), false), clientid);
    SetupPlayer(clientid);
	GetWitcherPlayer().DisplayHudMessage("Player: " + nick + " Joined to game");
    if(getStorage().DebugMode)
        Log("[WITCHER3MP] Spawned Entity With template " + npc_templ + " \nPosition X:" + xp + " Y:" + yp + " Z:" + zp);
}
exec function Spawn_NPC(npc_templ:string, xp:int, yp:int, zp:int)
{
    _Data_AddNPC(_SpawnITNPC(npc_templ, Vector(xp, yp, zp), true)); // TODO: SPAWN NPC
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
exec function KillActorByGuidHash(GuidHash : int, entiname:string, idcl:int)
{
	var action		: W3DamageAction;
	var i 			: int;
	var actors		: array<CActor>;
	var actor		: CActor;
	var currhash    : int;
	var data 		: Storage;
	data = getStorage();
	actor = (CActor)data.getNPC(GuidHash);
	if(!actor)
		_SpawnITCORPSE(entiname, data.getPlayerOBJ(idcl).GetWorldPosition());
	else
	{
		actor.SignalGameplayEvent( 'Death' );
		actor.SetAlive(false);
		actor.OnDeath(action);
	}
}
exec function CheckCombatTarget(GuidHash : int, entiname:string, idcl:int)
{
	var data : Storage;
	data = getStorage();
	_CheckOrSpawnNPC(GuidHash, entiname, data.getPlayerOBJ(idcl));
}