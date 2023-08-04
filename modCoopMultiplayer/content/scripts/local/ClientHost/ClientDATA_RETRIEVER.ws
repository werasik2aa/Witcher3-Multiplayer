exec function GetClientState()
{
     var data : MyModData;
     data = GetServerSettings();
     Log("WITCHER-3MP " + data.ClientState);
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
exec function GetCurrentPLevel()
{
    Log("WITCHER-3MP " + GetWitcherPlayer().GetLevel());
}
exec function GetCurrentPosition()
{
    Log("WITCHER-3MP " + VecToString(GetWitcherPlayer().GetWorldPosition()));
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