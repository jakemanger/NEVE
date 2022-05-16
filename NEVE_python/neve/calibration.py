import serial
from .env import Nenv
import time
from os import path, mkdir
import csv


class CalManager:
    def __init__(
        self,
        port: str = '/dev/ttyUSB0',
        baudrate: int = 115200,
        bytesize: int = 8,
        timeout: int = 2,
        stopbits=serial.STOPBITS_ONE,
        parity=serial.PARITY_NONE,
        calibration_file_outpath: str = None,
        data_encoding: str = 'utf-8',
        n_measurements=10,
        time_between_measurements=5
    ):
        """ A manager for calibrating stimulus parameters with a radiometer or similar device.

        The device must be accessible via a serial port and have the correct port and baudrate.
        """

        self.calibration_file_outpath = calibration_file_outpath

        parent_dir = path.dirname(self.calibration_file_outpath)
        if not path.exists(parent_dir):
            mkdir(parent_dir)

        self.serial_port = serial.Serial(
            port=port,
            baudrate=baudrate,
            bytesize=bytesize,
            timeout=timeout,
            stopbits=stopbits,
            parity=parity
        )
        print(f'Connected to {self.serial_port.portstr}')

        self.data_encoding = data_encoding

        self.n_measurements = n_measurements
        self.time_between_measurements = time_between_measurements


    def start_calibration(self, env_config_path: str):
        """ Starts the calibration procedure """

        initial_wait_time = 5 
        print(f'Waiting for {initial_wait_time} seconds for the buffer to fill from the serial port')
        time.sleep(initial_wait_time)

        nenv = Nenv(params=env_config_path)

        # write something to the serial port
        # self.serial_port.write(b'1')

        time_waited = 0
        print('Running for', len(nenv.execution_order), 'experimental conditions')
        for i in nenv.execution_order:
            nenv.set_params(i)
            nenv.reset()

            for ii in range(self.n_measurements):
                data = self.read_calibration_data()
                time_waited = 0

                self.write_calibration_data(data, i, ii)

                time.sleep(max(self.time_between_measurements - time_waited, 0))

        nenv.close()

        self.plot_results()

    def read_calibration_data(self):
        """ Gets the latest line from the serial port """
        # return b'000'

        while self.serial_port.inWaiting() > 0:
            status = self.serial_port.readline()

        return status

    def write_calibration_data(self, data: str, stimulus_condition: int, rep: int):
        """ Writes the calibration data to a csv file, line by line """
        if self.data_encoding is not None:
            data = data.decode(self.data_encoding)

        file_exists = path.exists(self.calibration_file_outpath)

        print(f'stimulus condition {stimulus_condition}, rep {rep} recorded: {data}')
        with open(self.calibration_file_outpath, 'a') as f:
            writer = csv.writer(f)
            if not file_exists:
                # write headers
                writer.writerow(['stimulus_condition', 'rep', 'data'])
            writer.writerow([stimulus_condition, rep, data])

