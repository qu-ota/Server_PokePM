//HMM POKE AND DIRECT MESSAGE
//By Dominoes

function serverCmdPoke(%cl, %id)
{
	if(!$Pref::PokePM::EnablePoke == 1)
		return messageClient(%cl,'',"\c7Sorry, but the server host has disabled Poking other users.");
	if(!%target.allowPoke == 1)
		return messageClient(%cl,'',"\c7Sorry, \c6" @ %target.name SPC "\c7has disabled poking.");
		//more soon
