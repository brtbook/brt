function Invoke-SQLUpdate { param ($RepoPath, $SQLServer, $SQLDatabase, $SQLUsername, $SQLPassword)

	$sqlupdate = $RepoPath + "\Automation\Tools\sqlupdate\SQLUpdate.exe"
	$sqlupdateparams = "-server", $SQLServer, "-database", $SQLDatabase, "-username", $SQLUsername, "-password", $SQLPassword
	& $sqlupdate $sqlupdateparams
}
Export-ModuleMember -Function Invoke-SQLUpdate 