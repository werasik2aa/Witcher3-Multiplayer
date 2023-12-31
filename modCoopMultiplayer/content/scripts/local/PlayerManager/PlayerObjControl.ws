latent function Update2()
{
   	var data : Storage;
   	var i, clid : int;
	var waterLevel : float;
   	var positionP, positionPH : Vector;
   	var horseEntity, entityP : CNewNPC;
	var entityPAC : CMovingPhysicalAgentComponent;
   	data = getStorage();
   	for (i = 0; i < data.PlayerIDS.Size(); i += 1) {
       	clid = data.PlayerIDS[i];
       	entityP = data.getPlayerOBJ(clid);
       	horseEntity = data.getPlayerHORSE(clid);
       	positionP = data.getPlayerPOS(clid);
		positionPH = data.getPlayerHorsePOS(clid);
		entityPAC = ((CMovingPhysicalAgentComponent)entityP.GetMovingAgentComponent());
       	if(!_IsOnHorse(entityP))
       	{
			//PLAYER POSITION 1.CHECK_WATER and other things 2.SETPPOS 3.SetHorsePos
			//1.
			waterLevel = theGame.GetWorld().GetWaterLevel(positionP);
			if(waterLevel > positionP.Z - 0.1 && !entityP.IsSwimming()) 
			{
				entityP.ChangeStance(NS_Swim);
				entityPAC.SetSwimming(true);
				entityPAC.SetDiving(true);
				entityPAC.SetGravity(false);
				entityPAC.SetAnimatedMovement(false);
				entityP.GotoState('Swimming');
			}
			if(waterLevel < positionP.Z - 0.1 && entityP.IsSwimming())
			{
				entityP.ChangeStance(NS_Normal);
				entityPAC.SetGravity(false);
				entityPAC.SetSwimming(false);
				entityPAC.SetDiving(false);
				entityP.GotoState('Exploration');
			}
			//2.SETPPOS | 3.SetHorsePos
			//_MoveEntity(entity, targetpos, ownspeed, mindist, maxdist, DirectOrAuto(BOOL))	
       	    _MoveEntity(entityP, positionP, 0, 1, 20, true);
       	    _MoveEntity(horseEntity, positionPH, 0, 2, 20, false);
       	} else //3.SetHorsePos
       	    _MoveEntity(horseEntity, positionPH, 0, 2, 20, true);//3.
    }
}