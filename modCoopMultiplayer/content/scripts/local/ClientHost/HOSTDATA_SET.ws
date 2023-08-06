exec function SetHostDebugMode(safa : int)
{
    var data : MyData;
    data = GetServerSettings();
    data.DebugMode = (safa == 1);
    Log("[WITCHER3MP] Debug Enabled: " + data.DebugMode);
}
exec function SetMaxPlayers(a:int)
{
    var data : MyData;
    data = GetServerSettings();
    data.MaxPlayers = a;
    if(GetServerSettings().DebugMode)
        Log("[WITCHER3MP] Succesfully changed MaxPlayers TO: " + a);
}
exec function OnLeaveT(a:int)
{
    var data : MyData;
    data = GetServerSettings();
    _RemovePlayer(data.PlayersDATAS[a]);
    if(GetServerSettings().DebugMode)
        Log("[WITCHER3MP] Succesfully RemovePlayer id: " + a);
}
exec function KillActorByGuidHash(GuidHash : int)
{
	var action		: W3DamageAction;
	var i 			: int;
	var actors		: array<CActor>;
	var actor		: CActor;
	var currhash    : int;
	actors = GetActorsInRange( thePlayer, 100);
	
	for	( i = 0; i < actors.Size(); i+= 1 )
	{
		actor = (CActor)actors[i];
		currhash = actor.GetGuidHash();
		if (currhash == GuidHash)
		{
			actor.SignalGameplayEvent( 'Death' );
			actor.SetAlive(false);
			actor.OnDeath( action );
			break;
		}
	}
}