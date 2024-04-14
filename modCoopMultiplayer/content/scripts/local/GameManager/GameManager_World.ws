function _SpawnNPC(npc_templ:string, nameenti:string, position:Vector, fullname:bool, CanEnemy:bool) : CActor 
{
    var template: CEntityTemplate;
    var entity: CActor ;
    var tags : array<CName>;
    var data : MP_NPCSTORAGE;
    data = getStorage().getNPCStorage();
    template = (CEntityTemplate)LoadResource(npc_templ, fullname);
    entity = (CActor )theGame.CreateEntity(template, position,EulerAngles(0, 0, 0),,,,PM_DontPersist, tags);
    if(getStorage().DebugMode)
        Log("[WITCHER3MP] Spawned Player OBJ: " + entity.GetGuidHash() + " TMPLATE: " + npc_templ + " Name: " + nameenti);
    return entity;
}
function _SpawnCorpse(npc_templ:string, position:Vector) : CActor 
{
    var template: CEntityTemplate;
    var entity: CActor ;
    var tags : array<CName>;
    var data : MP_NPCSTORAGE;
    data = getStorage().getNPCStorage();
    template = (CEntityTemplate)LoadResource(npc_templ, false);
    entity = (CActor )theGame.CreateEntity(template, position,EulerAngles(0, 0, 0),,,,PM_DontPersist, tags);
    _KillNPC(entity);
    if(getStorage().DebugMode)
        Log("[WITCHER3MP] Spawned Entity Corpse! TEMPLATE: " + npc_templ);
    return entity;
}
function _GetNPC_DataByID(id:int) : string
{
    var pos : Vector;
    var entity: CActor ;
    var output: string;
    var data : MP_NPCSTORAGE;
    data = getStorage().getNPCStorage();
    entity = data.GetByID(id).Entity;
    pos = entity.GetWorldPosition();
    output += "@COMBATTARGPOS@: @"  + (int)pos.X + " " + (int)pos.Y + " " + (int)pos.Z + "@";
    output += ",@COMBATTARGNAME@: @" + (CActor)entity+ "@";
    output += ",@COMBATTARGGUID@: " + id;
    output += ",@COMBATTHP@: " + (int)entity.GetCurrentHealth();
    output += ",@ISBOSS@: " + (thePlayer.GetBossTag() != '');
    output += ",@ISDEAD@: " + entity.isDead;
    output += ",@ANGLE@: " + entity.GetHeading();
    return output;
}
function _GetTargetDataByNPC(entity:CActor ) : string
{
    return "W3MP:!COMBATDATA!{" + _GetNPC_DataByID(getStorage().getNPCStorage().GetIDByNPC(entity)) + "}";
}
function _GetNearTarget(entity:CActor , rangeL:int) : CActor  //NEAR TARGET around entity IN RANGER
{
    var actors : array<CActor>;
    var actor	: CActor;
    var prevdist, dist : float;
    var i : int;
    prevdist = 99;
    actors = GetActorsInRange(entity, rangeL);
    for( i = 0; i < actors.Size(); i+= 1 )
    {
        dist = VecDistance(actors[i].GetWorldPosition(), entity.GetWorldPosition());
        if(dist < prevdist && actors[i] != entity && actors[i] != thePlayer) //NEED FIX FOR SEARCH THE ONLY MONSTER / or enemy
        {
          actor = actors[i];
          prevdist = dist;
        }
  }
  return (CActor )actor;
}
function _GetNearTargets(entity:CActor , rangeL:int) : array<CActor > //NEAR TARGETS IN RANGE
{
    var actors : array<CActor>;
    var allnpc : array<CActor >;
    var data : MP_NPCSTORAGE;
    var actor	: CActor;
    var i : int;
    actors = GetActorsInRange(entity, rangeL);
    data = getStorage().getNPCStorage();
    allnpc.Clear();
    for(i = 0; i < actors.Size(); i+= 1)
    {
        data.Add((CActor )actors[i], NULL, "FromMPLister", (CActor )actors[i]);
        allnpc.PushBack((CActor )actors[i]);
    }
    return allnpc;
}