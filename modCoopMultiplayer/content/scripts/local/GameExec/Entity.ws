exec function SetPositionByID(ID:int, x:int, y:int, z:int, isPlayer:bool, IsHorse:bool)
{
    var data : MP_NPCSTORAGE;
    if(!isPlayer)
        data = getStorage().getNPCStorage();
    else
        data = getStorage().getPLAYERStorage();

    data.SetByIDPos(ID, Vector(x, y, z), IsHorse);

    if(getStorage().DebugMode)
        Log("[WITCHER3MP] EntityID: " + ID + " POS1: " + VecToString(Vector(x, y, z)));
}
exec function SetNameByID(ID:int, namea:string, isPlayer:bool)
{
    var data : MP_NPCSTORAGE;
    if(!isPlayer)
        data = getStorage().getNPCStorage();
    else
        data = getStorage().getPLAYERStorage();
}
exec function SetAngleByID(ID:int, Angle:int, isPlayer:bool, IsHorse:bool)
{
    var data : MP_NPCSTORAGE;
    if(!isPlayer)
        data = getStorage().getNPCStorage();
    else
        data = getStorage().getPLAYERStorage();

    data.SetByIDAngle(ID, Angle, IsHorse);

    if(getStorage().DebugMode)
        Log("[WITCHER3MP] EntityID: " + ID + " Angle: " + Angle);
}
exec function TeleportEntityByID(ID:int, x:int, y:int, z:int, isPlayer:bool, IsHorse:bool)
{
    var data : MP_NPCSTORAGE;
    if(!isPlayer)
        data = getStorage().getNPCStorage();
    else
        data = getStorage().getPLAYERStorage();

    if(!IsHorse)
        _Teleport(data.GetByID(ID).Entity, Vector(x, y, z));
    else
        _Teleport(data.GetByID(ID).H_Entity, Vector(x, y, z));
}
exec function SetHealthByID(ID:int, health:int, isPlayer:bool)
{
    var data : MP_NPCSTORAGE;
    if(!isPlayer)
        data = getStorage().getNPCStorage();
    else
        data = getStorage().getPLAYERStorage();

    _SetCurrentHp(data.GetByID(ID).Entity, health);

    if(getStorage().DebugMode)
        Log("[WITCHER3MP] EntityID: " + ID + " HealthPerc: " + health);
}
exec function SetLevelByID(ID:int, lvl:int, isPlayer:bool)
{
    var data : MP_NPCSTORAGE;
    if(!isPlayer)
        data = getStorage().getNPCStorage();
    else
        data = getStorage().getPLAYERStorage();

	_SetLevel(data.GetByID(ID).Entity, lvl);

    if(getStorage().DebugMode)
        Log("[WITCHER3MP] EntityID: " + ID + " Level: " + lvl);
}