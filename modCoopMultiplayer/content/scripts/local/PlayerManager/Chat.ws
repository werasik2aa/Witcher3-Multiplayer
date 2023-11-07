/***********************************************************************/
/** 	© 2015 CD PROJEKT S.A. All rights reserved.
/** 	THE WITCHER® is a trademark of CD PROJEKT S. A.
/** 	The Witcher game is based on the prose of Andrzej Sapkowski.
/***********************************************************************/
exec function testchat()
{
    var module : W3MP_CHAT;
	if (!(module = (W3MP_CHAT)theGame.GetHud().GetHudModule("ChatModule"))) theGame.GetHud().CreateHudModule("ChatModule");
    module = (W3MP_CHAT)theGame.GetHud().GetHudModule("ChatModule");
    module.ChatMsg("FG1");
    module.ChatTest();
    module.ChatMsg("FG2");
}
class W3MP_CHAT extends CR4HudModuleBase
{
	private var m_fxHudChatMsg		: CScriptedFlashFunction;
	private var m_fxTestHudChat		: CScriptedFlashFunction;
	private var m_fxCleanupHudChat	: CScriptedFlashFunction;
	private var _iDuringDisplay : int;		default _iDuringDisplay = 0;
	private const var MAX_CHAT_MESSEGES_DISPLAYED : int;		default MAX_CHAT_MESSEGES_DISPLAYED = 3;
	private var NEW_ITEM_DELAY : float;		default NEW_ITEM_DELAY = 0.1;
	private var displayTime : float;		default displayTime = 0.0;
	private var pendingMessages : array<string>;
	
	event  OnConfigUI()
	{
		var flashModule : CScriptedFlashSprite;
		var hud : CR4ScriptedHud;
		m_anchorName = "mcAnchorChat";
		super.OnConfigUI();
		flashModule = GetModuleFlash();
		m_fxHudChatMsg		= flashModule.GetMemberFlashFunction( "showMessage" );
		m_fxTestHudChat		= flashModule.GetMemberFlashFunction( "debugMessage" );
		m_fxCleanupHudChat	= flashModule.GetMemberFlashFunction( "cleanup" );
		displayTime = NEW_ITEM_DELAY;
		hud = (CR4ScriptedHud)theGame.GetHud();
		if (hud) hud.UpdateHudConfig('W3MP_CHAT', true);
	}
	
	event OnTick( timeDelta : float )
	{
		if( _iDuringDisplay < MAX_CHAT_MESSEGES_DISPLAYED )
		{
			displayTime += timeDelta;
			if( CheckPendingMessages() && displayTime > NEW_ITEM_DELAY )
			{
				DisplayChatMsg(pendingMessages[0]);
				_iDuringDisplay += 1;
				displayTime = 0;
				LogChannel('HUD_Chat',"_iDuringDisplay "+_iDuringDisplay + " " + pendingMessages[0]);
				pendingMessages.Erase(0);
			}
		}
	}
	
	function CheckPendingMessages() : bool
	{
		if( pendingMessages.Size() > 0 ) return true;
		return false;
	}
	
	event  OnMessageHidden( value : string)
	{
		_iDuringDisplay = Max(0,_iDuringDisplay-1);
		LogChannel('HUD_Chat'," OnMessageHidden _iDuringDisplay "+(_iDuringDisplay-1) +" displayTime "+displayTime+" value "+value);
		displayTime = 0;
		if( _iDuringDisplay == 0 ) ChatCleanup();
	}
	
	public function ChatMsg( msgText : string )
	{
		pendingMessages.PushBack(msgText);
	}	
	
	public function DisplayChatMsg( msgText : string )
	{
		m_fxHudChatMsg.InvokeSelfOneArg( FlashArgString( msgText ) );
		ShowElement(true,true);
	}
	
	public function ChatTest()
	{
		m_fxTestHudChat.InvokeSelf();
	}
	
	public function ChatCleanup()
	{
		m_fxCleanupHudChat.InvokeSelf();
		ShowElement(false,false);
	}
	
	protected function UpdatePosition(anchorX:float, anchorY:float) : void
	{
		var l_flashModule 		: CScriptedFlashSprite;
		var tempX				: float;
		var tempY				: float;
		l_flashModule 	= GetModuleFlash();
		tempX = anchorX + (300.0 * (1.0 - theGame.GetUIHorizontalFrameScale()));
		tempY = anchorY;
		l_flashModule.SetX( tempX );
		l_flashModule.SetY( tempY );	
	}
}