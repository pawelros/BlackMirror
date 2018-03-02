$exitCode = (Start-Process -FilePath msiexec.exe -ArgumentList /i, MSI_FILENAME, /qn, /L*v, MSI_FILENAME.log -Wait -Passthru).ExitCode
$log = (Get-Content MSI_FILENAME.log) -join "`n"
Write-Host $log
exit($exitCode)
