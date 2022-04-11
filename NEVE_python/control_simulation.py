from neve.env import Nenv
import sys


def main(config_path):
    nenv = Nenv(params=config_path)

    print('Running for', nenv.execution_order, 'experimental conditions')
    for i in nenv.execution_order:
        nenv.set_params(i)
        nenv.reset()

    nenv.close()


if __name__ == '__main__':
    usage = (
        '''
        Usage:
            python control_simulation.py path/to/config.yaml

        Example:
            python control_simulation.py configs/looming.yaml
        '''
    )

    if len(sys.argv) == 2 and sys.argv[1] not in ('-h', '--help'):
        main(sys.argv[1])
    else:
        print(usage)

