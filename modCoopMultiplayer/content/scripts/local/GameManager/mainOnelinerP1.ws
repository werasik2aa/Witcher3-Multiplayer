
class Oneliner {
  var Header: string;
  var id: int;
  var text: string;
  var visible: bool;
  var position: Vector;
  var offset: Vector;
  var render_distance: int;
  default visible = true;
  function register() {
    var manager: Manager;
    manager = getManager();
    manager.createOneliner(this);
  }
  function unregister() {
    var manager: Manager;
    manager = getManager();
    manager.deleteOneliner(this);
  }
  function update() {
    var manager: Manager;
    manager = getManager();
    manager.updateOneliner(this);
  }
  public function setRenderDistance(value: int): Oneliner {
    render_distance = value;
    return this;
  }
  public function setText(value: string): Oneliner {
    text = value;
    return this;
  }
  public function setOffset(value: Vector): Oneliner {
    offset = value;
    return this;
  }
  public function setHeader(value: string): Oneliner {
    Header = value;
    return this;
  }
  function getVisible(player_position: Vector): bool {
    if (render_distance <= 0)
      return visible;
    return VecDistanceSquared2D(player_position, getPosition()) <= render_distance * render_distance;
  }
  function getPosition(): Vector {
    return position + offset;
  }
  function getScreenPosition(hud: CR4ScriptedHud, out screen_position: Vector): bool {
    var world_position: Vector;
    var result: bool;
    world_position = getPosition();
    result = worldToScreenPosition(hud, world_position, screen_position);
    return result;
  }
}
function getManager(): Manager {
	var manager: Manager;
	var storage: MP_STORAGE;
	storage = getBuffer();
	manager = (Manager)storage.getItem("Manager");
	if (!manager) {
		manager = new Manager in storage;
		storage.setItem(manager);
	}
	return manager;
}
//FONT
class HeaderBuilder {
  private var buffer: string;
  private var self_closing: bool;
  default self_closing = true;
  private var _Header: string;
  public function Header(value: string): HeaderBuilder {
    _Header = value;
    buffer = "<" + value;
    return this;
  }
  public function attr(key: string, value: string): HeaderBuilder {
    buffer += " ";
    buffer += key;
    buffer += "=\"";
    buffer += value;
    buffer +="\"";
    return this;
  }
  public function text(value: string): string {
    buffer += ">" + value;
    return buffer + "</" + _Header + ">";
  }
  public function close(): string {
    return buffer + " />";
  }
}
class OnelinerEntity extends Oneliner {
  var entity: CEntity;
  function getPosition(): Vector {
    return entity.GetWorldPosition() + offset;
  }
}
class OnelinerScreen extends Oneliner {
  function getScreenPosition(hud: CR4ScriptedHud, out screen_position: Vector): bool {
    var position: Vector;
    position = getPosition();
    screen_position = hud.GetScaleformPoint(position.X, position.Y);
    return true;
  }
}
function oneliner(text: string, position: Vector): Oneliner {
  var oneliner: Oneliner;
  oneliner = new Oneliner in thePlayer;
  oneliner.text = text;
  oneliner.position = position;
  oneliner.register();
  return oneliner;
}

function onelinerScreen(text: string, position: Vector): OnelinerScreen {
  var oneliner: OnelinerScreen;
  oneliner = new OnelinerScreen in thePlayer;
  oneliner.text = text;
  oneliner.position = position;
  oneliner.register();
  return oneliner;
}
function onelinerEntity(text: string, entity: CEntity): OnelinerEntity {
  var oneliner: OnelinerEntity;
  oneliner = new OnelinerEntity in thePlayer;
  oneliner.text = text;
  oneliner.entity = entity;
  oneliner.register();
  return oneliner;
}