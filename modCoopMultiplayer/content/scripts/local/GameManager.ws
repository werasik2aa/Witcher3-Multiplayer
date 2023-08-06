exec function SetClientState(safa:int)
{
    var data : MyData;
    data = GetServerSettings();
    data.ClientState = safa;
    if(GetServerSettings().DebugMode)
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
    if(GetServerSettings().DebugMode)
        Log("WITCHER-3MP [WITCHER3MP] Loading SAVE");
}
//SPAWN NEW NPC
exec function Spawn_Player(nick:string, clientid:int, npc_templ:string, xp:int, yp:int, zp:int)
{
    _Data_AddPLAYER(_SpawnIT(nick, npc_templ, xp, yp, zp));
    _Data_AddHORSE(_SpawnIT("Of player: " + nick + ":" + clientid, "horse", xp, yp, zp));
    if(GetServerSettings().DebugMode)
        Log("[WITCHER3MP] Spawned Entity With template " + npc_templ + " \nPosition X:" + xp + " Y:" + yp + " Z:" + zp);
}
//CHAT AND OTHER
exec function NotifyChatMSG(msg : String)
{
    thePlayer.DisplayHudMessage(msg);
    if(GetServerSettings().DebugMode)
        Log("WITCHER-3MP [WITCHER3MP] Message Sended");
}
exec function UnpauseGame()
{
    theGame.Unpause("menus");
    theGame.Unpause("IngameMenu");
    if(GetServerSettings().DebugMode)
        Log("[WITCHER3MP] GameUnpaused ");
}