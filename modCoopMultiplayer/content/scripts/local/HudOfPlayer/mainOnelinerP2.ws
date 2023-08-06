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
