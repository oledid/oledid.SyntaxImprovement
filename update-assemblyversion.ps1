$pattern = '^\[assembly: AssemblyVersion\("(.*)"\)\]'
$assemblyFiles = Get-ChildItem -Recurse . AssemblyInfo.cs

$versionTxt = Get-Content version.txt -First 1
$splitVersion = ($versionTxt -replace "\+", ".").split(".")

$majorVersion = $splitVersion[0]
$minorVersion = $splitVersion[1]
$buildVersion = $splitVersion[2]
$buildCounter = $splitVersion[3]

foreach ($file in $assemblyFiles) {
	(Get-Content $file.PSPath) | ForEach-Object {
		if ($_ -match $pattern) {
			# We have found the matching line
			# Edit the version number and put back.
			$newVersion = "{0}.{1}.{2}.{3}" -f $majorVersion, $minorVersion, $buildVersion, $buildCounter
			write-host "Setting AssemblyInfo version to $newVersion"
			'[assembly: AssemblyVersion("{0}")]' -f $newVersion
		}
		else {
			# Output line as is
			$_
		}
	} | Set-Content $file.PSPath
}
