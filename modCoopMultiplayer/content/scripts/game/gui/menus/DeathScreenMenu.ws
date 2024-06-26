/***********************************************************************/
/** 	© 2015 CD PROJEKT S.A. All rights reserved.
/** 	THE WITCHER® is a trademark of CD PROJEKT S. A.
/** 	The Witcher game is based on the prose of Andrzej Sapkowski.
/***********************************************************************/




class CR4DeathScreenMenu extends CR4MenuBase
{
	public var hasSaveData : bool;
	private var m_fxShowInputFeedback : CScriptedFlashFunction;
	event  OnConfigUI()
	{
		var tutorialPopupRef  : CR4TutorialPopup;
		super.OnConfigUI();
		PopulateData();
		m_fxShowInputFeedback = m_flashModule.GetMemberFlashFunction("showInputFeedback");
		tutorialPopupRef = (CR4TutorialPopup)theGame.GetGuiManager().GetPopup('TutorialPopup');
		if (tutorialPopupRef) tutorialPopupRef.ClosePopup();
		theSound.EnterGameState(ESGS_Death);
		theSound.SoundEvent( 'gui_global_player_death_thump' );
		theGame.Pause( "DeathScreen" );
		theGame.ResetFadeLock('DeathScreenMenu');
		theGame.FadeInAsync( 1.2f );
		m_guiManager.RequestMouseCursor(true);
	}
	private function updateHudConfigs():void
	{
	}
	event  OnClosingMenu()
	{
		m_guiManager.RequestMouseCursor(false);
		theInput.RestoreContext( 'EMPTY_CONTEXT', true );
		super.OnClosingMenu();
		theGame.Unpause( "DeathScreen" );
	}

	function OnRequestSubMenu( menuName: name, optional initData : IScriptable )
	{
		RequestSubMenu( menuName, initData );
	}
	event  OnCloseMenu()
	{
		var menu			: CR4MenuBase;
		menu = (CR4MenuBase)GetSubMenu();
		if(menu) menu.CloseMenu();
		CloseMenu();
	}
	
	function CloseMenuRequest():void
	{
		var menu : CR4MenuBase;
		menu = (CR4MenuBase)GetSubMenu();
		if(!menu) CloseMenu();
	}
	function ChildRequestCloseMenu()
	{
		var menu			: CR4MenuBase;
		var menuToOpen		: name;
		if (hasSaveData != hasSaveDataToLoad()) PopulateData();
		m_fxShowInputFeedback.InvokeSelfOneArg(FlashArgBool(true));
	}
	
	function PopulateData()
	{
		var l_FlashArray : CScriptedFlashArray;
		var l_DataFlashObject : CScriptedFlashObject;	
		
		// DEATH RESUR
		l_FlashArray = m_flashValueStorage.CreateTempFlashArray();
		l_DataFlashObject = m_flashValueStorage.CreateTempFlashObject();
		l_DataFlashObject.SetMemberFlashString	( "label", "Resurrect" );
		l_DataFlashObject.SetMemberFlashUInt	( "tag", NameToFlashUInt('DebugResurrect') );
		l_FlashArray.PushBackFlashObject( l_DataFlashObject );

		// LOAD
		l_DataFlashObject = m_flashValueStorage.CreateTempFlashObject();
		l_DataFlashObject.SetMemberFlashString	( "label", "Load Game (If connnected you must to Reconnect!)" );
		l_DataFlashObject.SetMemberFlashUInt	( "tag", NameToFlashUInt('Load') );
		l_FlashArray.PushBackFlashObject( l_DataFlashObject );
		
		/* LoadGame(Orig)
		l_DataFlashObject = m_flashValueStorage.CreateTempFlashObject();
		l_DataFlashObject.SetMemberFlashString	( "label", "LoadGame" );
		l_DataFlashObject.SetMemberFlashUInt	( "tag", NameToFlashUInt('Load') );
		l_FlashArray.PushBackFlashObject( l_DataFlashObject );
		*/

		// QUIT
		l_DataFlashObject = m_flashValueStorage.CreateTempFlashObject();	
		l_DataFlashObject.SetMemberFlashString	( "label", GetLocStringByKeyExt("panel_button_common_quittomainmenu") );
		l_DataFlashObject.SetMemberFlashUInt	( "tag", NameToFlashUInt('Quit') );
		l_FlashArray.PushBackFlashObject( l_DataFlashObject );
		
		m_flashValueStorage.SetFlashArray( "hud.deathscreen.list", l_FlashArray );//UPDATE LIST
	}
	
	function PlayOpenSoundEvent()
	{
	//HM
	}
	
	public function SetMenuAlpha( value : int ) : void
	{
		m_flashModule.SetAlpha(value);
	}
	
	event  OnPress( tag : name )
	{
		switch( tag )
		{
			case 'Load' :
				OnLoad();
				break;
			case 'Respawn' :
				OnRespawn();
				break;
			case 'Quit' :
				OnQuit();
				break;
			case 'DebugResurrect' :
				OnRessurect();
				break;
		}
	}
	public function HideInputFeedback() : void
	{
		m_fxShowInputFeedback.InvokeSelfOneArg(FlashArgBool(false));
	}
	event OnLoad()
	{
		var initData : W3MenuInitData;
		initData = new W3MenuInitData in this;
		initData.setDefaultState('LoadGame');
		RequestSubMenu( 'IngameMenu', initData );
	}
	event OnQuit()
	{
		theGame.GetGuiManager().TryQuitGame();
	}
	event OnRespawn()
	{
		theGame.SetIsRespawningInLastCheckpoint();
		theGame.LoadLastGameInit( true );
	}
	event OnRessurect()
	{
		theGame.SetIsRespawningInLastCheckpoint();
		thePlayer.CheatResurrect();
	}
}