exec function GetClientState()
{
    Log("W3MP " + getStorage().ClientState);
}
exec function GetVersionMod() {
    Log("W3MP 1.0");
}
exec function GetCurrentLevelId()
{
    Log("W3MP " + theGame.GetCommonMapManager().GetCurrentArea());
}
exec function GetCurrentHP()
{
    Log("W3MP " + GetWitcherPlayer().GetCurrentHealth());
}
exec function GetCurrentHeading()
{
    Log("W3MP " + GetWitcherPlayer().GetHeading());
}
exec function GetCurrentPLevel()
{
    Log("W3MP " + GetWitcherPlayer().GetLevel());
}
exec function GetCurrentPosition()
{
    var pos:Vector;
    pos = GetWitcherPlayer().GetWorldPosition();
    Log("W3MP " + (int)pos.X + " " + (int)pos.Y + " " + (int)pos.Z);
}
exec function GetCurrentHorsePosition()
{
    Log("W3MP " + VecToString(GetWitcherPlayer().GetHorseWithInventory().GetWorldPosition()));
}
exec function IsOnHorse()
{
    Log("W3MP " + GetWitcherPlayer().IsUsingHorse());
}
exec function GetCurrentRotation()
{
    var eu : EulerAngles;
    eu = GetWitcherPlayer().GetWorldRotation();
    Log("W3MP " + eu.Yaw + " " + eu.Pitch + " " + eu.Roll);
}
exec function GetStateAnimInt()
{
    Log("W3MP " + GetWitcherPlayer().GetActorAnimState());
}
exec function GetMovingTypeState()
{
    if(!GetWitcherPlayer().GetIsWalking() && !GetWitcherPlayer().GetIsRunning())
        Log("W3MP 0");
    else if(GetWitcherPlayer().GetIsWalking() && !GetWitcherPlayer().GetIsRunning())
            Log("W3MP 2");
    else if(GetWitcherPlayer().GetIsWalking() && GetWitcherPlayer().GetIsRunning())
            Log("W3MP 4");
    else if(!GetWitcherPlayer().GetIsWalking() && GetWitcherPlayer().GetIsRunning())
            Log("W3MP 1");
}
exec function GetStateAnimName()
{
    Log("W3MP " + GetWitcherPlayer().GetCurrentStateName());
}
exec function IsInMenu()
{
    var currentMenu : CR4Menu;
    currentMenu = theGame.GetGuiManager().GetRootMenu();
    if (currentMenu)
        Log("W3MP True");
    else
        Log("W3MP False");
}
exec function IsGameStoped()
{
   Log("W3MP " + !theGame.IsStopped());
}
exec function IsGamePaused()
{
    Log("W3MP " + !theGame.IsPaused());
}
exec function GetCombatTargetGuid()
{
    var data : Storage;
    data = getStorage();
    if(GetWitcherPlayer().GetTarget())
        if(!data.getNPC(GetWitcherPlayer().GetTarget().GetGuidHash()))
            data.addNPC((CNewNPC)GetWitcherPlayer().GetTarget(), GetWitcherPlayer().GetTarget().GetGuidHash());
    Log("W3MP " + GetWitcherPlayer().GetTarget().GetGuidHash());
}
exec function GetCombatTargetName()
{
    var data : Storage;
    data = getStorage();
    if(GetWitcherPlayer().GetTarget())
        if(!data.getNPC(GetWitcherPlayer().GetTarget().GetGuidHash()))
            data.addNPC((CNewNPC)GetWitcherPlayer().GetTarget(), GetWitcherPlayer().GetTarget().GetGuidHash());
    Log("W3MP " + GetWitcherPlayer().GetTarget().GetName());
}
exec function GetFightSwStateName()
{
    var cact : EBufferActionType;
    cact = GetWitcherPlayer().GetCombatAction();
    if(cact == EBAT_EMPTY)
        Log("W3MP 0");
    if(cact == EBAT_LightAttack)
        Log("W3MP 1");
    if(cact == EBAT_HeavyAttack)
        Log("W3MP 2");
    if(cact == EBAT_SpecialAttack_Light)
        Log("W3MP 3");
    if(cact== EBAT_SpecialAttack_Heavy)
        Log("W3MP 4");
    if(cact == EBAT_Parry)
        Log("W3MP 5");
    if(cact == EBAT_Dodge)
        Log("W3MP 6");
}
exec function IsCombatTargetDead(guid:int)
{
    var data : Storage;
    data = getStorage();
    Log("W3MP " + data.getNPC(guid).isDead);
}
exec function IsInCombat()
{
    Log("W3MP " + GetWitcherPlayer().IsInCombatAction());
}
exec function GG() : int
{
    return 1;
}