# Quick start

### Install

#### Clone the repository

From the terminal or command-line:

```bash
git clone git@github.com:jakemanger/NEVE.git 
```

Or alternatively use [Github desktop](https://desktop.github.com/) to clone the project into your desired folder.



#### Install python and dependencies

For a more in depth explanation, see https://towardsdatascience.com/getting-started-with-python-virtual-environments-252a6bd2240

1. Install python 3.6 or greater, following installation instructions at [https://www.python.org/](https://www.python.org/).

2. Create a virtual environment in the NEVE_run directory

```bash
python3 -m venv venv
```
s
3. Activate your virtual environment
   (on mac/linux)

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

Make desired changes to the experiment's configuration file in the `NEVE_run/configs` directory.

*For an optomotor experiment, we could change the grating density in the
`configs/optomotor.yaml` file to be 800 in the first trial and 50 in the second
trial with different speeds (5 and 10 degrees per second) like so*:

```python
... LINE 48
# stimuli
density: [800, 50] # CHANGED FROM [400, 200]
offset: 0
angle: 0
speed: [5, 10] # CHANGED FROM 5
square: 0
minimumVal: [0, 0]
maximumVal: [0.1, 0.5]
...
```


### Run

Start the stimulus, specifying the configuration file to use:

```
python control_simulation.py ./configs/optomotor.yaml
```

and follow the prompts. You should see control-related messages in the terminal and the stimulus displayed on your designated screen (specified in your config file).

Expected terminal output:
![Expected output from a successful setup](docs/successful_setup.png)

Expected stimulus with `./configs/optomotor.yaml`
![Optomotor experiment](docs/optomotor_exp.gif)

Expected stimulus with `./configs/loom.yaml`
![Loom experiment](docs/loom_exp.gif)

Data from each trial in the experiment (parameters of stimuli and timing of frames) will be continuously written and saved in the directory of the experiment e.g. `NEVE_run/builds/Optomotor/` as a csv file.
To view the frame rate reported from unity, look at the difference in time
(column t) in the csv output. Other data may also be present, such as the
timing of when a flash on the sync square was made (with a press of the F key)
or the position of a moving stimulus.
