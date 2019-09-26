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

foreach ($file in $assemblyFiles) {
	(Get-Content $file.PSPath) | ForEach-Object {
		if ($_ -match $pattern) {
			# We have found the matching line
			# Edit the version number and put back.
			$newVersion = "{0}.{1}.{2}.{3}" -f $major, $minor, $build, $revision
			write-host "Setting AssemblyInfo version to $newVersion"
			'[assembly: AssemblyVersion("{0}")]' -f $newVersion
		}
		else {
			# Output line as is
			$_
		}
	} | Set-Content $file.PSPath
}
