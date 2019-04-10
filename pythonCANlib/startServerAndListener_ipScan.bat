@echo off
for /f "tokens=1-2 delims=:" %%a in ('ipconfig^|find "IPv4"') do set ip==%%b
set ipAddress=%ip:~2%

start "webSocketServer" python webSocketServer.py %ipAddress%:8765
start "canListener" python CanListener.py 0 %ipAddress%:8765