$sage300Path = (Get-ItemProperty -Path 'HKLM:\SOFTWARE\Wow6432Node\Accpac International, Inc.\ACCPAC\Configuration').Programs
$source = $sage300Path + '\Online\Web\bin\Sage.CA.SBS.ERP.Sage300.ExportReport.exe'
$destination = (Get-Location).path
Copy-Item -Path $source -Destination $destination