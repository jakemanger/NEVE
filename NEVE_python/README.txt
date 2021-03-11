## Quick start

1. Open up your simulation's environment in Unity
2. cd into the crab-unityagents directory
3. start your python virtual environment with `source crab-ua-env/bin/activate`
you should now see the virtual environments name on the left of the screen
i.e. `(crab-ua-env) $`.
4. Train!
There are two main options for training: 1) use unity's pre-built mlagents-learn and 2) use a custom gym environment.
Option 1) to train the agent using unity's mlagents-learn, use `mlagents-learn config/rollerball_config.yaml --run-id=RollerBall`. Now follow instructions from the command line.

Option 2) to train the agent using gym