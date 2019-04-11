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

    def generate(self, file_name, interval, ids):
        self.__parse_text_file_to_data(file_name, ids)

        if len(self.angleData) > 20:
            self.__connect_to_web_socket()
            if self.webSocket is not None:
                self.__dump_message_loop(interval)

    def __parse_text_file_to_data(self, file_name, ids):
        with open(file_name, 'r') as openFile:
            for line in openFile:
                temp = line[11:-18]
                if temp.startswith(ids):
                    temp = temp.split(' ')
                    temp = [x for x in temp if x]
                    del temp[1]
                    temp = ':'.join('.'.join(temp).split('.', 1))
                    self.angleData.append(temp)

    def __connect_to_web_socket(self):
        try:
            self.webSocket = create_connection('ws://localhost:8765')
        except Exception as ex:
            print(ex)

    def __dump_message_loop(self, interval):
        pool = cycle(self.angleData)
        for msg in pool:
            if self.webSocket is not None:
                data_id, data = msg.split(':')
                self.webSocket.send("{0}:{1}".format(data_id, data))
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

    data_source_file = 'angleData.txt'
    # ids_to_read = ('386', '388', '389', '392', '393')
    ids_to_read = '392'
    message_interval = 0.1

    generator = AngleDataGenerator()
    generator.generate(data_source_file, message_interval, ids_to_read)
    generator.stop()
