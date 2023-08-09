exec function LaunchCustomFramework() {
    var manager: WITCHER_3MP;
    var storage : Storage;
    storage = getStorage();
    storage.PlayerOBJECTS.Clear();
    storage.PlayerPOSITIONS.Clear();
    storage.PlayerHORSES.Clear();
    storage.PlayerHORSESPOSITION.Clear();
    storage.PlayerIDS.Clear();
    manager = new WITCHER_3MP in thePlayer;
    manager.ExecuteIT();
}
statemachine class WITCHER_3MP {
    //here can be some vars but for functions inside
    public function ExecuteIT()
    {
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
        var player_position, screen_position, positionP1, positionP2: Vector;
        var horseEntity, entityP : CNewNPC;
        data = getStorage();
        while( true ) {
            for (i = 0; i < data.PlayerIDS.Size(); i += 1) {
                clid = data.PlayerIDS[i];
                entityP = data.getPlayerOBJ(clid);
                horseEntity = data.getPlayerHORSE(clid);
                positionP1 = data.getPlayerPOS(clid);
                positionP2 = ((CActor)entityP).GetWorldPosition();
                DistanceP = VecDistance(positionP2, positionP1);
                //if(entityP.GetBehaviorVariable('isOnHorse') == 0.0) {
                _SetHeading(entityP, VecHeading(positionP1 - positionP2));
                if(DistanceP < 1)
                    _SetTypeMove(entityP, 0);
                else if(DistanceP <= 10)
                    _SetTypeMove(entityP, 4);
                else
                    _Teleport(entityP, positionP1);
                ((CActor)entityP).ActionMoveToAsync(positionP1, MT_Run, 10, 10);
                /*} else {
                _SetHeading(horseEntity, VecHeading(positionP1 - positionP2));
                _Teleport(entityP, data.PlayersHORSES[i].GetWorldPosition());
                //_SetTypeMove(entityP, 2);
                if(DistanceP < 0.1)
                    _SetTypeMove(entityP, 0);
                if(DistanceP < 0.5)
                    _SetTypeMove(entityP, 1);
                else if(DistanceP < 2)
                    _SetTypeMove(entityP, 2);
                else if(DistanceP < 4)
                _SetTypeMove(entityP, 4);
                else if(DistanceP < 8)
                    _Teleport(horseEntity, positionP1);
                _SetTypeMoveHorse(horseEntity, 2);
                ((CActor)horseEntity).ActionMoveTo(positionP1, MT_Run);
                }*/
            }
            SleepOneFrame();
        } 
    }
}