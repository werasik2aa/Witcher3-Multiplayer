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
		var oneliners_count: int;
		var i: int;
		var player_position: Vector;
		var screen_position: Vector;
		var data : MyData;
		var positionP1 : Vector;
		var positionP2 : Vector;
		var horseEntity : CNewNPC;
		var entityP : CNewNPC;
		screen_position = thePlayer.GetWorldPosition();
		player_position = thePlayer.GetWorldPosition();
		while (true) {
			data = GetServerSettings();
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

			for (i = 0; i < data.PlayersDATAS.Size(); i += 1) {
			entityP = data.PlayersDATAS[i];
			horseEntity = data.PlayersHORSES[i];
			positionP1 = data.PlayersPOSITIONS[i];
			positionP2 = ((CActor)entityP).GetWorldPosition();
			if(entityP.GetBehaviorVariable('isOnHorse') == 0.0) {
				_SetHeading(entityP, VecHeading(positionP1 - positionP2));
				if(VecDistance(positionP2, positionP1) > 0.1 && VecDistance(positionP2, positionP1) < 0.5)
					_SetTypeMove(entityP, 2);
				else if(VecDistance(positionP2, positionP1) >= 1 && VecDistance(positionP2, positionP1) < 2)
					_SetTypeMove(entityP, 2);
				else if(VecDistance(positionP2, positionP1) >= 2 && VecDistance(positionP2, positionP1) < 3)
					_SetTypeMove(entityP, 4);
				else if(VecDistance(positionP2, positionP1) >= 100)
					_Teleport(entityP, positionP1);
				else if(VecDistance(positionP2, positionP1) <= 0.1)
					_SetTypeMove(entityP, 0);
				((CActor)entityP).ActionMoveTo(positionP1, MT_Run);
			} else {
				_SetHeading(horseEntity, VecHeading(positionP1 - positionP2));
				_Teleport(entityP, data.PlayersHORSES[i].GetWorldPosition());
				_SetTypeMove(horseEntity, 2);
				((CActor)horseEntity).ActionMoveTo(positionP1, MT_Run);
			}
		}
			SleepOneFrame();
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
