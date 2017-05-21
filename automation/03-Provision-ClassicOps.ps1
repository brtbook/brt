[CmdletBinding()]
Param(
    [Parameter(Mandatory=$True, Position=0, HelpMessage="The storage account name.")]
    [string]$Subscription,
    [Parameter(Mandatory=$True, Position=1, HelpMessage="The resource group name.")]
    [string]$ResourceGroup
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

##########################################################################################
# M A I N
##########################################################################################

Select-AzureSubscription -SubscriptionName $Subscription

$StartTime = Get-Date

$storageKey = (Get-AzureRmStorageAccountKey -AccountName $storageAccountName -ResourceGroupName $ResourceGroup)[0]
$StorageContext = New-AzureStorageContext -StorageAccountName $storageAccountName -StorageAccountKey $storageKey.Value

New-AzureStorageContainer -Context $StorageContext -Name $ArchiveContainerName -Permission Off -ErrorAction SilentlyContinue
New-AzureStorageContainer -Context $StorageContext -Name $RefDataContainerName -Permission Off -ErrorAction SilentlyContinue
New-AzureStorageContainer -Context $StorageContext -Name $ImageContainerName -Permission Off -ErrorAction SilentlyContinue

# Upload the rules file to the referecen daa container
$refdata = $path + "\automation\deploy\rules\$TempSensorRulesFilename"
Set-AzureStorageBlobContent -Context $StorageContext -Container $RefDataContainerName -File $refdata -Force

# Upload the image files to the image container
# $_.mode -match "-a---" scans the data directory and ony fetches the files. It filters out all directories
$imagedir = $path + "\automation\deploy\images"
$files = Get-ChildItem $imagedir -force| Where-Object {$_.mode -match "-a---"}
 
# iterate through all the files and start uploading data
foreach ($file in $files)
{
    #fq name represents fully qualified name
    $fqName = $imagedir + "\" + $file.Name

    #upload the current file to the blob
    Set-AzureStorageBlobContent -Blob $file.Name -Context $StorageContext -Container $ImageContainerName -File $fqName -Force
}

$sbr = Get-AzureSBAuthorizationRule -Namespace $serviceBusNamespace

$JSON =@"
{ 
    "ServiceBusConnectionString": "$sbr.ConnectionString",
}
"@

$ServiceBusInfo = $Path + "\automation\servicebus-$resourcegroup-output.json"
$JSON | Set-Content -Path $ServiceBusInfo
    
# Mark the finish time.
$FinishTime = Get-Date

#Console output
$TotalTime = ($FinishTime - $StartTime).TotalSeconds
Write-Verbose -Message "Elapse Time (Seconds): $TotalTime"





