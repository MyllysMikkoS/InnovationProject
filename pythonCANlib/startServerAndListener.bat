:: argument 1: ip 192.168.43.161:8765
start "webSocketServer" python webSocketServer.py 192.168.43.161:8765
:: argument 1: channel 0, argument 2: ip 192.168.43.161:8765
start "canListener" python CanListener.py 0 192.168.43.161:8765
