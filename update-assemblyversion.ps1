$pattern = '^\[assembly: AssemblyVersion\("(.*)"\)\]'
$assemblyFiles = Get-ChildItem -Recurse . AssemblyInfo.cs

$contributorsJson = Get-Content contributors.json | out-string
$contributors = ConvertFrom-Json $contributorsJson
$numberOfCommits = ($contributors | Measure-Object -Property contributions -Sum).Sum

$buildCounter = $numberOfCommits
if ([string]::IsNullOrWhitespace($buildCounter)) {
	$buildCounter = "0"
}

$major = "0"
$minor = "4"
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

write-host Creating GitHub API release-request.json
'{ "tag_name": "v' + $newVersion + '", "target_commitish": "master", "name": "v' + $newVersion + '", "body": "GitHub Actions test-release", "draft": true, "prerelease": true }' | Out-File -FilePath "release-request.json"
