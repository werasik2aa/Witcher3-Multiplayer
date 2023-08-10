exec function LaunchCustomFramework() {
    var W3MPmng: WITCHER_3MP;
    var storage : Storage;
    storage = getStorage();
    storage.PlayerOBJECTS.Clear();
    storage.PlayerPOSITIONS.Clear();
    storage.PlayerHORSES.Clear();
    storage.PlayerHORSESPOSITION.Clear();
    storage.PlayerIDS.Clear();
    if(!storage.W3MP_MANAGER)
    {
        storage.W3MP_MANAGER = W3MPmng = new WITCHER_3MP in thePlayer;
        W3MPmng.ExecuteIT();
    }
}
statemachine class WITCHER_3MP {
    //here can be some vars but for functions inside
    public function ExecuteIT()
    {
        if(GetCurrentStateName() != 'RunningState')
            this.GotoState('RunningState');
    }
}
state RunningState in WITCHER_3MP {
    event OnEnterState( prevStateName : name )
    {
        OnStart();
    }
    entry function OnStart() {
        //VAR?
        OnUpdate(); // updateclass
    }
    latent function OnUpdate() {
        var data : Storage;
        var i : int;
        var clid : int;
        var DistanceP : float;
        var positionP1, positionP2: Vector;
        var horseEntity, entityP : CNewNPC;
        data = getStorage();
        while( true ) {
            for (i = 0; i < data.PlayerIDS.Size(); i += 1) {
                clid = data.PlayerIDS[i];
                entityP = data.getPlayerOBJ(clid);
                horseEntity = data.getPlayerHORSE(clid);
                positionP1 = data.getPlayerPOS(clid);
                positionP2 = ((CActor)entityP).GetWorldPosition();
                //if(!entityP.IsUsingHorse())
                //{
                    DistanceP = VecDistance(positionP2, positionP1);
                    _SetHeading(entityP, VecHeading(positionP1 - positionP2));
                    if(DistanceP < 1)
                        _SetTypeMove(entityP, 0);
                    else if(DistanceP <= 10)
                        _SetTypeMove(entityP, 4);
                    else
                        _Teleport(entityP, positionP1);
                    //((CActor)entityP).ActionMoveToAsync(positionP1, MT_Run, 10, 10);
                /*} else {
                    _Teleport(entityP, horseEntity.GetWorldPosition() + Vector(0,2,0));
                    _SetHeading(horseEntity, VecHeading(positionP1 - positionP2));
                    if(DistanceP < 1)
                       _SetTypeMove(horseEntity, 0);
                    else if(DistanceP <= 10)
                        _SetTypeMove(horseEntity, 4);
                    else
                        _Teleport(horseEntity, positionP1);
                    //_SetTypeMoveHorse(horseEntity, 2);
                    //((CActor)horseEntity).ActionMoveToAsync(positionP1, MT_Run, 10, 10);
                }*/
            }
            SleepOneFrame();
        }
        OnUpdate();
    }
}