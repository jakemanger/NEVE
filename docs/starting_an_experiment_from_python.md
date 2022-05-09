 Starting an experiment from python

If you are not a fan of GUIs or are doing some developement. You can use NEVE with python.


#### Install python and dependencies

If you are unfamiliar with python and python virtual environments, see https://towardsdatascience.com/getting-started-with-python-virtual-environments-252a6bd2240

1. Install python 3.6 or greater, following installation instructions at [https://www.python.org/](https://www.python.org/).

* For the special case when you want to access the GUI and are using MacOS, you
will have to use your system installation of python and cannot use a python virtual
environment (as `wxpython` requires a Framework build of python to function), so skip
to step 4 and swap out `python` and `pip` for your main installation of python 3, e.g. 
`python3` and `pip3` in all steps. *

2. Create a virtual environment in the NEVE_python directory

```bash
python3 -m venv venv
```

3. Activate your virtual environment

(on mac or linux)

```bash
source venv/bin/activate
```

(on windows)

```
venv\Scripts\Activate
```

4. Install dependencies

```bash
pip install -r requirements.txt
```

### Setup

Pick a desired experiment to use. In this example, we will use an optomotor experiment.

Make desired changes to the experiment's configuration file in the `NEVE_python/configs` directory.

*For an optomotor experiment, we could change the grating density in the
`configs/optomotor.yaml` file to be 800 in the first trial and 50 in the second
trial with speeds of 5 and 10 degrees per second, like so*:

```python
... LINE 48
# stimuli
density: [800, 50] # CHANGED FROM [400, 200]
offset: 0
angle: 0
speed: [5, 10] # CHANGED FROM 5 (FOR ALL TRIALS)
square: 0
minimumVal: [0, 0]
maximumVal: [0.1, 0.5]
...
```

*Note: The value you supply to each parameter should have a length equal to your number of trials
or a single value to indicate it is fixed for all trials. For example, if you wanted two trials 
with different density parameters but the same square parameter, supply an array the size of your
number of trials `density: [400, 200]` and a single value `square: 0`.*

### Run

Ensure you have an activated virtual environment (Install step 3 above).

Start the stimulus, specifying the configuration file to use:

```
python control_simulation.py --ignore-gooey ./configs/optomotor.yaml
```

and follow the prompts. You should see control-related messages in the terminal and the
stimulus displayed on your designated screen (specified by your config file).
Note, the `--ignore-gooey` flag removes the GUI from the program. Exclude this
from the command if you want to see a GUI.

Expected terminal output:

![Expected output from a successful setup](Docs/successful_setup.png)

Expected stimulus with `./configs/optomotor.yaml`

![Optomotor experiment](Docs/optomotor_experiment.gif)


Logs from each trial in the experiment (parameters of stimuli and timing of frames) will 
be continuously written and saved in the directory of the experiment i.e.
`NEVE_python/trial_logs` as a csv file.

To view the frame rate reported from unity,
look at the difference in time (column t) in the csv output. Other data may also be present,
such as the timing of when a flash on the sync square was made (with a press of the F key)
or the position of a moving stimulus.


