# Build a new NEVE GUI executable

*tested using python3.9*

If you are on MacOS, ensure `python3` is your system installation of python, as
the GUI framework will complain that you are not using a "Framework" version of
python. If you are on another Operating System, ensure `python3` is your
virtual environment, see [here](https://towardsdatascience.com/getting-started-with-python-virtual-environments-252a6bd2240) for how to use a virtual environment.

To build a new executable, change directory to the `NEVE_python` directory 
```
cd NEVE_python/
```

install the required dependencies
```
pip3 install -r requirements.txt
```

and run the following command:

```
python3 -m nuitka --standalone --enable-plugin=numpy --macos-create-app-bundle --assume-yes-for-downloads --remove-output control_simulation.py
```

This will save the executable in the `NEVE_python` directory. Move it into the
base directory (`NEVE`), and change its name appropriately, e.g. `NEVE_mac` for
MacOS. Double click it to ensure it runs correctly.

See https://nuitka.net/doc/user-manual.html for more information on building
executables with python and debugging.

*If you get an error at the last step about no python-config. It may be that you need the python3.9-dev (the
development version of python*
