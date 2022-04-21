from neve.env import Nenv
import argparse


def main(config_path):
    """ Main function for looping through all experimental conditions

    Experimental conditions are specified by the `config_path` argument
    """
    nenv = Nenv(params=config_path)

    print('Running for', len(nenv.execution_order), 'experimental conditions')
    for i in nenv.execution_order:
        nenv.set_params(i)
        nenv.reset()

    nenv.close()


if __name__ == '__main__':
    parser = argparse.ArgumentParser(
        description='''
        Start Unity and loop through all stimuli specified by a
        configuration file at `config_path`.
        '''
    )
    parser.add_argument(
        'config_path',
        type=str,
        help='''
        Path to your yaml configuration file. Use "None" to connect to the
        Unity editor during development.
        '''
    )
    args = parser.parse_args()

    main(args.config_path)

