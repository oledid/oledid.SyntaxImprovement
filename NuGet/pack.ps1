$root = (split-path -parent $MyInvocation.MyCommand.Definition) + '\..'
$version = [System.Reflection.Assembly]::LoadFile("$root\src\SyntaxImprovement\Release\oledid.SyntaxImprovement.dll").GetName().Version
$versionStr = "{0}.{1}.{2}" -f ($version.Major, $version.Minor, $version.Build)

Write-Host "Setting .nuspec version tag to $versionStr"

$content = (Get-Content $root\NuGet\oledid.SyntaxImprovement.nuspec) 
$content = $content -replace '\$version\$',$versionStr

$content | Out-File $root\nuget\oledid.SyntaxImprovement.compiled.nuspec

& nuget pack $root\nuget\oledid.SyntaxImprovement.compiled.nuspec