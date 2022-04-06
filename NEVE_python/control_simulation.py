from neve.env import Nenv

config_path = 'configs/looming_stimulus.yaml'
# config_path = 'configs/optomotor.yaml'
# config_path = 'configs/dual_moving_stimulus.yaml'
# config_path = 'configs/moving_rectangle_stimulus.yaml'
# config_path = '' # for testing with unity

nenv = Nenv(params=config_path)

print('Running for', nenv.execution_order, 'experimental conditions')
for i in len(nenv.execution_order):
    nenv.set_params(i)
    nenv.reset()

nenv.close()
