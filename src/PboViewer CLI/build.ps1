$directory = [System.IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
dotnet publish -c Release -o $directory/publish/pboviewer-cli-windows-x64 --self-contained -r win-x64
dotnet publish -c Release -o $directory/publish/pboviewer-cli-windows-arm64 --self-contained -r win-arm64
dotnet publish -c Release -o $directory/publish/pboviewer-cli-macos-x64 --self-contained -r osx-x64
dotnet publish -c Release -o $directory/publish/pboviewer-cli-macos-arm64 --self-contained -r osx-arm64
dotnet publish -c Release -o $directory/publish/pboviewer-cli-linux-x64 --self-contained -r linux-x64
dotnet publish -c Release -o $directory/publish/pboviewer-cli-linux-arm64 --self-contained -r linux-arm64