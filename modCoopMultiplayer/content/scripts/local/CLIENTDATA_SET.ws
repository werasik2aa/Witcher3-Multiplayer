exec function SetClientDebugMode(safa : int)
{
    var data : MyModData;
    data = GetServerSettings();
    data.DebugMode = (safa == 1);
    UpdateServerSettings(data);
    Log("[WITCHER3MP] Debug Enabled: " + (safa == 1));
}