@echo off

:: Get the parent path where the binaries are installed
FOR %%I in ("%~dp0.") DO FOR %%J in ("%%~dpI.") DO SET mypath=%%~dpnxJ
FOR %%I in ("%mypath%") DO FOR %%J in ("%%~dpI.") DO SET mypath=%%~dpnxJ
echo %mypath%

:: Register the shell extension dll
%SystemRoot%\Microsoft.NET\Framework64\v4.0.30319\RegAsm.exe "%mypath%\PboViewer_ShellExtension.dll" /codebase