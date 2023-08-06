function _SpawnIT(nameenti:string, npc_templ:string, xp:int, yp:int, zp:int) : CNewNPC
{
    var template: CEntityTemplate;
    var entity: CNewNPC;
    var position : Vector;
    var data : MyData;
    var tags : array<CName>;
    data = GetServerSettings();
    position = Vector(xp, yp, zp);
    template = (CEntityTemplate)LoadResource(npc_templ);
    entity = (CNewNPC)theGame.CreateEntity(template, position,EulerAngles(0, 0, 0),,,,PM_DontPersist, tags);
    onelinerEntity(nameenti, entity);//nickname
    return entity;
}
function _SetHeading(entity:CNewNPC, Heading:float)
{
  ((CActor)entity).GetMovingAgentComponent().SetGameplayMoveDirection(Heading);
}
function _SetMoveTo(aa:int, moveType:EMoveType, pos:Vector)
{
  var data : MyData;
  data = GetServerSettings();
  data.PlayersPOSITIONS[aa] = pos;
}
function _Teleport(entity:CNewNPC, pos:Vector)
{
    ((CActor)entity).Teleport(pos);
}
function _SetTypeMove(entity:CNewNPC, st:int)
{
  ((CActor)entity).GetMovingAgentComponent().SetGameplayRelativeMoveSpeed(st); //0 stay in place. 1 is for Walking, 2 jogging, 4 sprinting.
}
function _SetCombatTarget(entity:CNewNPC, entity2:CNewNPC)
{
  //((CActor)entity).SetCombatTarget(entity2);
}

//HOST OPERA
function _Data_AddPLAYER(e:CNewNPC)
{
    var data : MyData;
    data = GetServerSettings();
    data.PlayersDATAS.PushBack(e);
    data.PlayersPOSITIONS.PushBack(e.GetWorldPosition());
    if(data.DebugMode)
        Log("[WITCHER3MP] Succesfully Added Player Entity to list");
}
function _Data_AddHORSE(e:CNewNPC)
{
    var data : MyData;
    data = GetServerSettings();
    data.PlayersHORSES.PushBack(e);
    if(data.DebugMode)
        Log("[WITCHER3MP] Succesfully Added Player Entity to list");
}
function _RemovePlayer(e:CNewNPC)
{
    var data : MyData;
    data = GetServerSettings();
    data.PlayersDATAS.Remove(e);
    if(data.DebugMode)
        Log("[WITCHER3MP] Succesfully Removed Player From List");
}