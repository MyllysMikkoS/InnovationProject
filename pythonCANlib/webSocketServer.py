import signal
import sys
from SimpleWebSocketServer import SimpleWebSocketServer, WebSocket


clients = []


class CanWebSocketServer(WebSocket):

    def handleMessage(self):
        for client in clients:
            if client != self:
                client.sendMessage(self.data)

    def handleConnected(self):
        print(self.address, 'connected')
        clients.append(self)

    def handleClose(self):
        clients.remove(self)
        print(self.address, 'closed')


def exit_gracefully(signum, frame):
    server.close()
    sys.exit()


if __name__ == '__main__':
    signal.signal(signal.SIGINT, exit_gracefully)
    signal.signal(signal.SIGTERM, exit_gracefully)

    host = ''
    port = 8765

    if len(sys.argv) >= 2:	
        host, port = sys.argv[1].split(":")
        port = int(port)

    server = SimpleWebSocketServer(host, port, CanWebSocketServer)
    server.serveforever()
