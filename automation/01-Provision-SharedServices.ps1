param(
    [Parameter(Mandatory=$True, Position=0, HelpMessage="The name of the Azure Subscription")]
    [string]$Subscription,
    [Parameter(Mandatory=$True, Position=1, HelpMessage="The name of the resource group.")]
    [string]$ResourceGroup,
    [Parameter(Mandatory=$True, Position=2, HelpMessage="The name of the Azure Region/Location: East US, Central US, West US.")]
    [string]$AzureLocation,
    [Parameter(Mandatory=$True, Position=3, HelpMessage="The common prefix for resources naming: 'd2c2d' or 'lab'")]
    [string]$Prefix,
    [Parameter(Mandatory=$True, Position=4, HelpMessage="The suffix which is one of 'dev', 'test' or 'prod'")]
    [string]$Suffix
)

#######################################################################################
# S E T  P A T H
#######################################################################################

$Path = Split-Path -parent $PSCommandPath
$Path = Split-Path -parent $path

##########################################################################################
# F U N C T I O N S
##########################################################################################

Function RegisterRP {
    Param(
        [string]$ResourceProviderNamespace
    )

    Write-Host "Registering resource provider '$ResourceProviderNamespace'";
    Register-AzureRmResourceProvider -ProviderNamespace $ResourceProviderNamespace -Force;
}

function GenerateUniqueResourceName()
{
    Param(
        [Parameter(Mandatory=$true,Position=0)] [string] $resourceBaseName
    )
    $name = $resourceBaseName
    $name = "{0}{1:x5}" -f $resourceBaseName, (get-random -max 1048575)
    return $name
}

##########################################################################################
# V A R I A B L E S
##########################################################################################

$storageAccountName = $Prefix + "blobstorage" + $Suffix
$storageAccountType = "Standard_LRS"
$serviceBusNamespace = $Prefix + "servicebusns" + $Suffix
$serviceBusNamespace = GenerateUniqueResourceName($serviceBusNamespace)
$databaseAccount = $Prefix + "docdb" + $Suffix
$iotHubName = $Prefix + "iothub" + $Suffix
$sqlServer = $Prefix + "sqlserver" + $Suffix
$sqlAdminUserid = $Prefix + "Admin" + $Suffix
$sqlAdminPassword = "Cloverfield7"
$sqlDatabaseName = "IoTHubTelemetry"
$firewallRules_AllowAllWindowsAzureIps = "$sqlServer/AllowAllWindowsAzureIps"

##########################################################################################
# M A I N
##########################################################################################

$Error.Clear()

# Mark the start time.
$StartTime = Get-Date

$ErrorActionPreference = "Stop"

# select subscription
Set-AzureRmContext -SubscriptionName $Subscription;

#Create or check for existing resource group
$rg = Get-AzureRmResourceGroup -Name $ResourceGroup -ErrorAction SilentlyContinue 
if(!$rg)
{
    Write-Host "Resource group '$ResourceGroup' does not exist.";
    Write-Host "Creating resource group '$ResourceGroup' in location '$AzureLocation'";
    New-AzureRmResourceGroup -Name $ResourceGroup -Location $AzureLocation
}
else{
    Write-Host "Using existing resource group '$ResourceGroup'";
}

Write-Host "Generating Template Parameter File";

Try
{
        $JSON = @"
{
    "$schema": "https://schema.management.azure.com/schemas/2015-01-01/deploymentTemplate.json#",
    "contentVersion": "1.0.0.0",
    "parameters": {
        "azureLocation": {
          "value": "$AzureLocation"
        },
        "storageAccountName": {
          "value": "$storageAccountName"
        },
        "storageAccountType": {
          "value": "$storageAccountType"
        },
		"servicebusNamespace": {
		  "value": "$servicebusNamespace"
		},
	    "docDbAccount": {
          "value": "$databaseAccount"
        },
        "iotHubName": {
          "value": "$iotHubName"
        },
		"sqlServer" : {
			"value": "$sqlServer"
		},
		"sqlAdminUserId": {
			"value": "$sqlAdminUserid"
		},
		"sqlAdminPassword": {
			"value": "$sqlAdminPassword"
		},
		"sqlDatabaseName": {
			"value": "$sqlDatabaseName"
		},
		"firewallRules_AllowAllWindowsAzureIps": {
			"value": "$firewallRules_AllowAllWindowsAzureIps"
		},
        "prefix":{"value":"$Prefix"},
        "suffix":{"value":"$Suffix"},
    }
}
"@
    $ParamsPath = $Path + "\Automation\Templates\brt-arm-template-params.json"
    $TemplatePath = $Path + "\Automation\Templates\brt-arm-template.json"
    $OutputPath = $Path + "\Automation\provision-$ResourceGroup-output.json"

    $JSON | Set-Content -Path $ParamsPath

	# validate the template files
	Test-AzureRmResourceGroupDeployment -ResourceGroupName $ResourceGroup -TemplateFile $TemplatePath -TemplateParameterFile  $ParamsPath -Verbose

	# perform the deployment
    New-AzureRmResourceGroupDeployment -DeploymentDebugLogLevel All -ResourceGroupName $ResourceGroup -TemplateFile $TemplatePath -TemplateParameterFile $ParamsPath | ConvertTo-Json | Out-File  "$OutputPath"
}
Catch
{
    Write-Verbose -Message $Error[0].Exception.Message
    Write-Verbose -Message "Exiting due to exception: Shared Services Not Created."
}

# Mark the finish time.
$FinishTime = Get-Date

#Console output
$TotalTime = ($FinishTime - $StartTime).TotalSeconds
Write-Verbose -Message "Elapse Time (Seconds): $TotalTime" -Verbose