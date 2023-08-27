exec function SetEnemyHealth(idcl:int,health:int)
{
    var data : Storage;
    data = getStorage();
    ((CActor)data.getPlayerOBJ(idcl).GetTarget()).SetHealthPerc(health);
}
exec function SetCurHealth(idcl:int,health:int)
{
    var data : Storage;
    data = getStorage();
    ((CActor)data.getPlayerOBJ(idcl)).SetHealthPerc(health);
}
exec function SetCurLevel(idcl:int,LVL:int)
{
    var data : Storage;
    data = getStorage();
    ((CNewNPC)data.getPlayerOBJ(idcl)).SetLevel(LVL);
}
exec function SetEnemyLevel(idcl:int,LVL:int)
{
    var data : Storage;
    data = getStorage();
    ((CNewNPC)data.getPlayerOBJ(idcl).GetTarget()).SetLevel(LVL);
}
