class Storage {
  private var items: array<ModSettingsTemplate>;
  private function isTagValid(data: string): bool {
    return data != "";
  }
  private function isItemValid(item: ModSettingsTemplate): bool {
    if (!item) return false;
    return isTagValid(item.Header);
  }
  public function getItem(data: String): ModSettingsTemplate {
    var i : int;
    if (!isTagValid(data)) return NULL;
    for (i = 0; i < items.Size(); i += 1)
      if (items[i].Header == data)
        return items[i];
    return NULL;
  }
  public function getItemIndex(data: ModSettingsTemplate): int {
    var i : int;
    if (!isTagValid(data.Header)) return -1;
    for (i = 0; i < items.Size(); i += 1)
      if (items[i].Header == data.Header)
        return i;
    return -1;
  }
  public function getItemIndexWithStr(data: String): int {
    var i : int;
    if (!isTagValid(data)) return -1;
    for (i = 0; i < items.Size(); i += 1)
      if (items[i].Header == data)
        return i;
    return -1;
  }

  public function Replace(item : ModSettingsTemplate)
  {
    setItemAt(getItemIndex(item), item);
  }
  public function hasItem(data: String): bool {
    return getItemIndexWithStr(data) >= 0;
  }
  public function setItem(item: ModSettingsTemplate): bool {
    var i : int;

    if (!isItemValid(item)) {
      return false;
    }
    if (!setItemAt(getItemIndexWithStr(item.Header), item)) {
      items.PushBack(item);  
    }
    return true;
  }
  public function removeItem(item: ModSettingsTemplate) {
    items.Remove(item);
  }

  public function removeALL() {
    items.Clear();
  }

  public function setItemAt(index: int, item: ModSettingsTemplate): bool {
    if (index < 0 || index >= items.Size()) {
      return false;
    }
    if (!isItemValid(item)) {
      return false;
    }
    items[index] = item;
    return true;
  }
}
function getStorage(): Storage {
	return getStorageFromInput(thePlayer.GetInputHandler());
}
function getStorageFromInput(player_input: CPlayerInput): Storage {
	if (!player_input.storage)
		player_input.storage = new Storage in player_input;
	return player_input.storage;
}

function getBuffer(): Storage {
	return getBufferFromInput(thePlayer.GetInputHandler());
}

function getBufferFromInput(player_input: CPlayerInput): Storage {
	if (!player_input.buffer)
		player_input.buffer = new Storage in player_input;
	return player_input.buffer;
}

function _sustuninstall(data: string) {
	var storage: Storage;
	storage = getStorage();
	if (storage.hasItem(data))
		storage.removeItem(storage.getItem(data));
}
exec function sustuninstall(data: string) {
	_sustuninstall(data);
}