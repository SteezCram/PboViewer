$directory = [System.IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
dotnet publish -c Release -o $directory/publish/win-x64 --self-contained -r win-x64
dotnet publish -c Release -o $directory/publish/win-arm64 --self-contained -r win-arm64
dotnet publish -c Release -o $directory/publish/macos-x64 --self-contained -r osx-x64
dotnet publish -c Release -o $directory/publish/macos-arm64 --self-contained -r osx-arm64
dotnet publish -c Release -o $directory/publish/linux-x64 --self-contained -r linux-x64
dotnet publish -c Release -o $directory/publish/linux-arm64 --self-contained -r linux-arm64