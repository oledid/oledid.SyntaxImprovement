$thisPath = $MyInvocation.MyCommand.Definition
$root = Resolve-Path "$thisPath/../.."
$version = [System.Reflection.Assembly]::LoadFile("$root/src/SyntaxImprovement/bin/Release/netstandard2.0/oledid.SyntaxImprovement.dll").GetName().Version
$versionStr = "{0}.{1}.{2}-beta" -f ($version.Major, $version.Minor, $version.Build)

Write-Host "Setting .nuspec version tag to $versionStr"

$content = (Get-Content $root/NuGet/oledid.SyntaxImprovement.nuspec) 
$content = $content -replace '\$version\$',$versionStr
$content | Out-File "$root/oledid.SyntaxImprovement.compiled.nuspec"

& nuget pack oledid.SyntaxImprovement.compiled.nuspec