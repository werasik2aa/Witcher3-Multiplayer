/*exec function SetActionsByState_Player(id:int, act:string)
{
    var entity : CEntity;
    var data: MyModData;
    var stupidArray : array<name>;
    var inv	: CInventoryComponent;
    var riderData : CAIStorageRiderData;
    var vehicleComponent : CVehicleComponent;
    var vehicleEntity : CEntity;
    var item : SItemUniqueId;
    var horseComponent : W3HorseComponent;
    data = getStorage();
    entity = getStorage().PlayersData[id];

    if(act == "sword_1handed" || act == "sword_2handed" || act == "TwoHanded" || act == "Bow")
    {
        inv = ((CActor)entity).GetInventory();
        item = inv.GetItemFromSlot('r_weapon');
	    if (inv.IsIdValid(item) && inv.ItemHasTag(item,'sword1h'))
	        stupidArray.PushBack('sword_1handed');
	    else {
            item = inv.GetItemFromSlot('l_weapon');
            if (inv.IsIdValid(item) && inv.ItemHasTag(item,'sword1h'))
                stupidArray.PushBack('sword_1handed');
        }
    } else if(act == "FistFight") {
        stupidArray.PushBack('Exploration');
    } else if(act == "HorseVeh") {
        //auhumancombatstrg GetActiveCombatStyle()
        riderData = entity.GetRiderData();
        vehicleEntity = riderData.sharedParams.GetHorse();
        vehicleComponent = ((CNewNPC)vehicleEntity).GetHorseComponent();
        horseComponent = (W3HorseComponent)vehicleComponent;
        ((CActor)entity).MountActor(riderData, 'VehicleHorse', vehicleComponent);
    } else
        stupidArray.PushBack('Exploration');
    ((CActor)entity).ActivateBehaviorsSync(stupidArray);
    //((CActor)entity).SetBehaviorMimicVariable('gameplayMimicsMode', (float)(int)GMM_Combat);
    //((CActor)entity).SetBehaviorMimicVariable('gameplayMimicsMode', (float)(int)GMM_Default);
    //((CActor)entity).SetBehaviorMimicVariable('gameplayMimicsMode', (float)(int)PGMM_None);
    //((CActor)entity).SetBehaviorMimicVariable('gameplayMimicsMode', (float)(int)PGMM_Default);
    //((CActor)entity).SetBehaviorMimicVariable('gameplayMimicsMode', (float)(int)PGMM_None);
    //((CActor)entity).SetBehaviorMimicVariable('gameplayMimicsMode', (float)(int)GMM_Death);
    //((CActor)entity).SetBehaviorMimicVariable('gameplayMimicsMode', (float)(int)PGMM_Inventory);
    //((CActor)entity).GetMovingAgentComponent().SetGameplayRelativeMoveSpeed(act); // 1 is for Walking, 2 jogging, 4 sprinting.
    //((CActor)entity).ActivateBehaviorsSync(act);
    if(getStorage().DebugMode)
        Log("[WITCHER3MP] |CURSTATE: " + act);
}*/
exec function MountHorse(idcl:int, istate:int)
{
    var data : Storage;
    var entityP : CNewNPC;
	var vehicle : CVehicleComponent;
    var vehicleEnti : CNewNPC;
    data = getStorage();
    entityP = data.getPlayerOBJ(idcl);
    vehicleEnti = data.getPlayerHORSE(idcl);
	vehicle = vehicleEnti.GetHorseComponent();
    _SetTypeMove(entityP, 0);
    entityP.SetBehaviorVariable('Editor_MoveSpeed', 0);
    vehicleEnti.SetBehaviorVariable('Editor_MoveSpeed', 0);
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
        //entityP.GetState('MountHorse')).SetupState(this, mountType, EVS_driver_slot);
    }
    else
        entityP.BreakAttachment();
    if(data.DebugMode)
        Log("[WITCHER3MP] PlayerID: " + idcl + " Set MountState: " + (istate == 1) + " HORSE: " + vehicle);
}