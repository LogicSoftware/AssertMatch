Import-Module "../packages/psake.4.7.0/tools/psake/psake.psm1"
$apiKey = Read-Host -Prompt "Enter nuget api key";
$version = Read-Host -Prompt "Enter version";
Invoke-psake .\default.ps1 -parameters @{ "nuget_api_key" = $apiKey; "version" = $version}