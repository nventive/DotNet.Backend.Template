# Script Download-NuGetLicenses
<#
    .SYNOPSIS
        Downloads licences for NuGet Packages in the solution
    .NOTES
        Run the script in the Package Manager Console
    .EXAMPLE
        .\Download-NuGetLicenses > ATTRIBUTIONS.txt
#>

@( Get-Project -All | ? { $_.ProjectName } | % { Get-Package -ProjectName $_.ProjectName } ) |
  Sort-Object Id -Unique |
  ? { $_.LicenseUrl } |
  % { $_ | Add-Member -MemberType NoteProperty -Name "Package" -Value $_.Id -PassThru } |
  % { if($_.LicenseUrl -like "https://github.com/*") { $_.LicenseUrl = $_.LicenseUrl + "?raw=true" } $_; } |
  % { $_ | Add-Member -MemberType NoteProperty -Name "License" -Value @(@(Invoke-WebRequest -Uri $_.LicenseUrl -UseBasicParsing).Content | % {$_ -replace '<[^>]+>',''} | % {$_ -replace '(?m)(.*)^[\r\n]+([\r\n]+.*)','$1$2'})-PassThru } |
  Format-List Package, Versions, LicenseUrl, License