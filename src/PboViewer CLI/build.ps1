$directory = [System.IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
dotnet publish -c Release -o $directory/publish/windows --self-contained -r win-x64
dotnet publish -c Release -o $directory/publish/macos --self-contained -r osx-x64
dotnet publish -c Release -o $directory/publish/debian --self-contained -r debian-x64
dotnet publish -c Release -o $directory/publish/ubuntu --self-contained -r ubuntu-x64