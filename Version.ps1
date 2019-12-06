$pattern = '^\[assembly: AssemblyVersion\("(.*)"\)\]'
$assemblyFiles = Get-ChildItem -Recurse . AssemblyInfo.cs

$nugetIndexStr = Get-Content nuget-index.json | out-string
$nugetIndexJson = ConvertFrom-Json $nugetIndexStr
$lastPackageBuildNo = ($nugetIndexJson.items.upper | select-string -pattern '^\d+\.\d+\.(\d+).*').matches.groups[1].Value

$buildCounter = ([long]$lastPackageBuildNo + 1).ToString()

$major = "0"
$minor = "8"
$build = $buildCounter
$revision = "0"
$newVersion = "{0}.{1}.{2}" -f $major, $minor, $build
$newAssemblyVersion = "{0}.{1}.{2}.{3}" -f $major, $minor, $build, $revision

$build_version = "{0}.{1}.{2}-beta" -f $major, $minor, $build