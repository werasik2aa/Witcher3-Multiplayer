exec function Spawn_Player(npc_templ:string, namea:string, xp:int, yp:int, zp:int, xp2:int, yp2:int, zp2:int)
{
    var PST: MP_NPCSTORAGE;
    var EntityP, EntityH : CActor ;
    var entityPAC : CMovingPhysicalAgentComponent;
    PST = getStorage().getPLAYERStorage();
    EntityP = _SpawnNPC(npc_templ, namea, Vector(xp, yp, zp), true, true);
    EntityH = _SpawnNPC("horse", "Horse for " + namea, Vector(xp2, yp2, zp2), true, true);
    ((CActor)EntityP).SetTemporaryAttitudeGroup('npc_charmed', AGP_Axii);
    
    entityPAC = ((CMovingPhysicalAgentComponent)EntityP.GetMovingAgentComponent());
    entityPAC.SetGravity(false);
    EntityP.EnableCollisions(false);
    
    PST.Add(EntityP, EntityH, namea, npc_templ);
    onelinerEntity(namea, EntityP);//nickname
    onelinerEntity("[Horse] " + namea, EntityH);//nickname
    _SetupPlayerObjectDefault(EntityP);
    if(getStorage().DebugMode)
        Log("[WITCHER3MP] Spawned Entity With template " + npc_templ + " \nPosition X:" + xp + " Y:" + yp + " Z:" + zp);
}
exec function Spawn_NPC(npc_templ:string, namea:string, xp:int, yp:int, zp:int, xp2:int, yp2:int, zp2:int)
{
    var PST: MP_NPCSTORAGE;
    PST = getStorage().getNPCStorage();
    //_SpawnNPC("horse", "Horse: " + namea, Vector(xp2, yp2, zp2), true, false);
    PST.Add(_SpawnNPC(npc_templ, namea, Vector(xp, yp, zp), true, true), NULL, namea, npc_templ);
    if(getStorage().DebugMode)
        Log("[WITCHER3MP] Spawned Entity With template " + npc_templ + " \nPosition X:" + xp + " Y:" + yp + " Z:" + zp);
}
exec function Spawn_Corpse(npc_templ:string, xp:int, yp:int, zp:int)
{
    _SpawnCorpse(npc_templ, Vector(xp, yp, zp));
    if(getStorage().DebugMode)
        Log("[WITCHER3MP] Spawned Entity With template " + npc_templ + " \nPosition X:" + xp + " Y:" + yp + " Z:" + zp);
}