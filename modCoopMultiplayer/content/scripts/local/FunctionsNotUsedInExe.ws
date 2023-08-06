function _SpawnIT(nameenti:string, npc_templ:string, xp:int, yp:int, zp:int, xr:int, yr:int, zr:int) : CEntity
{
    var template: CEntityTemplate;
    var entity: CEntity;
    var position : Vector;
    var rotation : EulerAngles;
    var data : MyData;
    data = GetServerSettings();
    position = Vector(xp, yp, zp);
    rotation = EulerAngles(xr, yr, zr);
    template = (CEntityTemplate)LoadResource(npc_templ, true);
    entity = (CGameplayEntity)theGame.CreateEntity(
      template,
      position,
      rotation,,,,
      PM_DontPersist
    );
    ((CNewNPC)entity).GetMovingAgentComponent().SetGameplayRelativeMoveSpeed(4);
    //ID DON'T ATTACK set
    ((CNewNPC)entity).SetTemporaryAttitudeGroup(
      'q104_avallach_friendly_to_all',
      AGP_Scenes
    );
    ((CNewNPC)entity).GetMovingAgentComponent().SetGameplayMoveDirection(GetWitcherPlayer().GetHeading());
    ((CNewNPC)entity).ActionCancelAll();
    onelinerEntity(nameenti, entity);
    return (CEntity)entity;
}
/*function _SetWalkOrRunState(entity:CEntity, istate:int)
{
  if(istate == 0){
    ((CNewNPC)entity).SetIsRunning(false);
		((CNewNPC)entity).SetIsWalking(false);
  } else
  if(istate == 1){
    ((CNewNPC)entity).SetIsRunning(false);
		((CNewNPC)entity).SetIsWalking(true);
  } else
  if(istate == 2){
    ((CNewNPC)entity).SetIsRunning(true);
		((CNewNPC)entity).SetIsWalking(true);
  }
}*/
//_SetHeading(entity, pos);
function _SetHeading(entity:CEntity, pos:Vector)
{
  ((CNewNPC)entity).GetMovingAgentComponent().SetGameplayMoveDirection(VecHeading(pos - entity.GetWorldPosition()));
}
function _SetMoveTo(entity:CEntity, moveType:EMoveType, pos:Vector)
{
  ((CNewNPC)entity).ActionMoveToAsync(pos, moveType);
  ((CNewNPC)entity).ActionMoveToWithHeadingAsync( pos, pos.Y, MT_Walk, 0.f, 0.1f );
}
function _SetRotAndMoveTo(entity:CEntity, pos:Vector)
{
    var moveType : EMoveType;
    moveType = MT_Run;
    ((CNewNPC)entity).ActionRotateToAsync(pos);
    ((CNewNPC)entity).ActionMoveToAsync(pos, moveType, 5, 10);
}
function _Teleport(entity:CEntity, pos:Vector)
{
    ((CNewNPC)entity).ActionCancelAll();
    ((CNewNPC)entity).Teleport(pos);
}
function _TeleportRotate(entity:CEntity, pos:Vector, rot:EulerAngles)
{
    ((CNewNPC)entity).ActionCancelAll();
    ((CNewNPC)entity).TeleportWithRotation(pos, rot);
}
function _SetTypeMove(entity:CEntity, st:int)
{
  ((CNewNPC)entity).GetMovingAgentComponent().SetGameplayRelativeMoveSpeed(st); //0 stay in place. 1 is for Walking, 2 jogging, 4 sprinting.
}
function _SetCombatTarget(entity:CEntity, entity2:CEntity)
{
  //((CNewNPC)entity).SetCombatTarget(entity2);
}

//HOST OPERA
function _Data_AddPLAYER(e:PlayerDataStruct)
{
    var data : MyData;
    data = GetServerSettings();
    data.PlayersDATAS.PushBack(new PlayerDataStruct in thePlayer);
    data.PlayersDATAS[data.PlayersDATAS.Size()-1] = e;
    UpdateServerSettings(data);
    if(data.DebugMode)
        Log("[WITCHER3MP] Succesfully Added Player Entity to list");
}
function _Data_AddNPC(e:CEntity)
{
    var data : MyData;
    data = GetServerSettings();
    data.NpcEntities.PushBack(e);
    UpdateServerSettings(data);
    if(data.DebugMode)
        Log("[WITCHER3MP] Succesfully changed Added NPC Entity to list");
}

function _RemoveNPC(e:CEntity)
{
    var data : MyData;
    data = GetServerSettings();
    data.NpcEntities.Remove(e);
    UpdateServerSettings(data);
    if(data.DebugMode)
        Log("[WITCHER3MP] Succesfully REMOVED NPC entity from list");
}
function _RemovePlayer(e:PlayerDataStruct)
{
    var data : MyData;
    data = GetServerSettings();
    data.PlayersDATAS.Remove(e);
    UpdateServerSettings(data);
    if(data.DebugMode)
        Log("[WITCHER3MP] Succesfully Removed Player From List");
}