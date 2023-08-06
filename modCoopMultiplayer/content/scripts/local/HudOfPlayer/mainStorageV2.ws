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
///CLASSES
class StorageItem {
  public var Header: string;
}
class Storage {
  private var items: array<StorageItem>;
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
