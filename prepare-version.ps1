param (
    [string]$commitish = "master"
 )

############################
# Update Assembly version
############################

. ./Version.ps1

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
