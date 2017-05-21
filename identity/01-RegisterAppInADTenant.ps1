param(
    [Parameter(Mandatory=$True, Position=0, HelpMessage="The name of the Azure Subscription")]
    [string]$Subscription,
    [Parameter(Mandatory=$True, Position=1, HelpMessage="The name of Application")]
    [string]$AppName
)

login-azurermaccount
set-azurermcontext -subscriptionname $Subscription
select-azuresubscription -subscriptionname $Subscription

$credentials = Get-Credential
Connect-MsolService -credential $credentials

$bytes = New-Object Byte[] 32
$rand = [System.Security.Cryptography.RandomNumberGenerator]::Create()
$rand.GetBytes($bytes)
$rand.Dispose()
$newClientSecret = [System.Convert]::ToBase64String($bytes)
$newClientSecret | Out-File ClientSecret.txt

New-MsolServicePrincipal -DisplayName $AppName -Type password -Value $newClientSecret | Out-File AppRegistration.txt

