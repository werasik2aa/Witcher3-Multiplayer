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
    var prevstate : int;
    var vehicleEntity : CEntity;
    var vehicleComponent : CVehicleComponent;
    var horseComponent : W3HorseComponent;
    var entity : CEntity;
    var spawnedhorse : CNewNPC;
    data = getStorage();
    entity = data.getPlayerOBJ(idcl);
    prevstate = (int)entity.GetBehaviorVariable('isOnHorse');
    vehicleEntity = data.getPlayerHORSE(idcl);
    if(prevstate != istate) {
        entity.SetBehaviorVariable('isOnHorse', 1.0);
        vehicleComponent = ((CNewNPC)vehicleEntity).GetHorseComponent();
        horseComponent = (W3HorseComponent)vehicleComponent;
        vehicleComponent.Mount((CActor)entity, VMT_ImmediateUse, EVS_driver_slot);
    } else
        entity.SetBehaviorVariable('isOnHorse', 0.0);
}