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
  % { if($_.LicenseUrl -like "https://github.com/*") { $_.LicenseUrl = $_.LicenseUrl + "?raw=true" } $_; } |
  % { $_ | Add-Member -MemberType NoteProperty -Name "License" -Value @(@(Invoke-WebRequest -Uri $_.LicenseUrl -UseBasicParsing).Content | % {$_ -replace '<[^>]+>',''} | % {$_ -replace '(?m)(.*)^[\r\n]+([\r\n]+.*)','$1$2'} | % {$_ -replace '&nbsp;',' '} | % {$_ -replace '&quot;','"'} | % {$_ -replace '&#39;',''''}) -PassThru } |
  Select @{N='Package';E={$_.Id}}, @{N='Version';E={$_.Version}}, @{N='LicenseUrl';E={$_.LicenseUrl}}, @{N='License';E={$_.License}} |
  Format-List Package, Version, LicenseUrl, License