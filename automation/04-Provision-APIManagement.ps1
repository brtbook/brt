[CmdletBinding()]
Param(
    [Parameter(Mandatory=$True, Position=0, HelpMessage="The name of the subscription")]
    [string]$Subscription,
    [Parameter(Mandatory=$True, Position=1, HelpMessage="The name of the Resource Group.")]
    [string]$ResourceGroup,
    [Parameter(Mandatory=$True, Position=2, HelpMessage="The name of the Azure Region/Location: East US, Central US, West US.")]
    [string]$AzureLocation,
    [Parameter(Mandatory=$True, Position=4, HelpMessage="The common prefix for resources naming: looksfamiliar")]
    [string]$Prefix,
    [Parameter(Mandatory=$True, Position=5, HelpMessage="The suffix which is one of 'dev', 'test' or 'prod'")]
    [string]$Suffix,
    [Parameter(Mandatory=$True, Position=6, HelpMessage="The organization")]
    [string]$Organization,
    [Parameter(Mandatory=$True, Position=7, HelpMessage="The API Service Name")]
    [string]$APIServiceName,
    [Parameter(Mandatory=$True, Position=6, HelpMessage="The email of the API Adminstrator")]
    [string]$APIAdminEmail
)

#######################################################################################
# F U N C T I O N S
#######################################################################################

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

#######################################################################################
# S E T  P A T H
#######################################################################################

$Path = Split-Path -parent $PSCommandPath
$Path = Split-Path -parent $path

#######################################################################################
# M A I N 
#######################################################################################

$Error.Clear()

# Mark the start time.
$StartTime = Get-Date

Select-Subscription $Subscription

# Create API Management 
New-AzureRmAPIManagement -ResourceGroupName $ResourceGroup -Location $AzureLocation -Sku Developer -Organization $Organization -Name $APIServiceName -AdminEmail $APIAdminEmail

# Mark the finish time.
$FinishTime = Get-Date

#Console output
$TotalTime = ($FinishTime - $StartTime).TotalSeconds
Write-Verbose -Message "Elapse Time (Seconds): $TotalTime" -Verbose