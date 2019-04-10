import signal
import sys
from time import sleep
from itertools import cycle
from websocket import create_connection


class AngleDataGenerator:

    def __init__(self):
        self.angleData = []
        self.webSocket = None
        self.ip_address = None

    def generate(self, file_name, interval):
        with open(file_name, 'r') as openFile:
            for line in openFile:
                self.angleData.append(line[:-1])

        if len(self.angleData) > 20:
            self.__connect_to_web_socket()
            if self.webSocket is not None:
                self.__dump_message_loop(interval)

    def __connect_to_web_socket(self):
        try:
            self.webSocket = create_connection('ws://localhost:8765')
        except Exception as ex:
            print(ex)

    def __dump_message_loop(self, interval):
        pool = cycle(self.angleData)
        for data in pool:
            if self.webSocket is not None:
                self.webSocket.send("{0}:{1}".format(392, data))
                sleep(interval)
            else:
                break

    def stop(self):
        if self.webSocket is not None:
            self.webSocket.close()


def exit_gracefully(signum, frame):
    generator.stop()
    sys.exit()


if __name__ == '__main__':
    signal.signal(signal.SIGINT, exit_gracefully)
    signal.signal(signal.SIGTERM, exit_gracefully)

    generator = AngleDataGenerator()
    generator.generate('angleData2.txt', 0.1)
    generator.stop()
