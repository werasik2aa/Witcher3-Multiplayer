statemachine class Manager extends StorageItem {
  default Header = "Manager";
  private var oneliner_counter: int;
  protected var oneliners: array<Oneliner>;
  protected var module_oneliners: CR4HudModuleOneliners;
  protected var module_flash: CScriptedFlashSprite;
  protected var module_hud: CR4ScriptedHud;
  private var fxCreateOnelinerSFF: CScriptedFlashFunction;
  private var fxRemoveOnelinerSFF: CScriptedFlashFunction;
  private function initialize() {
    module_hud = (CR4ScriptedHud)theGame.GetHud();
    module_oneliners = (CR4HudModuleOneliners)(module_hud.GetHudModule( "OnelinersModule" ));
    module_flash = module_oneliners.GetModuleFlash();
		fxCreateOnelinerSFF 	= module_flash.GetMemberFlashFunction( "CreateOneliner" );
		fxRemoveOnelinerSFF 	= module_flash.GetMemberFlashFunction( "RemoveOneliner" );
  }
  private function getNewId(): int {
    var id: int;
    id = Max(oneliner_counter, (int)theGame.GetLocalTimeAsMilliseconds());
	  oneliner_counter = id + 1;
    return id;
  }
  public function createOneliner(oneliner: Oneliner) {
    var should_initialize_and_render: bool;
    should_initialize_and_render = GetCurrentStateName() != 'Render';
    if (should_initialize_and_render) initialize();
    oneliner.id = getNewId();
    updateOneliner(oneliner);
    oneliners.PushBack(oneliner);
    if (should_initialize_and_render) GotoState('Render');
  }
  public function updateOneliner(oneliner: Oneliner) {
    fxRemoveOnelinerSFF.InvokeSelfOneArg(FlashArgInt(oneliner.id));
    fxCreateOnelinerSFF.InvokeSelfTwoArgs(
      FlashArgInt(oneliner.id),
      FlashArgString(oneliner.text)
    );
  }
  public function deleteOneliner(oneliner: Oneliner) {
    oneliners.Remove(oneliner);
    fxRemoveOnelinerSFF.InvokeSelfOneArg(FlashArgInt(oneliner.id));
  }
  public function findByHeader(Header: string): array<Oneliner> {
    var output: array<Oneliner>;
    var i: int;
    for (i = 0; i < oneliners.Size(); i += 1) 
      if (oneliners[i].Header == Header)
        output.PushBack(oneliners[i]);
    return output;
  }
  public function findByHeaderPrefix(Header: string): array<Oneliner> {
    var output: array<Oneliner>;
    var i: int;
    for (i = 0; i < oneliners.Size(); i += 1)
      if (StrStartsWith(oneliners[i].Header, Header))
        output.PushBack(oneliners[i]);
    return output;
  }
  public function deleteByHeader(Header: string): array<Oneliner> {
    var output: array<Oneliner>;
    var i: int;
    output = findByHeader(Header);
    for (i = 0; i < output.Size(); i += 1) deleteOneliner(output[i]);
    return output;
  }
  public function deleteByHeaderPrefix(Header: string): array<Oneliner> {
    var output: array<Oneliner>;
    var i: int;
    output = findByHeaderPrefix(Header);
    for (i = 0; i < output.Size(); i += 1)deleteOneliner(output[i]);
    return output;
  }
}

