Param ([string]$build_num)

$git_version = (git describe --tags --long --match Version_?.? | Select-String -pattern '(?<major>[0-9]+)\.(?<minor>[0-9]+)-(?<seq>[0-9]+)-(?<hash>[a-z0-9]+)').Matches[0].Groups
 
$git_describe = $git_version[0].Value
 
$version = [string]::Join('.', @(
	$git_version['major'],
	$git_version['minor'],
	$build_num,
	$git_version['seq']
))
 
Write-Host "##teamcity[buildNumber '$version']"