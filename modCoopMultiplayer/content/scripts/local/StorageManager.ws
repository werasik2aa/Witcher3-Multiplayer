class MyData {
  public var ClientState : int;
  public var PlayersDATAS : array<PlayerDataStruct>;
  public var NpcEntities : array<CEntity>;
  public var Name, Author, Version : String;
  public var DebugMode : bool;
  public var MaxPlayers, CurPlayers : int;
  public var CurrentPlayerEntityNoOverride : CEntity;
  public var MyPos : Vector;
}
class PlayerDataStruct extends MyData {
  editable var Object : CEntity;
  editable var NickName : String;
  editable var ClientID : String;
  editable var moveType : EMoveType;
  editable var pos : Vector;
  editable var rot : int;
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

function UpdateServerSettings(Data:MyData) //GETCLASS
{
  var player_input : CPlayerInput;
  player_input = thePlayer.GetInputHandler();
  if (!player_input.MYDATAstorage)
    player_input.MYDATAstorage = new MyData in thePlayer;
  player_input.MYDATAstorage = Data;
}