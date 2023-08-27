latent function Update2()
{
   	var data : Storage;
   	var i : int;
   	var clid : int;
   	var DistanceP, DistancePH : float;
   	var positionP1, positionP2: Vector;
	var positionPH1, positionPH2: Vector;
   	var horseEntity, entityP : CNewNPC;
   	data = getStorage();
   	for (i = 0; i < data.PlayerIDS.Size(); i += 1) {
       	clid = data.PlayerIDS[i];
       	entityP = data.getPlayerOBJ(clid);
       	horseEntity = data.getPlayerHORSE(clid);
       	positionP1 = data.getPlayerPOS(clid);
		positionPH1 = data.getPlayerHorsePOS(clid);
       	positionP2 = ((CActor)entityP).GetWorldPosition();
		positionPH2 = ((CActor)horseEntity).GetWorldPosition();
		DistancePH = VecDistance(positionPH2, positionPH1);
		DistanceP = VecDistance(positionP2, positionP1);
       	if(!_IsOnHorse(entityP))
       	{
			//PLAYER POSITION 1.CHECK_WATER 2.SETPOS 3.HorsePos
			//1.
			if (theGame.GetWorld().GetWaterLevel(positionP1) > positionP1.Z-0.1)
			{
				//entityP.EnablePhysicalMovement(true);
				((CMovingPhysicalAgentComponent)entityP.GetMovingAgentComponent()).SetSwimming(true);
				((CMovingPhysicalAgentComponent)entityP.GetMovingAgentComponent()).SetDiving(true);
				((CMovingPhysicalAgentComponent)entityP.GetMovingAgentComponent()).SetGravity(false);
				((CActor)entityP).SetIsSwimming(true);
				
				entityP.ChangeStance(NS_Swim);
			}
			else {
				//entityP.EnablePhysicalMovement(false);
				((CActor)entityP).SetIsSwimming(false);
				((CMovingPhysicalAgentComponent)entityP.GetMovingAgentComponent()).SetGravity(true);
				entityP.ChangeStance(NS_Normal);
			}
			//2.
       	    if(DistanceP <= 2)
           	    _SetTypeMove(entityP, 0);
			else if(DistanceP < 5)
           	    _SetTypeMove(entityP, 2);
           	else if(DistanceP <= 10)
           	    _SetTypeMove(entityP, 4);
           	else if(DistanceP > 10)
               	_Teleport(entityP, positionP1);
			_SetHeading(entityP, positionP1 - positionP2);
			//3.
			if(DistancePH <= 3)
           	    _SetTypeMove(horseEntity, 0);
			else if(DistanceP < 6)
           	    _SetTypeMove(entityP, 2);
           	else if(DistancePH <= 11)
           	    _SetTypeMove(horseEntity, 4);
           	else if(DistancePH > 20)
               	_Teleport(horseEntity, positionPH1);
			_SetHeading(horseEntity, positionPH1 - positionPH2);
       	} else {
			//3.
           	if(DistancePH <= 3)
           	    _SetTypeMove(horseEntity, 0);
			else if(DistanceP < 6)
           	    _SetTypeMove(entityP, 2);
           	else if(DistancePH <= 11)
           	    _SetTypeMove(horseEntity, 4);
           	else if(DistancePH > 20)
               	_Teleport(horseEntity, positionPH1);
			_SetHeading(horseEntity, positionPH1 - positionPH2);
        }
    }
}