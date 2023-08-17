exec function LaunchCustomFramework() {
    var storage : Storage;
    storage = getStorage();
    storage.PlayerOBJECTS.Clear();
    storage.PlayerPOSITIONS.Clear();
    storage.PlayerHORSES.Clear();
    storage.PlayerHORSESPOSITION.Clear();
    storage.PlayerIDS.Clear();
}