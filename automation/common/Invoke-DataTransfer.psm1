function Invoke-DataTransfer { param ($RepoPath, $dataPath, $connStr, $collectionName)

	$dt = $RepoPath + "\Automation\Tools\dt\dt-1.7\dt.exe"
	$dtparams = "/s:JsonFile", "/s.Files:$dataPath\*.json", "/t:DocumentDBBulk", "/t.ConnectionString:$connStr", "/t.Collection:$collectionName", "/t.CollectionThroughput:400" #, "/t.PartitionKey:id" #, "/t.CollectionTier:S1"
	& $dt $dtparams
}
Export-ModuleMember -Function Invoke-DataTransfer
