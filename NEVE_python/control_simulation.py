from neve.env import Nenv
import argparse
from gooey import Gooey, GooeyParser
import os
import sys


# for nuitka exe
target = None
if "__compiled__" in globals():
    target = sys.argv[0]


def start(config_paths):
    """ Main function for looping through all experimental conditions

    Experimental conditions are specified by the `config_paths` argument
    """
    print(config_paths)
    for i, config_path in enumerate(config_paths):
        print(f'Starting {config_path}')

        if i == 0:
            # first initialise and connect to unity (starting it)
            nenv = Nenv(params=config_path)
        else:
            # then just initialise parameters and keep existing connection to unity
            nenv.__init__(params=config_path, connect_to_unity=False)

        # add an extra dark adaptation condition if "darkAdaptFirstTrialOnly" is in
        # the config file
        if 'darkAdaptTime' in nenv.params and nenv.params['darkAdaptTime'] > 0:
            nenv.set_params(0, dark_adapt=True)
            nenv.reset()

        print('Running for', len(nenv.execution_order), 'experimental conditions')
        for i in nenv.execution_order:
            nenv.set_params(i)
            nenv.reset()

        if 'darkAdaptTime' in nenv.params and nenv.params['darkAdaptTime'] > 0:
            nenv.set_params(0, dark_adapt=True)
            nenv.reset()

    # finally close unity
    nenv.close()


def determine_configs_dir():
    """
    Determine the directory where the configuration files are stored.
    Tries different relative paths to find the existing 'configs' directory.
    """

    # Start by checking the directory of the script file (__file__)
    base_dir = os.path.dirname(__file__)

    # List of potential relative paths where the 'configs' directory might be located
    potential_paths = [
        'configs',                  # Directly in the script's directory
        os.path.join('..', 'configs'),             # One level up
        os.path.join('..', '..', '..', 'configs'), # Three levels up (common in a Mac app)
        os.path.join('..', '..', '..', '..', 'configs') # Four levels up
    ]

    # Check each path and return the first one that exists
    for path in potential_paths:
        config_dir = os.path.join(base_dir, path)
        if os.path.isdir(config_dir):
            return config_dir

    # If no valid path is found, raise an error
    raise FileNotFoundError("Could not find the 'configs' directory. Please ensure it exists.")


def ordinal(n):
    ''' cunction to convert an integer to its ordinal representation '''
    return "%d%s" % (n, "tsnrhtdd"[(n//10%10!=1)*(n%10<4)*n%10::4])


@Gooey(target=target, program_name='NEVE stimulus controller', use_cmd_args=True)
def main():
    parser = GooeyParser()
    
    # maximum number of config files you want to allow
    max_configs = 6

    configs_dir = determine_configs_dir()

    # create optional file chooser for each config
    for i in range(max_configs):
        parser.add_argument(
            f'--config_path_{i + 1}',  # use named argument instead of positional
            metavar=f'Config File {i + 1}',
            widget='FileChooser',
            help=f'Select your {ordinal(i + 1)} stimulus configuration file (optional)',
            gooey_options={
                'wildcard': "Configuration files (*.cfg;*.json;*.yaml)|*.cfg;*.json;*.yaml|All files (*.*)|*.*",
                'full_width': True,
                'default_dir': configs_dir  # set the default directory here
            },
            required=False
        )

    args = parser.parse_args()

    # collect all non-empty config paths
    config_paths = [getattr(args, f'config_path_{i + 1}') for i in range(max_configs) if getattr(args, f'config_path_{i + 1}', None)]

    start(config_paths)

if __name__ == '__main__':
    main()
