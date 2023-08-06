//SET TARGET COMBAT
exec function SetCombatTargetMP(id:int, id2:int){
    var data : MyData;
    data = GetServerSettings();
    _SetCombatTarget(data.PlayersDATAS[id], data.NpcEntities[id2]);
    if(data.DebugMode)
        Log("[WITCHER3MP] Setted npc " + id + " |TargetToHit: " + id2);
}