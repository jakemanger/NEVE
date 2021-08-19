# Quick start

1. Install python 3.6 or greater

2. Create a virtual environment in this directory
```bash
python3 -m venv venv
```

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

5. Edit your parameter file (e.g. `fiddlercrab_looming_stimulus_arena.py`)

6. Make sure this parameter file is imported in `control_simulation.py`

7. Start your simulation
```bash
python control_simulation.py
```