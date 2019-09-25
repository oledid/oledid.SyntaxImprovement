$pattern = '^\[assembly: AssemblyVersion\("(.*)"\)\]'
$assemblyFiles = Get-ChildItem -Recurse . AssemblyInfo.cs

$versionTxt = Get-Content version.txt -First 1
$splitVersion = ($versionTxt -replace "\+", ".").split(".")

$buildCounter = $splitVersion[3]
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
