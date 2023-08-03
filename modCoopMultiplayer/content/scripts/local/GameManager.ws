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
}
exec function NotifyChatMSG(msg : String)
{
    GetWitcherPlayer().DisplayHudMessage(msg);
    Log("WITCHER-3MP [WITCHER3MP] Message Sended");
}
exec function NPCSetAction(id:int, data:string)
{
   var entity : CEntity;
   var dat : MyModData;
   dat = GetServerSettings();
   entity = dat.PlayersEntities[id];
   ((CActor)entity).SetTemporaryAttitudeGroup('q104_avallach_friendly_to_all', AGP_Default);
}

exec function EntityMoveFloat(id:int, xp:int, yp:int, zp:int) {
   var entity : CEntity;
   var PosMov : Vector;
   var position : Vector;
   var dat : MyModData;
   position = Vector(xp, yp, zp);
   dat = GetServerSettings();
   entity = dat.PlayersEntities[id];
   ((CActor)entity).GetMovingAgentComponent().SetGameplayMoveDirection(180);
}
function SpawnNpcEntity(npc_templ:string, xp:int, yp:int, zp:int, xr:int, yr:int, zr:int)
{
    var template: CEntityTemplate;
    var template_path: string;
    var entity: CEntity;
    var data : MyModData;
    var position : Vector;
    var rotation : EulerAngles;
    position = Vector(xp, yp, zp);
    rotation = EulerAngles(xr, yr, zr);
    template = (CEntityTemplate)LoadResource(npc_templ, true);
    entity = theGame.CreateEntity(
      template,
      position,
      rotation
    );
    //ID DON'T ATTACK
    ((CActor)entity).SetTemporaryAttitudeGroup(
      'q104_avallach_friendly_to_all',
      AGP_Default
    );
    data = GetServerSettings();
    data.PlayersEntities.PushBack(entity);
    UpdateServerSettings(data);
}
exec function SpawnNpcEntityE(npc_templ:string, xp:int, yp:int, zp:int, xr:int, yr:int, zr:int) {
    SpawnNpcEntity(npc_templ, xp, yp, zp, xr, yr, zr);
}
exec function SetStateAnim(id:int)
{
    var entity : CEntity;
    var data: MyModData;
    data = GetServerSettings();
    entity = data.PlayersEntities[id];
    ((CActor)entity).GetMovingAgentComponent().SetGameplayRelativeMoveSpeed(id); // 1 is for Walking, 2 jogging, 4 sprinting.
    if(data.DebugMode)
        Log("[WITCHER3MP] 1 is for Walking, 2 jogging, 4 sprinting |CURSTATE: " + id);
    else
        Log("Success");
}
