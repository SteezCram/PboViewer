@echo off

:: Get the parent path where the binaries are installed
FOR %%I in ("%~dp0.") DO FOR %%J in ("%%~dpI.") DO SET mypath=%%~dpnxJ
FOR %%I in ("%mypath%") DO FOR %%J in ("%%~dpI.") DO SET mypath=%%~dpnxJ
echo %mypath%

:: Delete PBO Manager registry stuff
REG QUERY HKCR\.pbo >nul
if %ERRORLEVEL% EQU 0 (
	REG DELETE HKCR\.pbo /f
)

REG QUERY HKCR\PBOShellExt.FileContextMenuExt >nul
if %ERRORLEVEL% EQU 0 (
	REG DELETE HKCR\PBOShellExt.FileContextMenuExt /f
)


:: Install or not the file association
REG QUERY HKCR\PBOViewer >nul
if %ERRORLEVEL% EQU 1 (
	REG ADD HKCR\.pbo /t REG_SZ /d PBOViewer
	REG ADD HKCR\PBOViewer /t REG_SZ /d PBOViewer
	REG ADD HKCR\PBOViewer\DefaultIcon /t REG_SZ /d "\"%mypath%\Data\pbo_icon.ico\""
	REG ADD HKCR\PBOViewer\shell\open\command /t REG_SZ /d "\"%mypath%\PboViewer.exe\" \"%%1\""
)