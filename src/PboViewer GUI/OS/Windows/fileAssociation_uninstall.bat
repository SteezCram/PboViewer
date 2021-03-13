@echo off


:: Uninstall or not the file association
REG QUERY HKCR\PBOViewer >nul
if %ERRORLEVEL% EQU 0 (
	REG DELETE HKCR\.pbo /f
	REG DELETE HKCR\PBOViewer /f
)