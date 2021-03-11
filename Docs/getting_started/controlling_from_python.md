## Getting started: controlling from python
The below terminal commands assume you are using a mac or linux distribution. If you are using windows, the same process can be followed with cmd or powershell, however, commands may need to be slightly modified due to differences in DOS and bash languages.

### Open your simulation's environment in Unity
1. If you don't have a unity project, follow the [unity setup guide](unity_setup.md)
2. Open Unity Hub
3. Go to the Projects tab
4. Click on your project. If your project is not displayed, click add and open the projects folder.

### Install python 
5. The below steps require that you install python 3. Install this from https://www.python.org/
### Create your project's root folder and python virtual environment
6. Open your terminal and change directory to where you want to create your python project (where you keep all your python files). Python projects are commonly created inside a `projects/` folder. Assuming the projects folder exists and is located in the home directory, change your terminals current working directory with the following command:
```
cd ~/projects
```
7. Create your python project directory and change your directory to this folder. In this example, we will call this project  `animvr_exp`. Assuming your terminal is at your `projects/` folder, use the following two commands:
```
mkdir animvr_exp
cd animvr_exp
```
8. Now that you are in the `animvr_exp` directory, you can create a virtual environment to host your python version and packages locally. This lets you maintain different installations of python and python packages for individual projects, so updating packages in one project does not impact another. In this example, we will use the pip package `virtualenv`, however, a similar process could be followed using [anaconda tools](https://www.anaconda.com/). First install the virtualenv package with pip with the following command.
```
pip install virtualenv
```
9. Now that you have the virtualenv package, create a new python3 virtual environment with:
```
virtualenv venv -p python3
```
10. You can now start up your virtual environment any time you want to run an AnimVR project and control it from python. To start your newly created virtual environment, use the following command:
```
source venv/bin/activate
```
11. If it worked and you are now using your python virtual environment, your terminal window should look something like this:
```
(venv) $ 
```

### Installing dependencies
12. The first time you create your python virtual environment, you will be required to install required python packages. Using pip, install these with the following command:
```
pip install mlagents==0.21.0 
```

### Simple control
13. Now that everything is setup, i.e. our unity environment is open and our python virtual environment is activated, we can start to control the environment from python. Below, we are using the example `FindBurrow` project created in our [Creating a AnimVR unity project]() example. If you are using a different project, you will be required to change any text with `FindBurrow` to that of your project name.
14. To achieve simple control, create a python file named `test_env.py` and add the following code:

```
from mlagents_envs.environment import UnityEnvironment
from mlagents_envs.side_channel.engine_configuration_channel import EngineConfigurationChannel
import matplotlib.pyplot as plt

channel = EngineConfigurationChannel()

print('Waiting for you to press play in the unity editor...')
env = UnityEnvironment(side_channels=[channel])

# set timescale to 1, so the simulation runs at a normal speed
channel.set_configuration_parameters(time_scale = 1.0)

# first reset the environment
env.reset()

# get behaviours (to control) in the environment
behaviours = list(env.behavior_specs)
print('Number of behaviours in the environment:', len(behaviours))
print('Names of behaviours:', behaviours)
behavior_name = behaviours[0]
print('We are only changing behaviour', behavior_name)
spec = env.behavior_specs[behavior_name]

# Examine the number of observations per Agent
print("Number of observations : ", len(spec.observation_shapes))

# Is there a visual observation ?
# Visual observation have 3 dimensions: Height, Width and number of channels
vis_obs = any(len(shape) == 3 for shape in spec.observation_shapes)
obs_type = 'vector'
if vis_obs:
    obs_type = 'image'
print("Observation type:", obs_type)

# Is the Action continuous or multi-discrete ?
if spec.is_action_continuous():
  print("The action space is continuous")
if spec.is_action_discrete():
  print("The action space is discrete")

# How many actions are possible ?
print(f"There are {spec.action_size} possible action(s)")

# For discrete actions only : How many different options does each action has ?
if spec.is_action_discrete():
  for action, branch_size in enumerate(spec.discrete_action_branches):
    print(f"Action number {action} has {branch_size} different options")

print('Starting simulation')
# run the environment for a few episodes
for episode in range(3):
  env.reset()
  decision_steps, terminal_steps = env.get_steps(behavior_name)
  tracked_agent = -1 # -1 indicates not yet tracking
  done = False # For the tracked_agent
  episode_rewards = 0 # For the tracked_agent
  while not done:
    # Track the first agent we see if not tracking 
    # Note : len(decision_steps) = [number of agents that requested a decision]
    if tracked_agent == -1 and len(decision_steps) >= 1:
      tracked_agent = decision_steps.agent_id[0] 

    # Generate an action for all agents
    action = spec.create_random_action(len(decision_steps))
    
    # Set the actions
    env.set_actions(behavior_name, action)

    # Move the simulation forward
    env.step()

    # Get the new simulation results
    decision_steps, terminal_steps = env.get_steps(behavior_name)
    if tracked_agent in decision_steps: # The agent requested a decision
      episode_rewards += decision_steps[tracked_agent].reward
    if tracked_agent in terminal_steps: # The agent terminated its episode
      episode_rewards += terminal_steps[tracked_agent].reward
      done = True
  print(f"Total rewards for episode {episode} is {episode_rewards}")
```
14. This script should first wait for the user to press play in the unity editor, then it will reset the environment and finally run the environment for 3 episodes. To provide inputs to the unity editor, it uses the mlagents `channels` methods and also provides input for the animal via `actions`. The  following lines in the above script are used to create random actions (in this case x and y movement directions) and set them for the agent in the unity editor.
```
    # Generate an action for all agents
    action = spec.create_random_action(len(decision_steps))
    
    # Set the actions
    env.set_actions(behavior_name, action)
```
15. Finally, to control the agent and set parameters in the unity editor using this python script, use the following command:
```
python test_env.py
```


### Training a reinforcement learning agent
You can also use a reinforcement learning agent to control agents in the unity editor. This is the original purpose of the unity editor's mlagents project, which we have used to create controls and manipulate parameters of experiments in AnimVR. There are two main options for training: 1) use unity's pre-built mlagents-learn and 2) use a custom gym environment. See the [mlagents documentation](https://github.com/Unity-Technologies/ml-agents/tree/release_9_docs/docs) for more information. Below, we will demonstrate how to use such a agent with the mlagents-learn package.

To simply swap out the animals input for a pre-made reinforcement learning agent, create a file in a `config/` folder called `findburrow_config.yaml` and paste in the following code:

```
behaviors:
  FindBurrow:
    trainer_type: ppo
    hyperparameters:
      batch_size: 10
      buffer_size: 100
      learning_rate: 3.0e-4
      beta: 5.0e-4
      epsilon: 0.2
      lambd: 0.99
      num_epoch: 3
      learning_rate_schedule: linear
    network_settings:
      normalize: false
      hidden_units: 128
      num_layers: 2
    reward_signals:
      extrinsic:
        gamma: 0.99
        strength: 1.0
    max_steps: 500000
    time_horizon: 64
    summary_freq: 10000

```

This contains information about the environment and hyperparameters used to train the reinforcement learning agent. In this case, we are using the PPO algorithm.

To train the agent using unity's mlagents-learn python package, use the following command: 
```
mlagents-learn findburrow_config.yaml --run-id=FindBurrow
```