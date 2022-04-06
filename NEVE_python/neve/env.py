from mlagents_envs.environment import UnityEnvironment
from mlagents_envs.side_channel.environment_parameters_channel import EnvironmentParametersChannel
from mlagents_envs.side_channel.engine_configuration_channel import EngineConfigurationChannel
import yaml
from yaml.loader import SafeLoader


class Nenv:
    """A NEVE environment to control a unity environment.

    Is a user-friendly wrapper around the UnityEnvironment class.
    """

    def __init__(self, params):
        """Initialize the NEVE environment.

        Args:
            params (str): The path to the parameters of the Unity
            experiment/environment.
        """

        with open(params, 'r') as f:
            self.params = yaml.load(f, Loader=SafeLoader)

        # Create the side channels to communicate with unity
        self.env_parameters = EnvironmentParametersChannel()
        self.eng_config = EngineConfigurationChannel()

        print('Waiting for connection to Unity environment...')
        print('When Unity launches, python will gain control.')

        file_name = None
        if str(self.params['fileName']).lower not in (
                '', 'none', 'null', 'na', 'nan'
        ):
            file_name = self.params['fileName']
        
        self.env = UnityEnvironment(
            file_name=file_name,
            side_channels=[self.env_parameters, self.eng_config],
            timeout_wait=999999
        )
        self.execution_order = self.params['execution_order']

    def set_params(self, i):
        """Sets the parameters for the experimental trial

        Any change using set_params requires a reset (self.reset()) to be activated.

        Args:
            i (int): The index of the trial.
        """

        # Any change to a Unity SideChannel (self.env_parameters) will only be effective after a step or reset
        # so self.reset() will need to be called to apply the changes.
        print('Setting new environmental parameters...')
        for key, value in self.params.items():
            print('Setting', key, '...')
            if len(value) == 1:
                self.env_parameters.set_float_parameter(key, value[0])
            else:
                self.env_parameters.set_float_parameter(key, value[i])
                assert len(value) == len(self.execution_order), (
                    f'the length of {key} must be the same as the length of execution_order'
                )
    
    def reset(self):
        """Resets the environment

        Should be called after self.set_params().
        """
        print('Resetting environment/Starting experiment...')
        print('Press EXIT to end the Unity experiment prematurely. Press ALT-TAB to access python.')
        self.env.reset()

    def close(self):
        """Ends and closes the environment"""
        print('All experiments completed. Closing unity and exiting...')
        self.env.close()
