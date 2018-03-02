$appid = "`"$($args[3])`""
$installPath = if($args[0] -eq $null) { ".\" } else { $args[0] }
$user = $args[1]
$certificatePassword = $args[2]

Import-Module -Force $installPath\PowerShellModules\DependencyValidator.psm1 -Verbose
ValidateDependencies
Import-Module -Force $installPath\PowerShellModules\AppConfigReader.psm1 -Verbose
$environment = $env:Environment

Write-Host "Discovered environment: $environment"
Write-Host "Install folder set to: $installPath"

$configFilePath = ResolveConfigPath $environment;
$appConfig = ReadConfig $configFilePath;
#$urlNamespaces = ReadUrlNamespacesFromConfig $appConfig;
#$certificates = ReadCertificatesFromConfig $appConfig;

#Write-Host "Importing certificates and creating port reservations for SSL"

#foreach($cert in $certificates)
#{
#    $filePath = Join-Path $installPath $cert.file -Resolve
#    $cmd = "cmd.exe /C certutil.exe -f -p $certificatePassword -importpfx `"$($filePath)`""
#    Write-Host "Invoking command " $cmd.replace($certificatePassword, "***********")

#	try
#	{
#	$Error.Clear()
#	 Invoke-Expression -Command $cmd
#	}
#	catch 
#	{
#		$Error | format-list -force
#		Write-Host -foreground red $Error[0].Exception;
#	}

#    foreach($ip in $cert.ipport)
#    {
#        $cmd = "cmd.exe /C netsh.exe http add sslcert ipport=$($ip) certhash=$($cert.hash) appid=$appid"
#        Write-Host "Invoking command " $cmd
#        Invoke-Expression -Command $cmd
#    }
#}

#Write-Host "Removing Certificates folder"

#if (Test-Path "$installPath\Certificates")
#{
#    Remove-Item -path "$installPath\Certificates" -recurse
#}

#Write-Host "Creating port reservations"

#foreach($url in $urlNamespaces)
#{
#    $cmd = "cmd.exe /C netsh.exe http add urlacl url=$url user=`"$user`""
#    Write-Host "Invoking command " $cmd
#    Invoke-Expression -Command $cmd
#}

Exit 0