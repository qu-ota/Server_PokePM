exec("./Support_UpdaterDownload.cs");

if(!isFile("config/server/rules.cs"))
{
	$rules::RuleNumber[$rules::RuleTotal++] = "1. Do not spam.";
	$rules::RuleNumber[$rules::RuleTotal++] = "2. Do not cause needless arguments.";
	$rules::RuleNumber[$rules::RuleTotal++] = "3. Treat other players with respect.";
	$rules::RuleNumber[$rules::RuleTotal++] = "4. Have fun!";
	export("$rules::rule*", "config/server/rules.cs");
}
else
	exec("config/server/rules.cs");
	
if($GameModeArg !$= "Add-Ons/GameMode_Custom/gamemode.txt")
{
	if(isFile(filePath($GameModeArg) @ "/rules.txt"))
	{
		%file = new FileObject(){};
		
		%file.openForRead(filePath($GameModeArg) @ "/rules.txt");
		
		while(!%file.isEOF)
		{
			%count++;
			$Rules::RuleNumber[%count] = %file.readLine();
		}
		
		echo("Gamemode rule file successfully read.");
	}
}

for($i = 1; $i <= $rules::RuleTotal; $i++)
{
	if($rules::RuleNumber[$i] $= "")
		break;
	
	$z = $z @ "<br>" @ $rules::RuleNumber[$i];
}

$rules::RuleList = $z;


package betterRules
{
	function servercmdmissionstartphase3ack(%client, %bool)
	{
		if($rules::acceptedClients[%client.bl_id])
		{
			echo(%client.name SPC "has accepted the rules and was allowed to spawn.");
			return parent::servercmdmissionstartphase3ack(%client, %bool);
		}
			
		commandToClient(%client, 'messageBoxYesNo', "Rules", $rules::RuleList @ "<br><br>Please select YES below if you agree to abide by these rules", 'acceptRules');
		
		return;
	}	
};

activatepackage(betterRules);

function servercmdAcceptRules(%client)
{
	if($rules::acceptedClients[%client.bl_id] || %client.isSpawned)
		return;
	
	$rules::acceptedClients[%client.bl_id] = 1;
	export("$rules::Rule*", "config/server/rules.cs");
	servercmdmissionstartphase3ack(%client, 1);
	%client.isSpawned = 1;
}

function servercmdRules(%client)
{
	if(%client.isAdmin)
	{
		for(%i = 1; %i <= $rules::RuleTotal; %i++)
		{
			if($rules::RuleNumber[%i] $= "")
				break;
			
			%ruleBox = %ruleBox @ "<br>" @ $rules::RuleNumber[%i];
		}
		
		$rules::RuleList = %ruleBox;
	}
	
	commandToClient(%client, 'messageBoxOK', "Rules", $rules::RuleList);
}

function servercmdSetRule(%client, %number, %word1, %word2, %word3, %word4, %word5, %word6, %word7, %word8, %word9, %word10, %word11, %word12, %word13)
{
	if(!%client.isSuperAdmin)
		return;
		
	if(%number-1 < 0)
	{
		%client.chatMessage("Invalid rule number");
		return;
	}
		
	for(%i = 0; %i < 13; %i++)
		%words = %words SPC %word[%i];
		
	%words = strReplace(trim(%words), "_", " ");
	
	$rules::RuleNumber[%number] = %words;
	
	if(%number > $rules::RuleTotal)
		$rules::RuleTotal = %number;
		
	for(%i = 1; %i <= $rules::RuleTotal; %i++)
	{
		if($rules::RuleNumber[%i] $= "")
			break;
		
		%ruleBox = %ruleBox @ "<br>" @ $rules::RuleNumber[%i];
	}
	
	%client.chatMessage("Remember to use _underscores_ instead of spaces to make longer rules. Do /rules to view how your rules currently look.");
	%client.chatMessage("Rule" SPC %number SPC "has been set to\c6" SPC $rules::RuleNumber[%number]);
	
	export("$rules::Rule*", "config/server/rules.cs");
}