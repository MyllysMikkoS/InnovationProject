start "webSocketServer" python webSocketServer.py 192.168.43.161:8765
start "dataGenerator" python angleDataGenerator.py 192.168.43.161:8765
