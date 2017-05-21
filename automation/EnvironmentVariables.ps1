##################################################################
# E N V I R O N M E N T  V A R I A B L E S
##################################################################

$provisionOutputPath = $Path + "\automation\provision-$ResourceGroup-output.json"

$provisionInfo = ConvertFrom-Json -InputObject (Gc $provisionOutputPath -Raw)

$iotHubHostName = $provisionInfo.Outputs.iotHubHostName.Value
$iotHubKey = $provisionInfo.Outputs.iotHubKey.Value
$iotHubConnectionString = $provisionInfo.Outputs.iotHubConnectionString.Value
$iotHubname = $provisionInfo.Parameters.iotHubname.Value
$iothubkeyname = "iothubowner"

$docDbURI = $provisionInfo.Outputs.docDbURI.Value
$docDbKey = $provisionInfo.Outputs.docDbKey.Value
$docDbConnectionString = $provisionInfo.Outputs.docDbConnectionString.Value
$databaseAccount = $provisionInfo.Parameters.databaseAccount.Value

$storageAccountName = $provisionInfo.Parameters.storageAccountName.Value
$StorageAccountKey = (Get-AzureRmStorageAccountKey -AccountName $storageAccountName -ResourceGroupName $ResourceGroup)[0].Value

$serviceBusNamespace = $provisionInfo.Parameters.serviceBusNamespace.Value

$sqlServer = $provisionInfo.Parameters.sqlServer.Value
$sqlAdminUserid = $provisionInfo.Parameters.sqlAdminUserId.Value
$sqlAdminPassword = $provisionInfo.Parameters.sqlAdminPassword.value
$sqlDatabaseName = $provisionInfo.Parameters.sqlDatabaseName.Value
$sqlDatabaseTableName = "IoTHubSensorReadings"
$sqlConnStr = "Server=tcp:$sqlServer.database.windows.net,1433;Initial Catalog=$sqlDatabaseName;Persist Security Info=False;User ID=$sqlAdminUserid;Password=$sqlAdminPassword;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"

$ArchiveContainerName = "messages"
$RefDataContainerName = "refdata"
$ImageContainerName = "images"

$TempSensorRulesFilename = "TempSensorRules.json"

$AlarmsQueue = "alarms"
$AlertsQueue = "alerts"

##################################################################
# S T R E A M  A N A L Y T I C S
##################################################################

$ASAArchive = $Prefix + "-archive-blob"
$ASASensorReadings = $Prefix + "-sensorreadings-sqldb"
$ASAAlerts = $Prefix + "-alerts-queue"
$ASAAlarms = $Prefix + "-alarms-queue"

##################################################################
# A P I  S H A R E D  S E C R E T
##################################################################

$SharedSecret = "[provide-shared-secret-here]"

##################################################################
# A P P  S E R V I C E S
##################################################################

$appServicePlan = $Prefix + "AppServicePlan" + $Suffix 

$AccountAPI = $Prefix + "AccountAPI" + $Suffix
$ApplicationAPI = $Prefix + "ApplicationAPI" + $Suffix
$CustomerAPI = $Prefix + "CustomerAPI" + $Suffix
$DeviceAPI = $Prefix + "DeviceAPI" + $Suffix
$ReferenceAPI = $Prefix + "ReferenceAPI" + $Suffix
$RegistryAPI = $Prefix + "RegistryAPI" + $Suffix
$SimulationAPI = $Prefix + "SimulationAPI" + $Suffix

##################################################################
# D O C U M E N T D B
##################################################################

$AccountDatabase = "Account"
$AccountCollection = "Subscription"
$ApplicationDatabase = "Application"
$ApplicationCollection = "Configuration"
$CustomertDatabase = "Customer"
$CustomerCollection = "Orginization"
$DeviceDatabase = "Device"
$DeviceCollection = "Manifest"
$ReferenceDatabase = "Reference"
$ReferenceCollection = "Entity"
$RegistryDatabase = "Registry"
$RegistryCollection = "Profile"
$SimulationDatabase = "Simulation"
$SimulationCollection = "DataSets"
