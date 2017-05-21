[CmdletBinding()]
param
(
    [Parameter(Mandatory=$True, Position=0)]
    [string]$Subscription,
    [Parameter(Mandatory=$True, Position=1)]
    [string]$ResourceGroup,
    [Parameter(Mandatory=$True, Position=2)]
    [string]$AzureLocation,
    [Parameter(Mandatory=$True, Position=3)]
    [string]$Prefix,
    [Parameter(Mandatory=$True, Position=4)]
    [string]$Suffix,
    [Parameter(Mandatory = $true,Position=5)]
    [String]$ServiceName,
    [Parameter(Mandatory = $true, Position=6)]
    [String]$ServicePlan,
    [Parameter(Mandatory = $true, Position=7)]
    [String]$Database,
    [Parameter(Mandatory = $true, Position=8)]
    [String]$Collection)

#######################################################################################
# S E T  P A T H
#######################################################################################

$Path = Split-Path -parent $PSCommandPath
$Path = Split-Path -parent $path

##########################################################################################
# V A R I A B L E S
##########################################################################################

$includePath = $Path + "\Automation\EnvironmentVariables.ps1"
."$includePath"

$Service = $Prefix + $ServiceName + $Suffix
$AppPlan = $Prefix + $ServicePlan + $Suffix

##########################################################################################
# M A I N
##########################################################################################

$Error.Clear()

# Mark the start time.
$StartTime = Get-Date

# Select Subscription
Set-AzureRmContext -SubscriptionName $Subscription;

try {
    

    $TemplateFile = $Path + "\Automation\templates\brt-arm-template-website.json";
    $ParametersFile = $Path + "\Automation\Templates\brt-arm-template-params-website.json";

    Write-Host "Generating Template Parameter File";

    $schema = '$schema';

    $JSON = @"
{
    "$schema": "https://schema.management.azure.com/schemas/2015-01-01/deploymentTemplate.json#",
    "contentVersion": "1.0.0.0",
    "parameters": {
        "azureLocation": { "value": "$AzureLocation" },
        "appSiteName": { "value": "$Service" },
        "appServicePlan": { "value": "$AppPlan" },
        "appServicePlanSku": { "value": "Standard" },
        "appServicePlanSkuSize": { "value": "0" }
    }
}
"@
	$JSON | Set-Content -Path $ParametersFile

	Write-Verbose $TemplateFile
	Write-Verbose $ParametersFile
	Write-Verbose $JSON

	#Test-AzureRmResourceGroupDeployment -ResourceGroupName $ResourceGroup -TemplateParameterFile $ParametersFile -TemplateFile $TemplateFile -Verbose

    Stop-AzureRmWebApp -ResourceGroupName $ResourceGroup -Name $Service -ErrorAction Continue

	New-AzureRmResourceGroupDeployment -DeploymentDebugLogLevel All -ResourceGroupName $ResourceGroup -TemplateParameterFile $ParametersFile -TemplateFile $TemplateFile -Force
}
catch
{
    Write-Verbose -Message $Error[0].Exception.Message
	Write-Verbose "Error provisioning App Service"
}

# Mark the finish time.
$FinishTime = Get-Date

#Console output
$TotalTime = ($FinishTime - $StartTime).TotalSeconds
Write-Verbose -Message "Elapse Time (Seconds): $TotalTime" -Verbose