exec function LoadServerSave(filename:string)
{
    var saves : array< SSavegameInfo >;
	var i : int;
	
	theGame.ListSavedGames(saves);
	for(i=0; i<saves.Size(); i+=1)
	{
		if(StrLower(saves[i].filename) == StrLower(filename))
		{
			theGame.StartNewGamePlus(saves[i]);
			return;
		}
	}
}
exec function Spawn_Player(nick:string, clientid:int, npc_templ:string, xp:int, yp:int, zp:int, xp2:int, yp2:int, zp2:int)
{
	_DATA_AddPlayer(_SetupPlayerObjectDefault(_SpawnIT(nick, npc_templ, Vector(xp, yp, zp), true)), _SpawnIT("Of player: " + nick + ":" + clientid, "horse", Vector(xp2, yp2, zp2), false), clientid);
	GetWitcherPlayer().DisplayHudMessage("Player: " + nick + " Joined to game");
    if(getStorage().DebugMode)
        Log("[WITCHER3MP] Spawned Entity With template " + npc_templ + " \nPosition X:" + xp + " Y:" + yp + " Z:" + zp);
}
exec function Spawn_NPC(npc_templ:string, xp:int, yp:int, zp:int, guid:int)
{
    _Data_AddNPC(_SpawnITNPC(npc_templ, Vector(xp, yp, zp), true), guid); // TODO: SPAWN NPC
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
exec function UnpauseGame()
{
    theGame.Unpause("menus");
    theGame.Unpause("IngameMenu");
    if(getStorage().DebugMode)
        Log("[WITCHER3MP] GameUnpaused ");
}
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
    if(npc_templ == "horse")
    {
        ((CActor)entity).ForceAIBehavior(new CAIHorseDoNothingAction in entity, BTAP_AboveEmergency2, 'AI_Rider_Load_Forced' );
	    ((CActor)entity).SignalGameplayEventParamInt('RidingManagerMountHorse', MT_instant | MT_fromScript );
    } else entity.EnablePhysicalMovement(true);
    if(getStorage().DebugMode)
        Log("[WITCHER3MP] Spawned Player OBJ: " + entity.GetGuidHash() + " TMPLATE: " + npc_templ + " Name: " + nameenti);
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
    ((CActor)entity).SetTemporaryAttitudeGroup('q104_avallach_friendly_to_all', AGP_Default);
    entity.SetBehaviorVariable('isOnHorse', 0.0);
    if(getStorage().DebugMode)
        Log("[WITCHER3MP] SpawnedNPC: " + entity.GetGuidHash() + " TMPLATE: " + npc_templ);
    return entity;
}
function _SpawnITNPCENEMY(npc_templ:string, position:Vector, fullname:bool) : CNewNPC
{
    var template: CEntityTemplate;
    var entity: CNewNPC;
    var data : Storage;
    var tags : array<CName>;
    data = getStorage();
    template = (CEntityTemplate)LoadResource(npc_templ, fullname);
    entity = (CNewNPC)theGame.CreateEntity(template, position,EulerAngles(0, 0, 0),,,,PM_DontPersist, tags);
    ((CActor)entity).SetTemporaryAttitudeGroup('q104_avallach_friendly_to_all', AGP_Default);
    if(getStorage().DebugMode)
        Log("[WITCHER3MP] SpawnedNPC Enemy: " + entity.GetGuidHash() + " TMPLATE: " + npc_templ);
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
	((CActor)entity).OnDeath(action);
  if(getStorage().DebugMode)
        Log("[WITCHER3MP] Spawned Entity Corpse! TMPLATE: " + npc_templ);
  return entity;
}