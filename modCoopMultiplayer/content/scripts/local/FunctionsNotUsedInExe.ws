function _SpawnIT(nameenti:string, npc_templ:string, position:Vector, fullname:bool) : CNewNPC
{
    var template: CEntityTemplate;
    var entity: CNewNPC;
    var data : Storage;
    var tags : array<CName>;
    data = getStorage();
    template = (CEntityTemplate)LoadResource(npc_templ, fullname);
    entity = (CNewNPC)theGame.CreateEntity(template, position,EulerAngles(0, 0, 0),,,,PM_DontPersist, tags);
    ((CActor)entity).SetTemporaryAttitudeGroup('q104_avallach_friendly_to_all', AGP_Default);
    onelinerEntity(nameenti, entity);//nickname
    return entity;
}
function _SpawnITNPC(npc_templ:string, position:Vector, fullname:bool) : CNewNPC
{
    var template: CEntityTemplate;
    var entity: CNewNPC;
    var data : Storage;
    var tags : array<CName>;
    data = getStorage();
    template = (CEntityTemplate)LoadResource(npc_templ, fullname);
    entity = (CNewNPC)theGame.CreateEntity(template, position,EulerAngles(0, 0, 0),,,,PM_DontPersist, tags);
    return entity;
}
function _SpawnITCORPSE(npc_templ:string, position:Vector) : CNewNPC
{
  var template: CEntityTemplate;
  var entity: CNewNPC;
  var data : Storage;
  var tags : array<CName>;
  var action : W3DamageAction;
  data = getStorage();
  template = (CEntityTemplate)LoadResource(npc_templ, false);
  entity = (CNewNPC)theGame.CreateEntity(template, position,EulerAngles(0, 0, 0),,,,PM_DontPersist, tags);
  ((CActor)entity).SignalGameplayEvent( 'Death' );
	((CActor)entity).SetAlive(false);
	((CActor)entity).OnDeath( action );
  return entity;
}
function _SetHeading(entity:CNewNPC, Heading:float)
{
  ((CActor)entity).GetMovingAgentComponent().SetGameplayMoveDirection(Heading);
}
function _Teleport(entity:CNewNPC, pos:Vector)
{
  ((CActor)entity).Teleport(pos);
}
function _SetTypeMove(entity:CNewNPC, st:int)
{
  ((CActor)entity).GetMovingAgentComponent().SetGameplayRelativeMoveSpeed(st); //0 stay in place. 1 is for Walking, 2 jogging, 4 sprinting.
}
function _SetTypeMoveHorse(entity:CNewNPC, st:int)
{
  entity.GetHorseComponent().SetHorseWalkState(st); //0 stay in place. 1 is for Walking, 2 running.
}
function _SetCombatTarget(entity:CNewNPC, entity2:CNewNPC)
{
  //((CNewNPC)entity).SetCombatTarget(entity2);
}

//HOST OPERA
function _Data_AddNPC(e:CNewNPC)
{
    var data : Storage;
    data = getStorage();
    data.NpcEntities.PushBack(e);
    if(data.DebugMode)
        Log("[WITCHER3MP] Succesfully Added Player Entity to list");
}