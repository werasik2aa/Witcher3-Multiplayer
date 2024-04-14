//FUNCTIONS
function _SetupPlayerObjectDefault(entity:CActor ) : CActor  //Add Swords to back of PlayerObj :TODO: CHANGE THE WEARING
{
	var ids : array<SItemUniqueId>;
	var inv : CInventoryComponent;
    inv = entity.GetInventory();
	ids = inv.AddAnItem('Autogen steel sword', 1); entity.EquipItem(ids[0]);
	ids = inv.AddAnItem('Autogen silver sword', 1); entity.EquipItem(ids[0]);
	ids = inv.AddAnItem('Autogen Pants', 1); entity.EquipItem(ids[0]);
	ids = inv.AddAnItem('Autogen Gloves', 1); entity.EquipItem(ids[0]);
	ids = inv.AddAnItem('Autogen Boots', 1); entity.EquipItem(ids[0]);
	ids = inv.AddAnItem('Autogen Armor', 1); entity.EquipItem(ids[0]);
    return entity;
}
function _SetHeading(entity:CActor , targpos:Vector)
{
    var mvADJ : CMovementAdjustor; 
    ((CActor)entity).GetMovingAgentComponent().SetGameplayMoveDirection(VecHeading(targpos - entity.GetWorldPosition()));
    if(getStorage().DebugMode)
        Log("[WITCHER3MP] Setted Heading: " + VecHeading(targpos - entity.GetWorldPosition()));
}
function _Teleport(entity:CActor , pos:Vector)
{
  ((CActor)entity).Teleport(pos);
  if(getStorage().DebugMode)
        Log("[WITCHER3MP] Teleported TO: " + VecToString(pos));
}
function _MoveEntity(entity:CActor , targpos:Vector, SpeedOnDist:float, MinDistance:float, MaxDistance:int, Angle:float, optional DirectCtrl:bool)
{
    var dist, waterLevel : float;
    var entpos : Vector;
    var mvADJ : CMovementAdjustor; 
    var ticket : SMovementAdjustmentRequestTicket; 
    var entityPAC : CMovingPhysicalAgentComponent;
    entpos = entity.GetWorldPosition();
    dist = VecDistance(entpos, targpos); //Than far - then faster.
    mvADJ = entity.GetMovingAgentComponent().GetMovementAdjustor();
    if(SpeedOnDist > 0) dist = SpeedOnDist;//SET CUSTOM SPEED

    //ROTATE WITH DEFAULT SPEED AND SETTING
    _SetHeading(entity, targpos);

    if(dist > MinDistance) //SET WALK STATE
        _SetTypeMove(entity, (int)(dist*10)); //??
    else
        _SetTypeMove(entity, 0); //NOWALK

    entityPAC = ((CMovingPhysicalAgentComponent)entity.GetMovingAgentComponent());
    waterLevel = theGame.GetWorld().GetWaterLevel(entpos);
	if(waterLevel >= entpos.Z - 0.1 && !entity.IsSwimming()) 
	{
		entityPAC.SetSwimming(true);
		entityPAC.SetDiving(true);
		entityPAC.SetGravity(false);
		entityPAC.SetAnimatedMovement(false);
		entity.GotoState('Swimming');
	}
	if(waterLevel < entpos.Z - 0.1 && entity.IsSwimming())
	{
		entityPAC.SetGravity(false);
		entityPAC.SetSwimming(false);
		entityPAC.SetDiving(false);
		entity.GotoState('Exploration');
	}

    mvADJ.Cancel(mvADJ.GetRequest('W3MP_MOVEMENT'));
    ticket = mvADJ.CreateNewRequest('W3MP_MOVEMENT');
    mvADJ.AdjustmentDuration(ticket, 1);
    mvADJ.ShouldStartAt(ticket, targpos);
    mvADJ.MaxRotationAdjustmentSpeed(ticket, 50000);
    mvADJ.MaxLocationAdjustmentSpeed(ticket, dist * 50000);
    mvADJ.AdjustLocationVertically(ticket, true);
    mvADJ.ScaleAnimationLocationVertically(ticket, true);
    mvADJ.RotateTo(ticket, Angle); //DUNGEON MASTER IS FUCKING SLAVE!
    mvADJ.SlideTo(ticket, targpos + Vector(0, 1, 0));

    if(dist > MaxDistance+5) // TELEPORT IF hight range between object
        _Teleport(entity, targpos);
}
function _SetTypeMove(entity:CActor , st:int)
{
  ((CActor)entity).GetMovingAgentComponent().SetGameplayRelativeMoveSpeed(st); //0 stay in place. 1 is for Walking, 2 jogging, 4 sprinting.
  if(getStorage().DebugMode)
        Log("[WITCHER3MP] SetTypeMove: " + st + " ofEntityGUID: " + entity.GetGuidHash());
}
function _SetHealthEnemy(entity:CActor , hp:int)
{
    entity.SetHealthPerc(hp);
}
function _SetLevel(entity:CActor , LVL:int)
{
    ((CNewNPC)entity).SetLevel(LVL);
}
function _SendDamage(entity:CActor , ClientSHealth:int)
{
    entity.DrainVitality(entity.GetCurrentHealth() - ClientSHealth); //Percent of client healthenemy and Get value of damage
}
function _IsOnHorse(entity:CActor ) : bool
{
  return ((CActor)entity).HasTag('W3MPisOnHorsetrue');
}
function _SetCurrentHp(entity:CActor , health:int)
{
    entity.SetHealthPerc(health);
    if(entity.IsAlive() && health <= 0) entity.SetAlive(false);
    if(!entity.IsAlive() && health > 0) entity.SetAlive(true);
}