# Build a new NEVE executable

*tested using python3.9*

If you are on MacOS, ensure `python3` is your system installation of python, as
the GUI framework will complain that you are not using a "Framework" version of
python. If you are on another Operating System, ensure `python3` is your
virtual environment, see [here]() for how to use a virtual environment.

To build a new executable, change directory to the `NEVE_python` directory and
run the following command:

```
python3 -m nuitka --standalone --enable-plugin=numpy --macos-create-app-bundle --assume-yes-for-downloads --remove-output --onefile control_simulation.py
```

This will save the executable in the `NEVE_python` directory. Move it into the
base directory (`NEVE`), and change its name appropriately, e.g. `NEVE_mac` for
MacOS. Double click it to ensure it runs correctly.

See https://nuitka.net/doc/user-manual.html for more information on building
executables with python and debugging.
