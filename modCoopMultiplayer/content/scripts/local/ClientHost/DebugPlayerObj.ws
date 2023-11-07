exec function GetPlayerPos(idcl:int)
{
    var data : Storage;
    data = getStorage();
    Log("[WITCHER3MP] Player Position: " + VecToString(data.getPlayerPOS(idcl)));
}
exec function GetPlayerVariableValue(idcl:int, caname:name)
{
    var data : Storage;
    data = getStorage();
    Log("[WITCHER3MP] Value: " + data.getPlayerOBJ(idcl).GetBehaviorVariable(caname));
}
exec function GetPlayerHasTag(idcl:int, caname:name)
{
    var data : Storage;
    data = getStorage();
    Log("[WITCHER3MP] TagExist: " + data.getPlayerOBJ(idcl).HasTag(caname));
}
exec function GetPlayerIsOnHorse(idcl:int)
{
    var data : Storage;
    data = getStorage();
    Log("[WITCHER3MP] IsOnHorse: " + _IsOnHorse(data.getPlayerOBJ(idcl)));
}
exec function GetPlayerTargetName(idcl:int)
{
    var data : Storage;
    data = getStorage();
    Log("[WITCHER3MP] CombatTargetTemplate: " + data.getPlayerOBJ(idcl).GetTarget());
}
exec function GetPlayerTargetPos(idcl:int)
{
    var data : Storage;
    data = getStorage();
    Log("[WITCHER3MP] " + VecToString(data.getPlayerOBJ(idcl).GetTarget().GetWorldPosition()));
}
exec function GetPlayerTargetDist(idcl:int)
{
    var data : Storage;
    data = getStorage();
    Log("[WITCHER3MP] CombatDistance: " + VecDistance(data.getPlayerOBJ(idcl).GetTarget().GetWorldPosition(), data.getPlayerOBJ(idcl).GetWorldPosition()));
}
exec function GetPlayerHorsePosWorld(idcl:int)
{
    var data : Storage;
    data = getStorage();
    Log("[WITCHER3MP] Player HorsePos:" + VecToString(data.getPlayerHORSE(idcl).GetWorldPosition()));
}
exec function GetPlayerHorsePos(idcl:int)
{
    var data : Storage;
    data = getStorage();
    Log("[WITCHER3MP] Player HorsePos:" + VecToString(data.getPlayerHorsePOS(idcl)));
}
exec function GetPlayerHorseName(idcl:int)
{
    var data : Storage;
    data = getStorage();
    Log("[WITCHER3MP] Player HorseNameTempl" + data.getPlayerHORSE(idcl));
}
exec function IsPlayerSwim(idcl:int)
{
    var data : Storage;
    var entityP:CNewNPC;
    var pos:Vector;
    entityP = data.getPlayerOBJ(idcl);
    pos = entityP.GetWorldPosition();
    Log("[WITCHER3MP] Player IN water?:" + (theGame.GetWorld().GetWaterLevel(pos) > pos.Z - 0.5));
    
}
exec function SetPlayerSwim(idcl:int, istate:int)
{
    var data : Storage;
    var entityP:CNewNPC;
    var positionP1 : Vector;
    data = getStorage();
    entityP = data.getPlayerOBJ(idcl);
    positionP1 = entityP.GetWorldPosition();
    if (istate == 1)
	{
	    entityP.EnablePhysicalMovement(true);
		((CMovingPhysicalAgentComponent)entityP.GetMovingAgentComponent()).SetSwimming(true);
		((CMovingPhysicalAgentComponent)entityP.GetMovingAgentComponent()).SetDiving(true);
		((CMovingPhysicalAgentComponent)entityP.GetMovingAgentComponent()).SetGravity(false);
		entityP.ChangeStance(NS_Swim);
	}
	else {
		entityP.EnablePhysicalMovement(false);
		((CMovingPhysicalAgentComponent)entityP.GetMovingAgentComponent()).SetGravity(true);
		entityP.ChangeStance(NS_Normal);
	}
    Log("[WITCHER3MP] InWater: " + (theGame.GetWorld().GetWaterLevel(positionP1) > positionP1.Z + 2) + " ForceKey: " + istate);
}
exec function NametagEntityV1(idcl:int, nameenti:string)
{
    var data : Storage;
    data = getStorage();
    onelinerEntity(nameenti, data.getPlayerOBJ(idcl));//nickname
}