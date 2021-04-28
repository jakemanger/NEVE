from mlagents_envs.environment import UnityEnvironment
from mlagents_envs.side_channel.environment_parameters_channel import EnvironmentParametersChannel
from mlagents_envs.side_channel.engine_configuration_channel import EngineConfigurationChannel

# type in the full path to the executable file
# or set to None if testing in the unity editor
# file_name = "/Users/jakemanger/phd_projects/NEVE/NEVE_unity_URP/Builds/FiddlerCrabArena.app"
# file_name = "/Users/jakemanger/phd_projects/NEVE/NEVE_unity_URP/Builds/FiddlerCrabArena2.app"
file_name = None

# Create the side channels
env_parameters = EnvironmentParametersChannel()
eng_config = EngineConfigurationChannel()

print('Waiting for connection to Unity environment...')
env = UnityEnvironment(file_name=file_name, side_channels=[env_parameters, eng_config])

exitSimulation = False

extra = 0.1

while exitSimulation is False:
    # set experiment parameters
    env_parameters.set_float_parameter("aboveHorizonColorR", extra)
    env_parameters.set_float_parameter("aboveHorizonColorG", extra)
    env_parameters.set_float_parameter("aboveHorizonColorB", extra)
    env_parameters.set_float_parameter("aboveHorizonColorA", 1)

    # Any change to a SideChannel (env_parameters) will only be effective after a step or reset
    print('reset')
    env.reset()

    input("Press Enter to change parameters and reset...")

    extra += 0.1

env.close()