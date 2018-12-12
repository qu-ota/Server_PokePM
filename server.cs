//HMM POKE AND DIRECT MESSAGE
//By Dominoes

if(isFile("Add-Ons/System_ReturnToBlockland/server.cs") || isFile("Add-Ons/System_BlocklandGlass/server.cs"))
{
	if(!$RTB::RTBR_ServerControl_Hook)
	{
		RTB_registerPref("Enable Poke?","Poke and PM","Pref::PokePM::EnablePoke","bool","Server_PokePM",1,0,0);
	}
}
else
{
	$Pref::PokePM::EnablePoke = 1; //Enable poking others?
}

function serverCmdPoke(%cl, %id)
{
	if($Pref::PokePM::EnablePoke == 0)
		return messageClient(%cl,'',"\c7Sorry, but the server host has disabled Poking other users.");
	if(%target.allowPoke == 0)
		return messageClient(%cl,'',"\c7Sorry, \c6" @ %target.name SPC "\c7has disabled poking.");
	if(!isObject(%target))
		return messageClient(%cl,'',"\c7Your input \c6(" @ %target.name @ ") \c7doesn't seem to be a user that exists.");
	if(%id $="")
	{
		messageClient(%cl,'',"\c2Invalid syntax!");
		messageClient(%lc,'',"\c7Proper syntax: \c6/poke name");
		return;
	}
	
	commandToClient(%target,'MessageBoxOK',"Poke Notification",%cl.name SPC "has poked you. Close this dialogue box when you're back.");
	//we need a sound here, will add later
	messageClient(%target,'',"\c6" @ %cl.name SPC "\c7has poked you.");
	messageClient(%cl,'',"\c7You have successfully poked \c6" @ %target.name @ "\c7.");
