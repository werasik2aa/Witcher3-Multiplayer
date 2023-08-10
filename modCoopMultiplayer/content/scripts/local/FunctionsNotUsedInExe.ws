function _SpawnIT(nameenti:string, npc_templ:string, position:Vector, fullname:bool) : CNewNPC
{
    var template: CEntityTemplate;
    var entity: CNewNPC;
    var data : Storage;
    var tags : array<CName>;
    data = getStorage();
    template = (CEntityTemplate)LoadResource(npc_templ, fullname);
    entity = (CNewNPC)theGame.CreateEntity(template, position,EulerAngles(0, 0, 0),,,,PM_DontPersist, tags);
    ((CActor)entity).SetTemporaryAttitudeGroup('npc_charmed', AGP_Axii);
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
  ((CActor)entity).SetTemporaryAttitudeGroup('npc_charmed', AGP_Axii);
  if( VecDistance(entity.GetWorldPosition(), entity2.GetWorldPosition()) > 2)
    _Teleport(entity, entity2.GetWorldPosition());
  ((CActor)entity).SetAttitude( (CActor)entity2, AIA_Hostile );
}
function _ResetCombatTarget(entity:CNewNPC)
{
  ((CActor)entity).SetTemporaryAttitudeGroup('q104_avallach_friendly_to_all', AGP_Default);
}
function _CheckOrSpawnNPC(GuidHash:int, entiname:string, entity:CNewNPC)
{
  var action		: W3DamageAction;
	var i 			: int;
	var actors		: array<CActor>;
	var actor		: CActor;
	var currhash    : int;
	var data 		: Storage;
	data = getStorage();
	actors = GetActorsInRange(entity, 100);
	actor = (CActor)data.getNPC(GuidHash);
	if(!actor)
	{
		for	( i = 0; i < actors.Size(); i+= 1 )
		{
			actor = (CActor)actors[i];
			currhash = actor.GetGuidHash();
			if (currhash == GuidHash)
				return;
		}
    	data.addNPC((CNewNPC)_SpawnITNPC(entiname, entity.GetWorldPosition(), false), GuidHash);
	}
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