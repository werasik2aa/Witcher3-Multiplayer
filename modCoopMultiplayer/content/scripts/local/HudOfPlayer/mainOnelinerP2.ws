state Idle in Manager {
	event OnEnterState(previous_state_name: name) {
		super.OnEnterState(previous_state_name);
	}
}
state Render in Manager {
	event OnEnterState(previous_state_name: name) {
		super.OnEnterState(previous_state_name);
		this.Render_main();
	}
	entry function Render_main() {
		if (!thePlayer.IsCiri()) this.renderingLoop();
		parent.GotoState('Idle');
	}
	latent function renderingLoop() {
		var sprite: CScriptedFlashSprite;
		var oneliner: Oneliner;
		var oneliners_count, i: int;
		var player_position, screen_position : Vector;
		screen_position = thePlayer.GetWorldPosition();
		player_position = thePlayer.GetWorldPosition();
		while (true) {
			oneliners_count = parent.oneliners.Size();
			if (oneliners_count <= 0) break;
			player_position = thePlayer.GetWorldPosition();
			for (i = 0; i < oneliners_count; i += 1) {
				oneliner = parent.oneliners[i];
				sprite = parent.module_flash.GetChildFlashSprite("mcOneliner" + oneliner.id);
				if (!oneliner.getVisible(player_position)) {
					sprite.SetVisible(false);
					continue;	
				}
				if (oneliner.getScreenPosition(parent.module_hud, screen_position)) {
					sprite.SetPosition(screen_position.X, screen_position.Y);
					sprite.SetVisible(true);
				} else {
					sprite.SetVisible(false);
				}
			}
			Update2();
			SleepOneFrame();
		}
	}
	latent function Update2() 
	{
    	var data : Storage;
    	var i : int;
    	var clid : int;
    	var DistanceP : float;
    	var positionP1, positionP2: Vector;
    	var horseEntity, entityP : CNewNPC;
    	data = getStorage();
    	for (i = 0; i < data.PlayerIDS.Size(); i += 1) {
        	clid = data.PlayerIDS[i];
        	entityP = data.getPlayerOBJ(clid);
        	horseEntity = data.getPlayerHORSE(clid);
        	positionP1 = data.getPlayerPOS(clid);
        	positionP2 = ((CActor)entityP).GetWorldPosition();
        	if(!entityP.IsUsingHorse())
        	{
        	    DistanceP = VecDistance(positionP2, positionP1);
        	    _SetHeading(entityP, VecHeading(positionP1 - positionP2));
        	    if(DistanceP < 1)
            	    _SetTypeMove(entityP, 0);
            	else if(DistanceP <= 10)
            	    _SetTypeMove(entityP, 4);
            	else
                	_Teleport(entityP, positionP1);
        	} else {
            	_Teleport(entityP, horseEntity.GetWorldPosition() + Vector(0,2,0));
            	_SetHeading(horseEntity, VecHeading(positionP1 - positionP2));
            	if(DistanceP < 1)
            	    _SetTypeMove(horseEntity, 0);
            	else if(DistanceP <= 10)
            	    _SetTypeMove(horseEntity, 4);
            	else
            		_Teleport(horseEntity, positionP1);
                //_SetTypeMoveHorse(horseEntity, 2);
            }
        }
    }
}
function worldToScreenPosition(hud: CR4ScriptedHud, world_position: Vector, out screen_position: Vector): bool {
	if (!theCamera.WorldVectorToViewRatio(world_position, screen_position.X, screen_position.Y)) return false;
	screen_position.X = (screen_position.X + 1) / 2;
	screen_position.Y = (screen_position.Y + 1) / 2;
	screen_position = hud.GetScaleformPoint(screen_position.X, screen_position.Y);
	return true;
}
