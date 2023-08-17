///FUNCTIONS
function getStorage(): Storage {
	return getStorageFromInput(thePlayer.GetInputHandler());
}
function getStorageFromInput(player_input: CPlayerInput): Storage {
	if (!player_input.storage) player_input.storage = new Storage in player_input;
	return player_input.storage;
}
function getBuffer(): Storage {
	return getBufferFromInput(thePlayer.GetInputHandler());
}
function getBufferFromInput(player_input: CPlayerInput): Storage {
	if (!player_input.buffer)player_input.buffer = new Storage in player_input;
	return player_input.buffer;
}
function _sustuninstall(Header: string) {
	var storage: Storage;
	storage = getStorage();
	if (storage.hasItem(Header))
		storage.removeItem(storage.getItem(Header));
}
function AddNewPlayerToStorage(obj:CNewNPC, objhorse:CNewNPC, idcl:int) {
  getStorage().addPlayer(obj, objhorse, idcl);
}
///CLASSES
class StorageItem {
  public var Header: string;
}
class Storage {
  public var items: array<StorageItem>;
  public var ClientState : int;
  public var PlayerOBJECTS : array<CNewNPC>;
  public var PlayerPOSITIONS : array<Vector>;
  public var PlayerHORSES : array<CNewNPC>;
  public var PlayerHORSESPOSITION : array<Vector>;
  public var PlayerIDS : array<int>;
  public var NpcEntities : array<CNewNPC>;
  public var NpcGUIDS : array<int>;
  public var DebugMode : bool;
  public var MaxPlayers, CurPlayers : int;
  public function addPlayer(obj:CNewNPC, objhorse:CNewNPC, Header:int)
  {
    PlayerOBJECTS.PushBack(obj);
    PlayerPOSITIONS.PushBack(obj.GetWorldPosition());
    PlayerHORSES.PushBack(objhorse);
    PlayerHORSESPOSITION.PushBack(objhorse.GetWorldPosition());
    PlayerIDS.PushBack(Header);
  }
  public function setPlayerPos(idcl:int, pos:Vector)
  {
    PlayerPOSITIONS[getPlayerIndex(idcl)] = pos;
  }
  public function getPlayerPOS(idcl:int) : Vector
  {
    return PlayerPOSITIONS[getPlayerIndex(idcl)];
  }
  public function removePlayer(idcl:int)
  {
    PlayerOBJECTS[getPlayerIndex(idcl)].Destroy();
    PlayerHORSES[getPlayerIndex(idcl)].Destroy();

    PlayerOBJECTS.Remove(PlayerOBJECTS[getPlayerIndex(idcl)]);
    PlayerHORSES.Remove(PlayerOBJECTS[getPlayerIndex(idcl)]);

    PlayerPOSITIONS.Remove(PlayerPOSITIONS[getPlayerIndex(idcl)]);
    PlayerHORSESPOSITION.Remove(PlayerPOSITIONS[getPlayerIndex(idcl)]);
  }
  public function getPlayerOBJ(idcl:int) : CNewNPC
  {
    return PlayerOBJECTS[getPlayerIndex(idcl)];
  }
  public function getPlayerHORSE(idcl:int) : CNewNPC
  {
    return PlayerHORSES[getPlayerIndex(idcl)];
  }
  public function getPlayerID(idofrow:int) : int
  {
    return PlayerIDS[idofrow];
  }
  public function removeNPC(guid:int)
  {
    NpcEntities.Remove(getNPC(guid));
  }
  public function getNPC(guid:int) : CNewNPC
  {
    var i: int;
    for (i = 0; i < NpcGUIDS.Size(); i += 1)
      if(NpcGUIDS[i] == guid)
        return NpcEntities[i];
    return NULL;
  }
  public function addNPC(e:CNewNPC, guidhash:int)
  {
    NpcEntities.PushBack(e);
    NpcGUIDS.PushBack(guidhash);
  }
  public function getPlayerIndex(idcl:int) : int
  {
    var i: int;
    for (i = 0; i < PlayerIDS.Size(); i += 1)
      if(PlayerIDS[i] == idcl)
        return i;
    return -1;
  }
  //ANOTHERSTORAGE
  private function isHeaderValid(Header: string): bool {
    return Header != "";
  }
  private function isNullOrEmpty(item: StorageItem): bool {
    if (!item) return false;
    return isHeaderValid(item.Header);
  }
  public function getItem(Header: string): StorageItem {
    var i: int;
    if (!isHeaderValid(Header)) return NULL;
    for (i = 0; i < items.Size(); i += 1)
      if (items[i].Header == Header)
        return items[i];
    return NULL;
  }

  public function getItemIndex(Header: string): int {
    var i: int;
    if (!isHeaderValid(Header)) return -1;
    for (i = 0; i < items.Size(); i += 1)
      if (items[i].Header == Header)
        return i;
    return -1;
  }

  public function hasItem(Header: string): bool {
    return getItemIndex(Header) >= 0;
  }

  public function setItem(item: StorageItem): bool {
    var i: int;
    if (!isNullOrEmpty(item))return false;
    if (!setItemAt(getItemIndex(item.Header), item)) items.PushBack(item);  
    return true;
  }
  public function removeItem(item: StorageItem) {
    items.Remove(item);
  }

  private function setItemAt(index: int, item: StorageItem): bool {
    if (index < 0 || index >= items.Size())return false;
    if (!isNullOrEmpty(item))return false;
    items[index] = item;
    return true;
  }
}
