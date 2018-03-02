$installPath = if($args[0] -eq $null) { ".\" } else { $args[0] }

Import-Module –Force $installPath\PowerShellModules\DependencyValidator.psm1
ValidateDependencies
Import-Module –Force $installPath\PowerShellModules\AppConfigReader.psm1

$environment = $env:Environment

Write-Host "Discovered environment: $environment"
Write-Host "Install folder set to: $installPath"

$configFilePath = ResolveConfigPath;
$appConfig = ReadConfig $configFilePath;
#$urlNamespaces = ReadUrlNamespacesFromConfig $appConfig;
#$certificates = ReadCertificatesFromConfig $appConfig;

#if ($envConfig)
#{
#  Write-Host "Deleting certificates and port reservations for SSL"

#  foreach($cert in $certificates)
#  {
#      $cmd = "cmd.exe /C certutil.exe -delstore my $($cert.hash)"
#      Write-Host "Invoking command " $cmd
#      Invoke-Expression -Command $cmd

#        foreach($ip in $cert.ipport)
#        {
#            $cmd = "cmd.exe /C netsh.exe http delete sslcert ipport=$($ip)"
#            Write-Host "Invoking command " $cmd
#            Invoke-Expression -Command: $cmd
#        }
#  }

#  foreach($url in $urlNamespaces)
#  {
#    $cmd = "cmd.exe /C netsh.exe http del urlacl url=$url"
#    Write-Host "Invoking command " $cmd
#    Invoke-Expression -Command $cmd
#  }
#}

if (Test-Path $installPath)
{
  Remove-Item -path "$installPath" -recurse
}

Exit 0