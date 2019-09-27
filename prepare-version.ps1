param (
    [string]$commitish = "master"
 )

############################
# Update Assembly version
############################

$pattern = '^\[assembly: AssemblyVersion\("(.*)"\)\]'
$assemblyFiles = Get-ChildItem -Recurse . AssemblyInfo.cs

$nugetIndexStr = Get-Content nuget-index.json | out-string
$nugetIndexJson = ConvertFrom-Json $nugetIndexStr
$lastPackageBuildNo = ($nugetIndexJson.items.upper | select-string -pattern '^\d+\.\d+\.(\d+).*').matches.groups[1].Value

$buildCounter = ([long]$lastPackageBuildNo + 1).ToString()

$major = "0"
$minor = "5"
$build = $buildCounter
$revision = "0"
$newVersion = "{0}.{1}.{2}" -f $major, $minor, $build
$newAssemblyVersion = "{0}.{1}.{2}.{3}" -f $major, $minor, $build, $revision

foreach ($file in $assemblyFiles) {
	(Get-Content $file.PSPath) | ForEach-Object {
		if ($_ -match $pattern) {
			# We have found the matching line
			# Edit the version number and put back.
			write-host "Setting AssemblyInfo version to $newAssemblyVersion"
			'[assembly: AssemblyVersion("{0}")]' -f $newAssemblyVersion
		}
		else {
			# Output line as is
			$_
		}
	} | Set-Content $file.PSPath
}

##########################################################
# Write the POST body for the GitHub API Release-command
##########################################################

write-host Creating GitHub API release-request.json
'{ "tag_name": "v' + $newVersion + '", "target_commitish": "' + $commitish + '", "name": "v' + $newVersion + '", "body": "Version ' + $newVersion + '", "draft": false, "prerelease": false }' | Out-File -FilePath "release-request.json"
