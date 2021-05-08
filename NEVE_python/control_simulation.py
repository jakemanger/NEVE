from mlagents_envs.environment import UnityEnvironment
from mlagents_envs.side_channel.environment_parameters_channel import EnvironmentParametersChannel
from mlagents_envs.side_channel.engine_configuration_channel import EngineConfigurationChannel

# type in the full path to the executable file
# or set to None if testing in the unity editor
# file_name = None

# optomotor experiment
# from optomotor_paras import paras, execution_order
# file_name = "Builds/OptomotorArena/NEVE_unity_URP.exe"

# hyperiid looming stimulus/ static stimulus experiment
# from hyperiid_looming_stimulus_paras import paras, execution_order
# file_name = "Builds/HyperiidLoomingStimulusArena/NEVE_unity_URP.exe"

# hyperiid dual stimulus experiment
from hyperiid_dual_stimulus_paras import paras, execution_order
file_name = "/Users/jakemanger/phd_projects/NEVE/NEVE_python/Builds/HyperiidManualControlArena.app"

# fiddler crab looming stimulus/ static stimulus experiment
# from fiddlercrab_looming_stimulus_paras import paras, execution_order
# file_name = "Builds/FiddlerCrabLoomingStimulusArena/NEVE_unity_URP.exe"

# Create the side channels to communicate with unity
env_parameters = EnvironmentParametersChannel()
eng_config = EngineConfigurationChannel()

print('Waiting for connection to Unity environment...')
print('When Unity launches, python will gain control.')
env = UnityEnvironment(file_name=file_name, side_channels=[env_parameters, eng_config], timeout_wait=999999)

num_exps = len(paras[list(paras)[0]])
print('Running for', num_exps, 'experiments')
for i in execution_order:
    input('Press Enter to change parameters and start experiment ' + str(paras['frameDataIdCode'][i]))
    print('Setting new environmental parameters...')
    for key, value in paras.items():
        print('Setting', key, '...')
        env_parameters.set_float_parameter(key, value[i])

    # Any change to a SideChannel (env_parameters) will only be effective after a step or reset
    print('Starting experiment...')
    print('Press exit to end the Unity experiment prematurely. Press ALT-TAB to access python.')
    env.reset()

print('All experiments completed. Closing unity and exiting...')
env.close()
        
