function ValidateDependencies() {
    CheckPowershellVersion
}

function CheckPowershellVersion() {

    $psVersion = $PSVersionTable.PSVersion

    if($psVersion.Major -lt 4){
        Write-Error "This script requires PowerShell version 4.0.0 or higher. Please install 'Windows Management Framework 4.0' or higher."
        Exit -1
    }
}
