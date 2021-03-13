@echo off

:: Get the current path of the batch
SET mypath=%~dp0
SET mypath=%mypath:~0,-1%
echo %mypath%

cd %mypath%

CALL fileAssociation_install.bat
CALL contextMenu_install.bat