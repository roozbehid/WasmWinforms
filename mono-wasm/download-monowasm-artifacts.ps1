if (!(Test-Path "incoming\remove-to-redownload.txt")){
    Remove-Item .\incoming -Force -Recurse
    $progressPreference = 'silentlyContinue'
    [Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12
    $build_id = "1452"
    $build_server ="ubuntu-1804-amd64"
    $build_hash ="90c80dd026d"
    $main_url = "https://jenkins.mono-project.com/job/test-mono-mainline-wasm/label=$build_server/$build_id/Azure/processDownloadRequest/$build_id/$build_server/sdks/wasm/mono-wasm-$build_hash.zip"
    Invoke-RestMethod -Method GET -Uri $main_url -OutFile "mono-wasm.zip" 
    Expand-Archive -Path mono-wasm.zip -DestinationPath incoming -Force
    New-Item -path incoming\remove-to-redownload.txt -type file
    Remove-Item -Path mono-wasm.zip
}