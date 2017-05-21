
$Path = Split-Path -parent $PSCommandPath
$Path = Split-Path -parent $path

$UserJsonPath = $Path + "\identity\UserJson"
$B2CGraphClient = $Path + "\identity\B2C-GraphClient-DotNet\B2CGraphClient\bin\debug\b2c.exe"
$files = Get-ChildItem $UserJsonPath

for ($i=0; $i -lt $files.Count; $i++) {

    $filename = $files[$i].FullName
    
    $params = "Create-User", $filename
    & $B2CGraphClient $params

    Write-Host $filename " imported"
}
