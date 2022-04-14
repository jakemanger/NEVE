from neve.calibration import CalManager
import datetime

# set outpath as the current datetime
datetime_info = datetime.datetime.now().strftime("%Y%m%d_%H%M%S")

cm = CalManager(
    port="/dev/ttyUSB0",
    calibration_file_outpath=(
        f'./calibration_file_{datetime_info}.csv'
    )
)

cm.start_calibration(env_config_path='./configs/calibration.yaml')
