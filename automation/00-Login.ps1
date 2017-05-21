[CmdletBinding()]
Param(
    [Parameter(Mandatory=$True, Position=0, HelpMessage="The storage account name.")]
    [string]$Subscription
)

login-azurermaccount
add-azureaccount
set-azurermcontext -subscriptionname $Subscription
select-azuresubscription -subscriptionname $Subscription
get-azurermsubscription