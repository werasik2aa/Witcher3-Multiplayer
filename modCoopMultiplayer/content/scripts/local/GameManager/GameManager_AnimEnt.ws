exec function SetMovingTypePlayer(id:int, act:int)
{
    var entity : CEntity;
    var data: MyModData;
    data = GetServerSettings();
    entity = data.PlayersEntities[id];
    ((CActor)entity).GetMovingAgentComponent().SetGameplayRelativeMoveSpeed(act);
    if(data.DebugMode)
        Log("[WITCHER3MP] 1 is for Walking, 2 jogging, 4 sprinting |CURSTATE: " + id);
}
exec function SetMovingTypeNPC(id:int, act:int)
{
    var data: MyModData;
    data = GetServerSettings();
    _SetTypeMove(data.PlayersEntities[id], act);
    if(data.DebugMode)
        Log("[WITCHER3MP] 1 is for Walking, 2 jogging, 4 sprinting |CURSTATE: " + id);
}
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
    data = GetServerSettings();
    entity = data.PlayersEntities[id];

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
    if(data.DebugMode)
        Log("[WITCHER3MP] |CURSTATE: " + act);
}*/