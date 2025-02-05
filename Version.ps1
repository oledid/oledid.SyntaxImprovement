# change these:
###################
$major = "1"
$minor = "1"
###################

$pattern = '^\[assembly: AssemblyVersion\("(.*)"\)\]'
$assemblyFiles = Get-ChildItem -Recurse . AssemblyInfo.cs

$nugetIndexStr = curl https://api.github.com/repos/oledid/oledid.SyntaxImprovement/releases/latest -s | jq .name -r
if ($a -match "v(\d+)\.(\d+)\.(\d+)") {
    $lastPackageMajor = $matches[1]
    $lastPackageMinor = $matches[2]
    $lastPackageBuildNo = $matches[3]
}
else {
	$lastPackageMajor = "0"
    $lastPackageMinor = "0"
    $lastPackageBuildNo = "0"
}
$buildCounter = ([long]$lastPackageBuildNo + 1).ToString()
if ("$major.$minor" -ne "$lastPackageMajor.$lastPackageMinor") {
	$buildCounter = "0"
}
else {
    $buildCounter = "0"
}

$build = $buildCounter
$revision = "0"
$newVersion = "{0}.{1}.{2}" -f $major, $minor, $build
$newAssemblyVersion = "{0}.{1}.{2}.{3}" -f $major, $minor, $build, $revision
$build_version = "{0}.{1}.{2}-beta" -f $major, $minor, $build;
Write-Host "Variables set, `$build_version = $build_version"
