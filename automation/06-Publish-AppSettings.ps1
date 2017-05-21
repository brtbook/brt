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
    [Parameter(Mandatory = $true, Position=7)]
    [String]$Database,
    [Parameter(Mandatory = $true, Position=8)]
    [String]$Collection
)

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

##########################################################################################
# M A I N
##########################################################################################

$Error.Clear()

# Mark the start time.
$StartTime = Get-Date

# Select Subscription
Set-AzureRmContext -SubscriptionName $Subscription;

try
{
    if ($ServiceName -eq "TelemetryAPI")
    {
        $Settings = @{
            "SQLConnStr" = "$SQLConnStr";
            "apikey" = "$SharedSecret";
        }
    }
    else
    {
        if ($ServiceName -eq "DeviceAPI")
        {
            $Settings = @{
                "docdburi" = "$docDbURI";
            "docdbkey" = "$docDbKey";
            "database" = "$Database";
            "collection" = "$Collection";
            "iothubhostname" = "$iothubhostname";
            "iothubconnstr" = "$iotHubConnectionString";
            "apiss" = "$sharedsecret";
            }
        }
        else {
            $Settings = @{
                "docdburi" = "$docDbURI";
                "docdbkey" = "$docDbKey";
                "database" = "$Database";
                "collection" = "$Collection";
                "apiss" = "$sharedsecret";
            }     
        }
    }

	New-AzureRmResource -Location $AzureLocation -PropertyObject $Settings -ResourceGroupName $ResourceGroup -ResourceType Microsoft.Web/sites/config -ResourceName "$Service/appsettings" -ApiVersion 2015-08-01 -Force

    Start-AzureRmWebApp -ResourceGroupName $ResourceGroup -Name $Service
}
catch
{
	Write-Verbose "Error provisioning App Service"
}

# Mark the finish time.
$FinishTime = Get-Date

#Console output
$TotalTime = ($FinishTime - $StartTime).TotalSeconds
Write-Verbose -Message "Elapse Time (Seconds): $TotalTime" -Verbose