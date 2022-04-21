from neve.calibration import CalManager
import datetime
import argparse


def main(
    config_path: str,
    out_path_prefix:str,
    port: str,
    baudrate: int,
    num_measurements: int,
    time_between_measurements: float
):
    # set outpath as the current datetime
    datetime_info = datetime.datetime.now().strftime("%Y%m%d_%H%M%S")

    cm = CalManager(
        port=port,
        calibration_file_outpath=f'{out_path_prefix}{datetime_info}.csv',
        baudrate=baudrate,
        n_measurements=num_measurements,
        time_between_measurements=time_between_measurements
    )

    cm.start_calibration(env_config_path=config_path)


if __name__ == '__main__':
    parser = argparse.ArgumentParser(
        description='''
        Start a calibration procedure by connecting to a radiometer or another device at a serial port.
        Saves readings of the device to each stimulus trial in a csv file.
        '''
    )
    parser.add_argument(
        'config_path',
        type=str,
        help='''
        Path to your yaml configuration file. Should be something like `configs/calibration.yaml`.
        Use "None" to connect to the Unity editor during development.
        '''
    )
    parser.add_argument(
        '-p',
        '--port',
        dest='port',
        type=str,
        help='''
        The port to connect to your radiometer or other device for calibration. Should be something like
        `/dev/ttyUSB0` on linux.
        ''',
        default='/dev/ttyUSB0'
    )
    parser.add_argument(
        '-b',
        '--baudrate',
        dest='baudrate',
        type=int,
        help='''
        The baudrate to connect to your radiometer or other device for calibration. Should be something like
        `4800`, `9600`, or `115200`.
        ''',
        default='4800'
    )
    parser.add_argument(
        '-n',
        '--num_measurements',
        dest='num_measurements',
        type=int,
        help='''
        The number of measurements to collect for each stimulus condition in the config file at `config_path`.
        ''',
        default='10'
    )
    parser.add_argument(
        '-t',
        '--time_between_measurements',
        dest='time_between_measurements',
        type=float,
        help='''
        The time in seconds to wait to collect each measurement.
        ''',
        default='0.1'
    )
    parser.add_argument(
        '-o',
        '--outpath_prefix',
        dest='out_path_prefix',
        type=str,
        help='''
        The prefix to use when saving the calibration file.
        ''',
        default='./calibrations/calibration_file_'
    )

    args = parser.parse_args()

    main(
        args.config_path,
        args.out_path_prefix,
        args.port,
        args.baudrate,
        args.num_measurements,
        args.time_between_measurements
    )
