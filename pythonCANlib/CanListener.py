import signal
import sys
from canlib import canlib, Frame
from websocket import create_connection


def send_start_signal(ch):
    frame = Frame(id_=0, data=[1, 0], dlc=2, flags=0)
    ch.write(frame)


def dump_message(id, msg, dlc, flag, time):
    if flag & canlib.canMSG_ERROR_FRAME != 0:
        print("***ERROR FRAME RECEIVED***")
    else:
        dataStr = ""
        msgLen = len(msg)

        for i in range(0, 8):
            if i < msgLen:
                dataStr += "{}".format(msg[i])
                if i != (msgLen - 1):
                    dataStr += "."

        ws.send("{0}:{1}".format(id, dataStr))


def dump_message_loop(ch):
    finished = False
    while not finished:
        try:
            id, msg, dlc, flag, time = ch.read(50)
            hasMessage = True

            while hasMessage:
                if 386 <= id <= 393 and id != 390:
                    dump_message(id, msg, dlc, flag, time)
                try:
                    id, msg, dlc, flag, time = ch.read()
                except canlib.canNoMsg as ex:
                    hasMessage = False
                except canlib.canError as ex:
                    print(ex)
                    finished = True
        except canlib.canNoMsg as ex:
            None
        except canlib.canError as ex:
            print(ex)
            finished = True


def exit_gracefully(signum, frame):
    ws.close()
    ch.busOff()
    ch.close()
    sys.exit()


if __name__ == '__main__':
    signal.signal(signal.SIGINT, exit_gracefully)
    signal.signal(signal.SIGTERM, exit_gracefully)

    # Initialization
    channel = 0
    ip = "ws://localhost:8765"

    if len(sys.argv) >= 2:
        channel = int(sys.argv[1])
        if len(sys.argv) == 3:
            ip = "ws://" + sys.argv[2]

    try:
        ch = canlib.openChannel(channel, canlib.canOPEN_ACCEPT_VIRTUAL)
        ws = create_connection(ip)
    except canlib.exceptions.CanNotFound as ex:
        print(ex)
        sys.exit()

    ch.setBusParams(canlib.canBITRATE_250K)
    ch.busOn()

    # Set node to operational state
    send_start_signal(ch)

    # Start listening for messages
    dump_message_loop(ch)

    # Channel teardown
    ch.busOff()
    ch.close()
