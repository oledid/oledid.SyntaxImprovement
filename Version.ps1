# change these:
###################
$major = "0"
$minor = "12"
###################

$pattern = '^\[assembly: AssemblyVersion\("(.*)"\)\]'
$assemblyFiles = Get-ChildItem -Recurse . AssemblyInfo.cs

$apiFilePath = "nuget-index.json"
if (Test-Path $apiFilePath -PathType Leaf) {
    $nugetIndexStr = Get-Content $apiFilePath | out-string
    $nugetIndexJson = ConvertFrom-Json $nugetIndexStr
    $lastPackageBuildNo = ($nugetIndexJson.items.upper | select-string -pattern '^\d+\.\d+\.(\d+).*').matches.groups[1].Value
    $lastPackageMajorMinor = ($nugetIndexJson.items.upper | select-string -pattern '^(\d+\.\d+)\.\d+.*').matches.groups[1].Value
    $buildCounter = ([long]$lastPackageBuildNo + 1).ToString()
    if ("$major.$minor" -ne $lastPackageMajorMinor) {
        $buildCounter = "0"
    }
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
