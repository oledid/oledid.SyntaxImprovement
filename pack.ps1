. ./Version.ps1

dotnet pack --configuration Release /p:Version=$build_version -p:IncludeSymbols=true
