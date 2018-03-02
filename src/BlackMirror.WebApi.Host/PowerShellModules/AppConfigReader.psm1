$tempDirPath = Join-Path $installPath "temp"

if(Test-Path $tempDirPath) {
    Remove-Item $tempDirPath -recurse
    New-Item $tempDirPath -type directory
}

$fileList = @(
"Consul.dll"
"LazyCache.dll"
"BlackMirror.Configuration.dll"
"LiteGuard.dll"
"Newtonsoft.Json.dll"
"Serilog.dll"
"Serilog.Sinks.Async.dll"
"Serilog.Sinks.ColoredConsole.dll"
"Serilog.Sinks.File.dll"
"Serilog.Sinks.RollingFile.dll"
"Serilog.Sinks.Trace.dll"
"Spg.Configuration.dll"
"Topshelf.dll"
"Topshelf.Serilog.dll");

 foreach($file in $fileList)
{
    $p = $(Join-Path $installPath $file -Resolve)
    Write-Host "copying file: $($p) to: $($tempDirPath)"

    Copy-Item "$($p)" "$($tempDirPath)"
}

try
{
$Error.Clear()
Add-Type -Path $(Join-Path $tempDirPath "BlackMirror.Configuration.dll" -Resolve)
Add-Type -AssemblyName System.Configuration
}
catch 
{
    Write-Host -foreground yellow "LoadException";
    $Error | format-list -force
    Write-Host -foreground red $Error[0].Exception.LoaderExceptions;
}

function ResolveConfigPath($environment) {
    $configPathResolver = New-Object BlackMirror.Configuration.ConfigPathResolver
    $configUri = $configPathResolver.Resolve($environment, $installPath).AbsolutePath
    Write-Host "Resolved config file path is: $($configUri)"

        return $configUri;
}

function ReadConfig($configPath){
    $entries = @{
        appSettings = @{}
        connectionStrings = @{}
        sslCertificates = @()
    }
    $appConfig = New-Object XML
    $appConfig.Load($configPath)

    Write-Host "appSettings"
    foreach($entry in $appConfig.configuration.appSettings.add)
    {
        Write-Host "key: $($entry.key) value: $($entry.value)"

        $entries.appSettings.Add($entry.key, $entry.value)
    }

    Write-Host "connectionStrings"
    foreach($entry in $appConfig.configuration.connectionStrings.add)
    {
        Write-Host "name: $($entry.name) connectionString: $($entry.connectionString)"

        $entries.connectionStrings.Add($entry.name, $entry.connectionString)
    }

    Write-Host "sslCertificates"
    foreach($entry in $appConfig.configuration.sslCertificates.certificate)
    {
        Write-Host "file: $($entry.file) hash: $($entry.hash) ipport: $($entry.ipport)"
        $certObj = @{
            file = $entry.file
            hash = $entry.hash
            ipport = @()
        }
        $separator = ";"
        $option = [System.StringSplitOptions]::RemoveEmptyEntries
        $certObj.ipport = $entry.ipport.Split($separator, $option)

        $entries.sslCertificates += $certObj
    }

    Write-Host $entries

    return $entries;
}

function ReadUrlNamespacesFromConfig($appConfigEntries){
    $entry = $appConfigEntries.appSettings['UrlNamespace'];
    $entries = $entry.split(";");

    return $entries
}

function ReadCertificatesFromConfig($appConfigEntries){
    return $appConfigEntries.sslCertificates;
}

    $path = ResolveConfigPath;
    $appConfig = ReadConfig $path;
    $urlNamespaces = ReadUrlNamespacesFromConfig $appConfig;
    $sslCertificates = ReadCertificatesFromConfig $appConfig;

    foreach($c in $sslCertificates){
        Write-Host "$($c.file) $($c.hash) $($c.ipport)"
    }

export-modulemember -function ResolveConfigPath
export-modulemember -function ReadConfig