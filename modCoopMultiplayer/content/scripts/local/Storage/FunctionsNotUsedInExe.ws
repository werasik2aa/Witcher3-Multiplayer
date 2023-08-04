function _SpawnIT(cl:bool, npc_templ:string, xp:int, yp:int, zp:int, xr:int, yr:int, zr:int) : CEntity
{
    var template: CEntityTemplate;
    var entity: CEntity;
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
    ((CActor)entity).GetMovingAgentComponent().SetGameplayRelativeMoveSpeed(4);
    //ID DON'T ATTACK set
    ((CActor)entity).SetTemporaryAttitudeGroup(
      'q104_avallach_friendly_to_all',
      AGP_Default
    );
    ((CActor)entity).GetMovingAgentComponent().SetGameplayMoveDirection(380);
    return entity;
}
function _SetMoveTo(entity:CEntity, moveType:EMoveType, pos:Vector)
{
  ((CActor)entity).ActionRotateToAsync(pos);
  ((CActor)entity).ActionMoveToAsync(pos, moveType, 5, 10);
}
function _SetRotateTo(entity:CEntity, pos:Vector)
{
  ((CActor)entity).ActionRotateToAsync(pos);
}
function _SetRotAndMoveTo(entity:CEntity, pos:Vector)
{
    var moveType : EMoveType;
    moveType = MT_Run;
    ((CActor)entity).ActionRotateToAsync(pos);
    ((CActor)entity).ActionMoveToAsync(pos, moveType, 5, 10);
}
function _Teleport(entity:CEntity, pos:Vector)
{
    ((CActor)entity).Teleport(pos);
}
function _TeleportRotate(entity:CEntity, pos:Vector, rot:EulerAngles)
{
    var moveType : EMoveType;
    moveType = MT_Run;
    ((CActor)entity).TeleportWithRotation(pos, rot);
}
function _SetTypeMove(entity:CEntity, st:int)
{
  ((CActor)entity).GetMovingAgentComponent().SetGameplayRelativeMoveSpeed(st); // 1 is for Walking, 2 jogging, 4 sprinting.
}
function _SetCombatTarget(entity:CEntity, entity2:CEntity)
{
  //((CActor)entity).SetCombatTarget(entity2);
}

//HOST OPERA
function Data_AddPLAYER(e:CEntity)
{
    var data : MyModData;
    data = GetServerSettings();
    data.PlayersEntities.PushBack(e);
    UpdateServerSettings(data);
    if(data.DebugMode)
        Log("[WITCHER3MP] Succesfully Added Player Entity to list");
}
function Data_AddNPC(e:CEntity)
{
    var data : MyModData;
    data = GetServerSettings();
    data.NpcEntities.PushBack(e);
    UpdateServerSettings(data);
    if(data.DebugMode)
        Log("[WITCHER3MP] Succesfully changed Added NPC Entity to list");
}

function _RemoveNPC(e:CEntity)
{
    var data : MyModData;
    data = GetServerSettings();
    data.NpcEntities.Remove(e);
    UpdateServerSettings(data);
    if(data.DebugMode)
        Log("[WITCHER3MP] Succesfully REMOVED NPC entity from list");
}
function _RemovePlayer(e:CEntity)
{
    var data : MyModData;
    data = GetServerSettings();
    data.PlayersEntities.Remove(e);
    UpdateServerSettings(data);
    if(data.DebugMode)
        Log("[WITCHER3MP] Succesfully Removed Player From List");
}