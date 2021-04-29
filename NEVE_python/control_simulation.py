from mlagents_envs.environment import UnityEnvironment
from mlagents_envs.side_channel.environment_parameters_channel import EnvironmentParametersChannel
from mlagents_envs.side_channel.engine_configuration_channel import EngineConfigurationChannel

from hyperiid_manual_control_paras import paras

# type in the full path to the executable file
# or set to None if testing in the unity editor
file_name = "/Users/jakemanger/phd_projects/NEVE/NEVE_unity_URP/Builds/HyperiidManualControlArena.app"
# file_name = None

# Create the side channels
env_parameters = EnvironmentParametersChannel()
eng_config = EngineConfigurationChannel()

print('Waiting for connection to Unity environment...')
env = UnityEnvironment(file_name=file_name, side_channels=[env_parameters, eng_config])

num_exps = len(paras[list(paras)[0]])
print('Running for', num_exps, 'experiments')
for i in range(num_exps):
    input("Press Enter to change parameters and reset...")
    print('Setting new environmental parameters')
    for key, value in paras.items():
        print('Setting', key)
        env_parameters.set_float_parameter(key, value[i])

    # Any change to a SideChannel (env_parameters) will only be effective after a step or reset
    print('Starting experiment...')
    env.reset()

env.close()