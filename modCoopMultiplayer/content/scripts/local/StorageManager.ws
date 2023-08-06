class MyData {
  public var ClientState : int;
  public var PlayersPOSITIONS : array<Vector>;
  public var PlayersHORSES : array<CNewNPC>;
  public var PlayersDATAS : array<CNewNPC>;
  public var NpcEntities : array<CNewNPC>;
  public var Name, Author, Version : String;
  public var DebugMode : bool;
  public var MaxPlayers, CurPlayers : int;
  public var CurrentPlayerEntityNoOverride : CEntity;
  public var MyPos : Vector;
}
function GetServerSettings() : MyData //Instantiace the settings iF NOT EXIST AND get data
{
  var data: MyData;
  var player_input : CPlayerInput;
  player_input = thePlayer.GetInputHandler();
  if (!player_input.MYDATAstorage)
    player_input.MYDATAstorage = new MyData in thePlayer;
  data = player_input.MYDATAstorage;
  return data;
}