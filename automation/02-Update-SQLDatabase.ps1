param(
    [Parameter(Mandatory=$True, Position=0, HelpMessage="The name of the Azure Subscription for which you've imported a *.publishingsettings file.")]
    [string]$Subscription,
    [Parameter(Mandatory=$True, Position=1, HelpMessage="The name of the resource group.")]
    [string]$ResourceGroup,
    [Parameter(Mandatory=$True, Position=2, HelpMessage="The name of the Azure Region/Location: East US, Central US, West US.")]
    [string]$AzureLocation,
    [Parameter(Mandatory=$True, Position=3, HelpMessage="The common prefix for resources naming: looksfamiliar")]
    [string]$Prefix,
    [Parameter(Mandatory=$True, Position=4, HelpMessage="The suffix which is one of 'dev', 'test' or 'prod'")]
    [string]$Suffix
)

#######################################################################################
# S E T  P A T H
#######################################################################################

$Path = Split-Path -parent $PSCommandPath
$Path = Split-Path -parent $path

#######################################################################################
# I M P O R T S
#######################################################################################

$sqlUpdate = $Path + "\Automation\Common\Invoke-SQLUpdate.psm1"
Import-Module -Name $sqlUpdate

##########################################################################################
# F U N C T I O N S
##########################################################################################

Function Select-Subscription()
{
    Param([String] $Subscription)

    Try
    {
        Set-AzureRmContext  -SubscriptionName $Subscription -ErrorAction Stop
    }
    Catch
    {
        Write-Verbose -Message $Error[0].Exception.Message
    }
}

Function Get-ExternalIPAddress()
{
    $WebClient = New-Object System.Net.WebClient
    Return $WebClient.downloadstring("http://checkip.dyndns.com") -replace "[^\d\.]"
}

##########################################################################################
# V A R I A B L E S
##########################################################################################

$includePath = $Path + "\Automation\EnvironmentVariables.ps1"
."$includePath"

$firewallRules_AllowAllWindowsAzureIps = "$sqlServer/AllowAllWindowsAzureIps"
$firewallRules_ClientIPAddress = Get-ExternalIPAddress

##########################################################################################
# M A I N
##########################################################################################

$Error.Clear()

# Mark the start time.
$StartTime = Get-Date

$ErrorActionPreference = "Stop"

# select subscription
Set-AzureRmContext -SubscriptionName $Subscription;

#########################################################################
# S Q L  D A T A B A S E  T A B L E S
#########################################################################

try
{
	$TcpIPAddress = Get-ExternalIPAddress

	# Create SQL Database Server Firewall Rule for this client computer ONLY.
	#New-AzureRmSqlServerFirewallRule -ResourceGroupName $ResourceGroup -ServerName $sqlServer -FirewallRuleName $env:COMPUTERNAME -StartIpAddress $TcpIPAddress -EndIpAddress $TcpIPAddress -ErrorAction SilentlyContinue
	#Start-Sleep -Seconds 60

	#write-verbose $sqlServer
	#write-verbose $sqlDatabaseName
	#write-verbose $sqlAdminUserid
	#write-verbose $sqlAdminPassword

	Invoke-SQLUpdate $Path $sqlServer $sqlDatabaseName $sqlAdminUserid $sqlAdminPassword

}catch
{
    Write-Verbose -Message $Error[0].Exception.Message
    Write-Verbose -Message "Exiting due to exception"
}

# Mark the finish time.
$FinishTime = Get-Date

#Console output
$TotalTime = ($FinishTime - $StartTime).TotalSeconds
Write-Verbose -Message "Elapse Time (Seconds): $TotalTime" -Verbose