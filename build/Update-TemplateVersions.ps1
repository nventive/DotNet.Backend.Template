Write-Host Version: $env:GitVersion_NuGetVersionV2

$buildPropsFiles = Get-ChildItem -Rec | ? { $_.Name -eq "Directory.Build.props" } | %{ $_.FullName }

foreach ($buildPropsFile in $buildPropsFiles) {
  [xml]$buildProps = Get-Content $buildPropsFile
  Write-Host Updating $buildPropsFile
  $buildProps.Project.PropertyGroup.TemplateVersion = $env:GitVersion_NuGetVersionV2
  $buildProps.Save($buildPropsFile)
}
