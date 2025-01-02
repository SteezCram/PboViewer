$directory = [System.IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
dotnet publish -c Release -o $directory/publish/pboviewer-gui-windows-x64 --self-contained -r win-x64
dotnet publish -c Release -o $directory/publish/pboviewer-gui-windows-arm64 --self-contained -r win-arm64
dotnet publish -c Release -o $directory/publish/pboviewer-gui-macos-x64 --self-contained -r osx-x64
dotnet publish -c Release -o $directory/publish/pboviewer-gui-macos-arm64 --self-contained -r osx-arm64
dotnet publish -c Release -o $directory/publish/pboviewer-gui-linux-x64 --self-contained -r linux-x64
dotnet publish -c Release -o $directory/publish/pboviewer-gui-linux-arm64 --self-contained -r linux-arm64