exec function GetClientState()
{
    Log("WITCHER-3MP " + GetServerSettings().ClientState);
}
exec function GetVersionMod() {
    Log("WITCHER-3MP 1.0");
}
exec function GetCurrentLevelId()
{
    Log("WITCHER-3MP " + theGame.GetCommonMapManager().GetCurrentArea());
}
exec function GetCurrentHP()
{
    Log("WITCHER-3MP " + GetWitcherPlayer().GetCurrentHealth());
}
exec function GetCurrentHeading()
{
    Log("WITCHER-3MP " + GetWitcherPlayer().GetHeading());
}
exec function GetCurrentPLevel()
{
    Log("WITCHER-3MP " + GetWitcherPlayer().GetLevel());
}
exec function GetCurrentPosition()
{
    Log("WITCHER-3MP " + VecToString(GetWitcherPlayer().GetWorldPosition()));
}
exec function GetCurrentHorsePosition()
{
    Log("WITCHER-3MP " + VecToString(GetWitcherPlayer().GetHorseWithInventory().GetWorldPosition()));
}
exec function IsOnHorse()
{
    Log("WITCHER-3MP " + GetWitcherPlayer().IsUsingHorse());
}
exec function GetCurrentRotation()
{
    var eu : EulerAngles;
    eu = GetWitcherPlayer().GetWorldRotation();
    Log("WITCHER-3MP " + eu.Yaw + " " + eu.Pitch + " " + eu.Roll);
}
exec function GetStateAnimInt()
{
    Log("WITCHER-3MP " + GetWitcherPlayer().GetActorAnimState());
}
exec function GetMovingTypeState()
{
    if(!GetWitcherPlayer().GetIsWalking() && !GetWitcherPlayer().GetIsRunning())
        Log("WITCHER-3MP 0");
    else if(GetWitcherPlayer().GetIsWalking() && !GetWitcherPlayer().GetIsRunning())
            Log("WITCHER-3MP 2");
    else if(GetWitcherPlayer().GetIsWalking() && GetWitcherPlayer().GetIsRunning())
            Log("WITCHER-3MP 4");
    else if(!GetWitcherPlayer().GetIsWalking() && GetWitcherPlayer().GetIsRunning())
            Log("WITCHER-3MP 1");
}
exec function GetStateAnimName()
{
    Log("WITCHER-3MP " + GetWitcherPlayer().GetCurrentStateName());
}
exec function IsInMenu()
{
    var currentMenu : CR4Menu;
    currentMenu = theGame.GetGuiManager().GetRootMenu();
    if (currentMenu)
        Log("WITCHER-3MP True");
    else
        Log("WITCHER-3MP False");
}
exec function IsGameStoped()
{
   Log("WITCHER-3MP " + !theGame.IsStopped());
}
exec function IsGamePaused()
{
    Log("WITCHER-3MP " + !theGame.IsPaused());
}
exec function GetActiveCombatStyle()
{
    //((CHumanAICombatStorage)combatDataStorage).GetActiveCombatStyle();
}
exec function GetMovingState()
{
    if (GetWitcherPlayer().GetIsWalking())
        Log("WITCHER-3MP " + (GetWitcherPlayer().GetIsRunning() ? 2 : 1));
    else
        Log("WITCHER-3MP " + 0);
}