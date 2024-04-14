exec function MountHorse(idcl:int, istate:int)
{
    var data : MP_NPCSTORAGE;
    var entityP : CActor ;
	var vehicle : CVehicleComponent;
    var vehicleEnti : CNewNPC;
    data = getStorage().getPLAYERStorage();
    entityP = data.GetByID(idcl).Entity;
    vehicleEnti = (CNewNPC)data.GetByID(idcl).H_Entity;
	vehicle = vehicleEnti.GetHorseComponent();
    _SetTypeMove(entityP, 0);
    entityP.SetBehaviorVariable('NPCrider',istate);
    entityP.SetBehaviorVariable('isOnHorse',istate);
    entityP.EnableCollisions(istate == 1);
    ((CActor)entityP).EnablePhysicalMovement(istate == 1);
    if(istate == 1) 
    {
        vehicle.StopTheVehicle();
        vehicle.AttachEntity(entityP);//AttachTo Vehicle object
        vehicle.OnMountStarted((CActor)entityP, EVS_driver_slot);
        ((CActor)entityP).SetUsedVehicle((CGameplayEntity)vehicleEnti);
        vehicle.Mount((CActor)entityP, VMT_TeleportAndMount, EVS_driver_slot);//Just teleports to vehicle
		if (((CActor)entityP).GetMovingAgentComponent().GetMovementAdjustor()) ((CActor)entityP).GetMovingAgentComponent().GetMovementAdjustor().CancelAll(); //CANCEL ALL MOVEMENT EVENTS
        vehicle.OnMountFinished((CActor)entityP);
        entityP.GotoState('HorseAction');
		vehicle.Mount((CActor)entityP, VMT_ImmediateUse, EVS_driver_slot);
        ((CActor)entityP).AddTag('W3MPisOnHorsetrue');
    }
    else {
        entityP.BreakAttachment();
        ((CActor)entityP).RemoveTag('W3MPisOnHorsetrue');
    }
    if(getStorage().DebugMode)
        Log("[WITCHER3MP] PlayerID: " + idcl + " Set MountState: " + (istate == 1) + " HORSE: " + vehicle);
}