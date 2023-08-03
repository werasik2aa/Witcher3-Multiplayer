class ModSettingsTemplate {
  public var Header : String;
}
function GetServerSettings() : MyModData {
  var data : MyModData;
  data = (MyModData) getStorage().getItem("ServerSettings");
  if (!data) {
    data = new MyModData in thePlayer;
    getStorage().setItem(data);
  }
  return data;
}

function UpdateServerSettings(data : MyModData)
{
    getStorage().Replace(data);
}



///HERE MY DATA
class MyModData extends ModSettingsTemplate 
{
  default Header = "ServerSettings";
  public var ClientState : int;
  public var PlayersEntities : array<CEntity>;
  public var Name, Author, Version : String;
  public var DebugMode : bool;
  public var MaxPlayers, CurPlayers : int;
}