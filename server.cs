//HMM POKE AND DIRECT MESSAGE
//By Dominoes

if(isFile("Add-Ons/System_ReturnToBlockland/server.cs") || isFile("Add-Ons/System_BlocklandGlass/server.cs"))
{
	if(!$RTB::RTBR_ServerControl_Hook)
	{
		RTB_registerPref("Enable Poke?","Poke and PM","Pref::PokePM::EnablePoke","bool","Server_PokePM",1,0,0);
		RTB_registerPref("Enable PM?","Poke and PM","Pref::PokePM::EnablePM","bool","Server_PokePM",1,0,0);
	}
}
else
{
	$Pref::PokePM::EnablePoke = 1; //Enable poking others?
	$Pref::PokePM::EnablePM = 1; //Allow users to private message others?
}

function reallowPokePM(%client)
{
	%client.sentPoke = 0;
}

function reallowPM(%cl)
{
	%cl.sentPM = 0;
}

function serverCmdPoke(%cl, %id)
{
	%t = findclientbyname(%id);
	
	if($Pref::PokePM::EnablePoke == 0)
		return messageClient(%cl,'',"\c7The server host has \c0disabled \c7poking other users.");
	if(%target.allowPoke == 0)
		return messageClient(%cl,'',"\c6" @ %t.name SPC "\c7has disabled poking.");
	if(!isObject(%t))
		return messageClient(%cl,'',"\c7Your input \c6(" @ %t.name @ ") \c7doesn't seem to be a user that exists.");
	if(%cl.sentPoke == 1)
		return messageClient(%cl,'',"\c7You've already poked someone, try again in a few seconds. \c6(10 second cooldown)");
	
	commandToClient(%t,'MessageBoxOK',"Poke Notification",%cl.name SPC "has poked you. Close this dialogue box when you're back.");
	//we need a sound here, will add later
	messageClient(%t,'',"\c6" @ %cl.name SPC "\c7has poked you.");
	messageClient(%cl,'',"\c7You have successfully poked \c6" @ %t.name @ "\c7.");
	%cl.sentPoke = 1;
	schedule(10000, 0, reallowPoke, %cl);
}

function serverCmdAllowPoke(%cl)
{
	if($Pref::PokePM::EnablePoke == 0)
		return messageClient(%cl,'',"\c7The server host has \c0disabled \c7poking.");
	if(%cl.allowPoke == 0)
	{
		messageClient(%cl,'',"\c7You have \c3enabled \c7poking; other users can now send you poke notifications.");
		%cl.allowPoke = 1;
	}
	else
	{
		messageClient(%cl,'',"\c7You have \c0disabled \c7poking; other users can no longer send you poke notifications.");
		%cl.allowPoke = 0;
	}
}

function serverCmdPM(%cl,%id,%c1,%c2,%c3,%c4,%c5,%c6,%c7,%c8,%c9,%c10,%c11,%c12,%c13,%c14,%c16,%c17,%c18,%c19,%c20)
{
	%t = findClientByName(%id);
	
	if($Pref::PokePM::EnablePM == 0)
		return messageClient(%cl,'',"\c7The host has \c0disabled \c7private messaging between others.");
	if(%t.allowPM == 0)
		return messageClient(%cl,'',"\c7The user you've entered has disabled receiving PMs.");
	if(!isObject(%t))
		return messageClient(%cl,'',"\c7Your input \c6(" @ %t.name @ ") \c7doesn't seem to be a user that exists.");
	if(%cl.sentPM == 1)
		return messageClient(%cl,'',"\c7You've already sent a private message, please wait a bit before trying again. \c6(10 second cooldown)");
	
	for(%a = 1; %a < 21; %a++)
	{
		if(%c[%a] !$= "")
		{
			%pm = %pm SPC %c[%a];
		}
	}
	
	%pm = stripMLControlChars(trim(%pm));
	messageClient(%cl,'',"\c3" @ %cl.name SPC "\c7[PM to" @ %t.name @ "]\c6:" SPC %pm);
	messageClient(%t,'',"\c3" @ %cl.name SPC "\c7[PM from" @ %cl.name @ "]\c6:" SPC %pm);
	%cl.sentPM = 1;
	schedule(10000,0,reallowPM,%cl);
}

function serverCmdAllowPM(%cl)
{
	if($Pref::PokePM::EnablePM == 0)
		return messageClient(%cl,'',"\c7The server host has \c0disabled \c7private messaging between others.");
	if(%cl.allowPM == 0)
	{
		messageClient(%cl,'',"\c7You have \c3enabled \c7the receiving of private messages; other users can now send you private messages.");
		%cl.allowPM = 1;
	}
	else
	{
		messageClient(%cl,'',"\c7You have \c0disabled \c7the receiving of private messages; other users can no longer send them to you.");
		%cl.allowPoke = 0;
	}
}
