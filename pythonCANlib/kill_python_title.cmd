@echo off
if "%~1"=="" exit
set "TITLE=%~1"
for /f "tokens=1,2,* delims=," %%a in ('tasklist /fi "imagename eq python.exe" /v /fo csv ^| find "%TITLE%"') do (
    if "%%~a"=="python.exe" taskkill /pid "%%~b"
)
exit
