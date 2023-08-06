exec function SetClientDebugMode(safa : int)
{
    var data : MyData;
    data = GetServerSettings();
    data.DebugMode = (safa == 1);
    UpdateServerSettings(data);
    Log("[WITCHER3MP] Debug Enabled: " + (data.DebugMode? 1 : 0));
}