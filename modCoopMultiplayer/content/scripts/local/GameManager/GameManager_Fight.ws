//SET TARGET COMBAT
exec function SetCombatTargetMP(idcl:int, id2:int){
    var data : Storage;
    data = getStorage();
    _SetCombatTarget(data.getPlayerOBJ(idcl), data.NpcEntities[id2]);
    if(data.DebugMode)
        Log("[WITCHER3MP] Setted npc " + idcl + " |TargetToHit: " + id2);
}