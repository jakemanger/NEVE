import serial
from .env import Nenv

class CalManager:
    def __init__(
        self,
        port:int=1,
        baudrate:int=115200,
        bytesize:int=8,
        timeout:int=2,
        stopbits=serial.STOPBITS_ONE,
        parity=serial.PARITY_NONE,
        calibration_file_outpath:str=None,
    ):
        self.calibration_file_outpath = calibration_file_outpath

        self.serial_port = serial.Serial(
            port=port,
            baudrate=baudrate,
            bytesize=bytesize,
            timeout=timeout,
            stopbits=stopbits,
            parity=parity
        )
        print(f'Connected to {self.serial_port.portstr}')

    def start_calibration(self, env_config_path):
        nenv = Nenv(params=env_config_path)

        # write something to the serial port
        # self.serial_port.write(b'1')

        print('Running for', nenv.execution_order, 'experimental conditions')
        for i in nenv.execution_order:
            nenv.set_params(i)
            nenv.reset()
            data = self.read_calibration_data()
            self.write_calibration_data(data)

        nenv.close()

    def read_calibration_data(self):
        return self.serial_port.readline()

    def write_calibration_data(self, data):
        print(f'Writing the following: {data}')

