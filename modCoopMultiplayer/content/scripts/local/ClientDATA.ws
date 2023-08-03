exec function GetClientState()
{
     var data : MyModData;
     data = GetServerSettings();
     Log("WITCHER-3MP " + data.ClientState);
}
exec function GetVersionMod() {
    Log("WITCHER-3MP 1.0");
}
exec function GetLevelId()
{
    Log("WITCHER-3MP " + theGame.GetCommonMapManager().GetCurrentArea());
}
exec function GetMyCurrentHP()
{
    Log("WITCHER-3MP " + GetWitcherPlayer().GetCurrentHealth());
}
exec function GetMyCurrentPLevel()
{
    Log("WITCHER-3MP " + GetWitcherPlayer().GetLevel());
}
exec function GetMyPosition()
{
    Log("WITCHER-3MP " + VecToString(GetWitcherPlayer().GetWorldPosition()));
}
exec function GetMyRotation()
{
    var eu : EulerAngles;
    eu = GetWitcherPlayer().GetWorldRotation();
    Log("WITCHER-3MP " + eu.Yaw + " " + eu.Pitch + " " + eu.Roll);
}
exec function GetStateAnim()
{
    Log("WITCHER-3MP " + GetWitcherPlayer().GetActorAnimState());
}
exec function IsInMenu()
{
    var currentMenu : CR4Menu;
    currentMenu = theGame.GetGuiManager().GetRootMenu();
    if (currentMenu)
    {
        Log("WITCHER-3MP: True");
    } else {
        Log("WITCHER-3MP: False");
    }
}
exec function IsGameStoped()
{
   Log("WITCHER-3MP: " + !theGame.IsStopped());
}