exec function LaunchCustomFramework(templa:string) {
    var storage : Storage;
    var actors : array<CActor>;
    var actor : CActor;
    storage = getStorage();
    storage.clearPlayers();
    _SwitchCharacter(templa);
    if(storage.DebugMode)
        Log("[WITCHER3MP] Player Data Cleared!");
}