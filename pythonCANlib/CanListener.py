import signal
import sys
import threading
from canlib import canlib, Frame
from websocket import create_connection, WebSocketConnectionClosedException


class CanListener:

    def __init__(self):
        self.canIdWhiteList = None
        self.channel = None
        self.webSocket = None
        self.listenClient = False
        self.clientMsgListener = threading.Thread(target=self.__client_msg_listener)

    def start_listening_channel(self, ch, flags=canlib.canOPEN_ACCEPT_VIRTUAL, bit_rate=canlib.canBITRATE_250K):
        try:
            self.channel = canlib.openChannel(ch, flags)
            self.channel.setBusParams(bit_rate)
            self.channel.busOn()

            # Set node to operational state
            self.__send_start_signal()

            # Start thread for listen client messages
            if self.listenClient:
                self.clientMsgListener.start()

            # Start listening for messages
            self.__dump_message_loop()

        except canlib.exceptions.CanNotFound as ex:
            print(ex)

    def stop(self):
        self.listenClient = False
        self.clientMsgListener.join(5)
        if self.webSocket is not None:
            self.webSocket.close()
        self.channel.busOff()
        self.channel.close()

    def connect_to_web_socket(self, ip_address, listen_client_messages=False):
        try:
            self.webSocket = create_connection(ip_address)
            self.listenClient = listen_client_messages
        except Exception as ex:
            print(ex)

    def set_can_id_white_list(self, can_id_white_list):
        self.canIdWhiteList = can_id_white_list

    def __send_start_signal(self):
        frame = Frame(id_=0, data=[1, 0], dlc=2, flags=0)
        self.channel.write(frame)

    def __send_reset_zero_level_signal(self):
        # COB-id: 0x60A, Byte 1: initiating download, index: 0x2020, sub index: 2
        frame = Frame(id_=1546, data=[int('00101111', 2), 32, 32, 2, 1, 0, 0, 0], dlc=8, flags=0)
        self.channel.write(frame)

    def __dump_message(self, id, msg, dlc, flag, time):
        if flag & canlib.canMSG_ERROR_FRAME != 0:
            print("***ERROR FRAME RECEIVED***")
        else:
            data_str = ""
            msg_len = len(msg)

            for i in range(0, 8):
                if i < msg_len:
                    data_str += "{}".format(msg[i])
                    if i != (msg_len - 1):
                        data_str += "."
            if self.webSocket is not None:
                self.webSocket.send("{0}:{1}".format(id, data_str))
            else:
                print("{0}:{1}".format(id, data_str))

    def __dump_message_loop(self):
        finished = False
        while not finished:
            try:
                id, msg, dlc, flag, time = self.channel.read(50)
                has_message = True

                while has_message:
                    if self.canIdWhiteList is not None:
                        if id in self.canIdWhiteList:
                            self.__dump_message(id, msg, dlc, flag, time)
                    else:
                        self.__dump_message(id, msg, dlc, flag, time)

                    try:
                        id, msg, dlc, flag, time = self.channel.read()
                    except canlib.canNoMsg as ex:
                        has_message = False
                    except canlib.canError as ex:
                        print(ex)
                        finished = True
            except canlib.canNoMsg as ex:
                None
            except canlib.canError as ex:
                print(ex)
                finished = True

    def __client_msg_listener(self):
        while self.listenClient:
            try:
                result = self.webSocket.recv()
                if result == "RZL":
                    self.__send_reset_zero_level_signal()
                    print result
            except WebSocketConnectionClosedException:
                self.listenClient = False


def exit_gracefully(signum, frame):
    canListener.stop()
    sys.exit()


if __name__ == '__main__':
    signal.signal(signal.SIGINT, exit_gracefully)
    signal.signal(signal.SIGTERM, exit_gracefully)

    # Initialization
    channel = 0
    ip = "ws://localhost:8765"
    ids_to_read = [386, 388, 389, 392, 393, 1418]

    if len(sys.argv) >= 2:
        channel = int(sys.argv[1])
        if len(sys.argv) == 3:
            ip = "ws://" + sys.argv[2]

    canListener = CanListener()
    canListener.set_can_id_white_list(ids_to_read)
    canListener.connect_to_web_socket(ip, True)
    canListener.start_listening_channel(channel)
    canListener.stop()
