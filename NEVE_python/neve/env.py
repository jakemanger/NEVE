from mlagents_envs.environment import UnityEnvironment
from mlagents_envs.side_channel.environment_parameters_channel import (
    EnvironmentParametersChannel
)
from mlagents_envs.side_channel.engine_configuration_channel import (
    EngineConfigurationChannel
)
import yaml
import time
from yaml.loader import SafeLoader
import os
from sys import platform
import shutil


class Nenv:
    """A NEVE environment to control a unity environment.

    A user-friendly wrapper around the UnityEnvironment class.
    """

    def __init__(self, params):
        """Initialize the NEVE environment.

        Args:
            params (str): The path to the configuration file with parameters of the
            Unity stimulus/environment.
        """

        if not os.path.exists(params):
            raise Exception('Could not find the configuration file')

        with open(params, 'r') as f:
            self.params = yaml.load(f, Loader=SafeLoader)

        # run python setup code
        if 'python_setup' in self.params:
            exec(self.params['python_setup'])

        # loop through the parameters and find any that are python code
        # and evaluate them
        for key, value in self.params.items():
            python_code_prefix = 'python:'
            if type(value) == str and value.startswith(python_code_prefix):
                try:
                    self.params[key] = eval(
                        str.strip(value[len(python_code_prefix):]),
                    )
                except Exception as e:
                    raise Exception(
                        'Could not evaluate the python code for '
                        f'parameter: {key}. The error was: '
                        f'{e}'
                    )

        # Create the side channels to communicate with unity
        self.env_parameters = EnvironmentParametersChannel()
        self.eng_config = EngineConfigurationChannel()

        self.config_log_path = './config_logs/'
        if not os.path.exists(self.config_log_path):
            os.makedirs(self.config_log_path)

        # copy file saved at params to config_log_path
        self.stripped_name = os.path.basename(params)
        shutil.copyfile(
            params,
            f'{self.config_log_path}'
            f'{self.stripped_name}_started_at_{time.time()}.yaml'
        )

        with open(
            f'{self.config_log_path}'
            f'evaluated_{self.stripped_name}_started_at_{time.time()}.yaml',
            'w'
        ) as f:
            yaml.dump(self.params, f)

        print('Waiting for connection to Unity environment...')
        print('When Unity launches, python will gain control.')

        file_name = self._get_built_file_path()
        self.env = UnityEnvironment(
            file_name=file_name,
            side_channels=[self.env_parameters, self.eng_config],
            timeout_wait=999999,
            worker_id=0 if file_name is None else self._get_worker_id()
        )
        self.execution_order = self.params['execution_order']
        self.params.pop('execution_order')
        self.params.pop('python_setup')

    def set_params(self, i, dark_adapt=False):
        """Sets the parameters for the experimental trial

        Any change using set_params requires a reset (self.reset()).

        Args:
            i (int): The index of the trial.
        """

        if dark_adapt:
            self.params['darkAdaptNow'] = 1.0
        else:
            self.params['darkAdaptNow'] = 0.0

        # Any change to a Unity SideChannel (self.env_parameters) will
        # only be effective after a step or reset
        # so self.reset() will need to be called to apply the changes.
        print(f'Setting new environmental parameters for condition {i}...')
        if dark_adapt:
            self.env_parameters.set_float_parameter('darkAdaptNow', 1)

        used_params = {}

        for key, value in self.params.items():
            if key == 'experimentDuration' and dark_adapt:
                value = self.params['darkAdaptTime']

            if type(value) != list and type(value) != range:
                val = value
            elif len(value) == 1:
                val = value[0]
            else:
                val = value[i]

            # ensure that the value is a float
            val = float(val)

            print('Setting', key, 'to', val)
            used_params[key] = val

            self.env_parameters.set_float_parameter(key, val)

        # save the parameters used for this trial to a new yaml file
        # with the current time
        with open(
            f'{self.config_log_path}{self.stripped_name}_params'
            f'_used_at_{time.time()}.yaml',
            'w'
        ) as f:
            yaml.dump(used_params, f)

    def reset(self):
        """Resets the environment

        Should be called after self.set_params().
        """
        print('Resetting environment/Starting experiment...')
        print(
            'Press EXIT to end the Unity experiment prematurely.'
            ' Press ALT-TAB to access python.'
        )
        self.env.reset()

    def close(self):
        """Ends and closes the environment"""
        print('All experiments completed. Closing unity and exiting...')
        self.env.close()

    def _get_built_file_path(self):
        """Returns the path to the built Unity executable"""
        file_name = None
        if str(self.params['buildDir']).lower() not in (
            '', 'none', 'null', 'na', 'nan'
        ):
            build_dir = self.params['buildDir']

            if not os.path.exists(build_dir):
                build_dir = os.path.join(os.path.dirname(__file__), '..',
                                         self.params['buildDir'])
                if not os.path.exists(build_dir):
                    build_dir = os.path.join(os.path.dirname(__file__), '..',
                                             '..', self.params['buildDir'])

                    if not os.path.isdir(build_dir):
                        # running as a mac app and need to go up 5 or 6 levels
                        build_dir = os.path.join(
                            os.path.dirname(__file__),
                            '..', '..', '..', '..', self.params['buildDir']
                        )
                        if not os.path.isdir(build_dir):
                            build_dir = os.path.join(
                                os.path.dirname(__file__),
                                '..', '..', '..', '..', '..',
                                self.params['buildDir'])
                            raise FileNotFoundError(
                                f'Could not find build file at {build_dir}'
                                'Please check the buildDir parameter in the'
                                ' config file.'
                            )

            if platform in ['win32', 'cygwin']:
                file_name = os.path.join(
                    build_dir,
                    'Windows/NEVE_unity_urp.exe'
                )
            elif platform in ['darwin']:
                file_name = os.path.join(build_dir, 'Mac.app')
            elif platform in ['linux', 'linux2']:
                file_name = os.path.join(build_dir, 'Linux/Linux.x86_64')
            else:
                raise Exception(
                    f'Build file for platform {platform} has not been made.'
                )
        else:
            print(
                'No buildDir provided. Assuming you are'
                ' running your experiment in Unity for development.'
                ' Press play in the Unity Editor to start.'
            )

        # now remove buildDir from params, as we should not send it to unity
        self.params.pop('buildDir')

        return file_name

    def _get_worker_id(self, filename=".worker_id.dat"):
        """ Workaround for ml-agents socket connection communicator problem

        This changes the worker id if there is one left over from an old unity
        environment that didn't close its socket connection correctly.

        See https://github.com/Unity-Technologies/ml-agents/issues/1505
        """

        with open(filename, 'a+') as f:
            f.seek(0)
            val = int(f.read() or 0) + 1
            f.seek(0)
            f.truncate()
            f.write(str(val))
            return val
