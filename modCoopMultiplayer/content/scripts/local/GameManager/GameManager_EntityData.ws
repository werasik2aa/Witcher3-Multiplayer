//EXECS
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
//functions
function _IsOnHorse(entity:CNewNPC) : bool
{
  return ((CActor)entity).HasTag('MYisOnHorsetrue');
}
function _MountWearing(entity:CNewNPC, citem:name)
{
  var ids : array<SItemUniqueId>;
	var inv : CInventoryComponent;
  var data : Storage;
  data = getStorage();
  inv = entity.GetInventory();
  ids = inv.AddAnItem(citem, 1); entity.EquipItem(ids[0]);
}