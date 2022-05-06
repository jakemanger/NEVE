from neve.env import Nenv
import argparse
from gooey import Gooey, GooeyParser
import os


def start(config_path):
    """ Main function for looping through all experimental conditions

    Experimental conditions are specified by the `config_path` argument
    """
    nenv = Nenv(params=config_path)

    print('Running for', len(nenv.execution_order), 'experimental conditions')
    for i in nenv.execution_order:
        nenv.set_params(i)
        nenv.reset()

    nenv.close()


@Gooey(program_name='NEVE stimulus controller', use_cmd_args=True)
def main():
    parser = GooeyParser(
        description='''
        Start Unity and loop through all stimuli specified by a
        configuration file at `config_path`.
        '''
    )

    # find possible config files in the configs/ directory
    configs_dir = 'configs' if os.path.exists('configs') else 'NEVE_run/configs'

    if os.path.exists(configs_dir):
        config_files = [
            os.path.join(configs_dir, f) for f in os.listdir(configs_dir)
        ]
    else:
        print(
            'Could not find the configs directory'
            'Store your config files in the configs/ directory'
        )

    parser.add_argument(
        'config_path',
        type=str,
        help='''
        Select your stimulus configuration file.
        ''',
        choices=config_files
    )
    args = parser.parse_args()

    start(args.config_path)

if __name__ == '__main__':
    main()
