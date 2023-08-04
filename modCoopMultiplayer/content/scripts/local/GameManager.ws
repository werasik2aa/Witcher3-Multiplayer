exec function SetClientState(safa : int)
{
    var data : MyModData;
    data = GetServerSettings();
    data.ClientState = safa;
    UpdateServerSettings(data);
    if(data.DebugMode)
        Log("[WITCHER3MP] Succesfully ClientState changed: " + safa);
}
exec function LoadServerSave(saveListIndex:int)
{
    var startGameStatus : ENewGamePlusStatus;
    var saveGameRef : SSavegameInfo;
	var saveGames	: array< SSavegameInfo >;
    var dat : MyModData;
    dat = GetServerSettings();
    theGame.ListSavedGames( saveGames );
    saveGameRef = saveGames[saveListIndex];
    startGameStatus = theGame.StartNewGamePlus(saveGameRef);
    if (startGameStatus == NGP_Success)
    {
        theGame.GetGuiManager().RequestMouseCursor(false);
        theGame.GetGuiManager().GetRootMenu().CloseMenu();
    }
    if(dat.DebugMode)
        Log("WITCHER-3MP [WITCHER3MP] Loading SAVE");
}
//SPAWN NEW NPC
exec function Spawn_Player(id:int, xp:int, yp:int, zp:int, xr:int, yr:int, zr:int)
{
    var data : MyModData;
    data = GetServerSettings();
    _SpawnIT(true, "characters\npc_entities\main_npc\lambert.w2ent", xp, yp, zp, xr, yr, zr);
    if(data.DebugMode)
        Log("[WITCHER3MP] Spawned Entity With template_id: " + id + " \nPosition X:" + xp + " Y:" + yp + " Z:" + zp);
}
exec function Spawn_PlayerV2(npc_templ:string, xp:int, yp:int, zp:int, xr:int, yr:int, zr:int)
{
    var data : MyModData;
    data = GetServerSettings();
    _SpawnIT(true, npc_templ, xp, yp, zp, xr, yr, zr);
    if(data.DebugMode)
        Log("[WITCHER3MP] Spawned Entity With template " + npc_templ + " \nPosition X:" + xp + " Y:" + yp + " Z:" + zp);
}
exec function Spawn_NPC(npc_templ:string, xp:int, yp:int, zp:int, xr:int, yr:int, zr:int)
{
    var data : MyModData;
    data = GetServerSettings();
    _SpawnIT(false, npc_templ, xp, yp, zp, xr, yr, zr);
    if(data.DebugMode)
        Log("[WITCHER3MP] Spawned Entity! Template: " + npc_templ + " \nPosition X:" + xp + " Y:" + yp + " Z:" + zp);
}

//CHAT AND OTHER
exec function NotifyChatMSG(msg : String)
{
    var dat : MyModData;
    dat = GetServerSettings();
    GetWitcherPlayer().DisplayHudMessage(msg);
    if(dat.DebugMode)
        Log("WITCHER-3MP [WITCHER3MP] Message Sended");
}
exec function UnpauseGame()
{
    var data : MyModData;
    theGame.Unpause("menus");
    theGame.Unpause("IngameMenu");
    data = GetServerSettings();
    if(data.DebugMode)
        Log("[WITCHER3MP] GameUnpaused ");
}