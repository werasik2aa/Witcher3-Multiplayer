function getStorage(): MP_STORAGE {
    var player_input: CPlayerInput;
    player_input = thePlayer.GetInputHandler();
	if (!player_input.W3MP_STORAGE)
        player_input.W3MP_STORAGE = new MP_STORAGE in player_input;
    if (!player_input.W3MP_NPCSTORAGE)
        player_input.W3MP_NPCSTORAGE = new MP_NPCSTORAGE in player_input;
    if (!player_input.W3MP_PLAYERSTORAGE)
        player_input.W3MP_PLAYERSTORAGE = new MP_NPCSTORAGE in player_input;
	return player_input.W3MP_STORAGE;
}
function getBuffer(): MP_STORAGE {
    var player_input: CPlayerInput;
    player_input = thePlayer.GetInputHandler();
    if (!player_input.buffer)
        player_input.buffer = new MP_STORAGE in player_input;
	return player_input.buffer;
}
class StorageItem {
  public var Header: string;
}
class NPC {
    var Entity : CActor ;
    var H_Entity : CActor ;
    var ID : int;
    var Angle : float;
    var H_Angle : float;
    var Position : Vector;
    var H_Position : Vector;
    var Name : string;
    var Template : string;
    var FightID : int;
}
class MP_NPCSTORAGE {
    public var Entity : array<CActor >;
    public var H_Entity : array<CActor >;
    public var ID : array<int>;
    public var Position : array<Vector>;
    public var FightID : array<int>;
    public var Angle : array<float>;
    public var H_Position : array<Vector>;
    public var H_Angle : array<float>;
    public var Name : array<string>;
    public var Template : array<string>;

    public function Clear()
    {
        var i: int;// CLEAR ALL PLAYER DATA BEFORE

        for (i = 0; i < Entity.Size(); i += 1)
            Entity[i].Destroy();

        for (i = 0; i < H_Entity.Size(); i += 1)
            H_Entity[i].Destroy();
            
        Entity.Clear();
        H_Entity.Clear();
        ID.Clear();
        Position.Clear();
        H_Position.Clear();
        Name.Clear();
        Angle.Clear();
        H_Angle.Clear();
        Template.Clear();
        FightID.Clear();
    }

    public function Add(EntityE:CActor , EntityH:CActor , namei:string, templ:string)
    {
        if (!IsExistByNPC(EntityE))
        {
            ID.PushBack(Entity.Size());
            Entity.PushBack(EntityE);
            H_Entity.PushBack(EntityH);
            Angle.PushBack(EntityE.GetHeading());
            H_Angle.PushBack(EntityH.GetHeading());
            FightID.PushBack(-1);
            Position.PushBack(EntityE.GetWorldPosition());
            H_Position.PushBack(EntityH.GetWorldPosition());
            Name.PushBack(namei);
            Template.PushBack(templ);
        }
    }
    public function GetByIndex(index:int) : NPC
    {
        var data : NPC;

        if(index > -1) 
        {
            data = new NPC in theGame;
            data.Entity = Entity[index];
            data.H_Entity = H_Entity[index];
            data.ID = ID[index];
            data.Position = Position[index];
            data.H_Position = H_Position[index];
            data.Name = Name[index];
            data.Template = Template[index];
            data.FightID = FightID[index];
            data.Angle = Angle[index];
            data.H_Angle = H_Angle[index];

            return data;
        }

        return NULL;
    }
    public function RemoveByID(Req:int)
    {
        var index:int;
        index = GetIndexByID(Req);
        if (index > -1)
        {
            Entity[index].Destroy();
            H_Entity[index].Destroy();
            Entity.Remove(Entity[index]);
            H_Entity.Remove(H_Entity[index]);
            ID.Remove(ID[index]);
            Angle.Remove(Angle[index]);
            H_Angle.Remove(H_Angle[index]);
            FightID.Remove(FightID[index]);
            Position.Remove(Position[index]);
            H_Position.Remove(H_Position[index]);
            Name.Remove(Name[index]);
            Template.Remove(Template[index]);
        }
    }
    public function RemoveByNPC(Req:CActor )
    {
        RemoveByID(GetIndexByNPC(Req));
    }
    public function DeleteByID(Req:int)
    {
        RemoveByID(Req);
    }
    public function DeleteByNPC(Req:CActor )
    {
        RemoveByNPC(Req);
    }
    public function GetByID(Req:int) : NPC
    {
        return GetByIndex(GetIndexByID(Req));
    }
    public function GetByNPC(Req:CActor ) : NPC
    {
        return GetByIndex(GetIndexByID(GetIDByNPC(Req)));
    }
    public function GetIndexByNPC(Req:CActor ) : int
    {
        return Entity.FindFirst(Req);
    }
    public function GetIndexByID(Req:int) : int
    {
        return ID.FindFirst(Req);
    }
    public function GetIDByNPC(Req:CActor ) : int
    {
        if(Entity.FindFirst(Req) == -1) return -1;
        return ID[Entity.FindFirst(Req)];
    }
    public function IsExistByNPC(Req:CActor ) : bool
    {
        if(Entity.FindFirst(Req) > -1) return true;
        return false;
    }
    public function IsExistByID(Req:int) : bool
    {
        return GetIndexByID(Req) > -1;
    }
    public function SetByIDPos(Req:int, Pos:Vector, IsHorse:bool)
    {
        var index:int;
        index = GetIndexByID(Req);
        if(index > -1)
            if(!IsHorse)
                Position[index] = Pos;
            else
                H_Position[index] = Pos;
    }
    public function SetByIDAngle(Req:int, angl:float, IsHorse:bool)
    {
        var index:int;
        index = GetIndexByID(Req);
        if(index > -1)
            if(!IsHorse)
                Angle[index] = angl;
            else
                H_Angle[index] = angl;
    }
    public function SetByIDFightID(Req:int, sid:int)
    {
        var index:int;
        index = GetIndexByID(Req);
        if(index > -1)
            FightID[index] = sid;
    }
}

class MP_STORAGE {
    public var items: array<StorageItem>;
    public var DebugMode : bool;
    public var Version : string;
    public var ClientState : int;
    public var EventsBuffer : array<string>; //temp 
    public var QuestsGuid : array<string>;
    public var KilledID : array<string>;

    default Version = "1.0";
    default ClientState = 1;
    default DebugMode = false;

    public function getNPCStorage(): MP_NPCSTORAGE
    {
	    return thePlayer.GetInputHandler().W3MP_NPCSTORAGE;
    }

    public function getPLAYERStorage(): MP_NPCSTORAGE {
	    return thePlayer.GetInputHandler().W3MP_PLAYERSTORAGE;
    }

    public function SetClientState(st:int)
    {
        ClientState = st;
    }

    public function SetDebugMode(md:bool)
    {
        DebugMode = md;
    }

    public function OnExecutedEvent(data:string)
    {
        EventsBuffer.PushBack(data);
    }

    public function OnAddedQuestEvent(data:string)
    {
        QuestsGuid.PushBack(data);
    }

    public function GetEventsData() : array<string>
    {
        return EventsBuffer;
    }

    public function GetQuestGuids() : array<string>
    {
        return QuestsGuid;
    }
    public function AddKilledID(id:string)
    {
        KilledID.PushBack(id);
    }
    //ANOTHERSTORAGE
  private function isHeaderValid(Header: string): bool {
    return Header != "";
  }
  private function isNullOrEmpty(item: StorageItem): bool {
    if (!item) return false;
    return isHeaderValid(item.Header);
  }
  public function getItem(Header: string): StorageItem {
    var i: int;
    if (!isHeaderValid(Header)) return NULL;
    for (i = 0; i < items.Size(); i += 1)
      if (items[i].Header == Header)
        return items[i];
    return NULL;
  }

  public function getItemIndex(Header: string): int {
    var i: int;
    if (!isHeaderValid(Header)) return -1;
    for (i = 0; i < items.Size(); i += 1)
      if (items[i].Header == Header)
        return i;
    return -1;
  }

  public function hasItem(Header: string): bool {
    return getItemIndex(Header) >= 0;
  }

  public function setItem(item: StorageItem): bool {
    var i: int;
    if (!isNullOrEmpty(item))return false;
    if (!setItemAt(getItemIndex(item.Header), item)) items.PushBack(item);  
    return true;
  }
  public function removeItem(item: StorageItem) {
    items.Remove(item);
  }

  private function setItemAt(index: int, item: StorageItem): bool {
    if (index < 0 || index >= items.Size())return false;
    if (!isNullOrEmpty(item))return false;
    items[index] = item;
    return true;
  }
}
