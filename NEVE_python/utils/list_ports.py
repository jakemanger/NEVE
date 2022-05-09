import serial.tools.list_ports

def main():
    ports = serial.tools.list_ports.comports()

    for port, desc, hwid in sorted(ports):
        print("{}: {} [{}]".format(port, desc, hwid))

if __name__ == '__main__':
    main()