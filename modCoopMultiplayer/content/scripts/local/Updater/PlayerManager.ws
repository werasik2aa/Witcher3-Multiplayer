latent function Update2()
{
   	var data : Storage;
   	var i : int;
   	var clid : int;
   	var positionP: Vector;
	var positionPH: Vector;
   	var horseEntity, entityP : CNewNPC;
   	data = getStorage();
   	for (i = 0; i < data.PlayerIDS.Size(); i += 1) {
       	clid = data.PlayerIDS[i];
       	entityP = data.getPlayerOBJ(clid);
       	horseEntity = data.getPlayerHORSE(clid);
       	positionP = data.getPlayerPOS(clid);
		positionPH = data.getPlayerHorsePOS(clid);
       	if(!_IsOnHorse(entityP))
       	{
			//PLAYER POSITION 1.CHECK_WATER 2.SETPPOS 3.SetHorsePos
			//1.
			if (theGame.GetWorld().GetWaterLevel(positionP) > positionP.Z-0.5)
			{
				//entityP.EnablePhysicalMovement(true);
				((CMovingPhysicalAgentComponent)entityP.GetMovingAgentComponent()).SetSwimming(true);
				((CMovingPhysicalAgentComponent)entityP.GetMovingAgentComponent()).SetDiving(true);
				((CMovingPhysicalAgentComponent)entityP.GetMovingAgentComponent()).SetGravity(false);
				((CActor)entityP).SetIsSwimming(true);
				entityP.SetBehaviorVariable('shallowWater',1.0);
				entityP.ChangeStance(NS_Swim);
			}
			else {
				//entityP.EnablePhysicalMovement(false);
				((CActor)entityP).SetIsSwimming(false);
				((CMovingPhysicalAgentComponent)entityP.GetMovingAgentComponent()).SetGravity(true);
				entityP.ChangeStance(NS_Normal);
				entityP.SetBehaviorVariable('shallowWater', 0.0);
			}
			//2.SETPPOS | 3.SetHorsePos
       	    _MoveEntity(entityP, positionP, -10, 1, 20);
       	    _MoveEntity(horseEntity, positionPH, -10, 2, 20);
       	} else //3.SetHorsePos
       	    _MoveEntity(horseEntity, positionPH, -10, 2, 20);//3.
    }
}