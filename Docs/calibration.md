# Calibrating your screens

We have added a simple calibration method that allows
you to use a external sensory (e.g. a photodiode
or radiometer) to get real world values for
your stimuli. Once calibrated, you should then be able to
specify the intensity you would like for each stimulus.

In the below example, we will calibrate a screen's luminance
using a International Light Technologies (ILT) NST light
measurement system ILT1700 research radiometer.

1. Install required drivers to communicate with your device.
For our radiometer, we can 

2. If you did the above step correctly, your computer should
be able to detect the device and provide a com port number.
Check the connection with your device and find it's com port
number with:

```
python list_ports
```

Alternatively, if on Windows, of to Device manager and see
the usb com port number that appears and disapears when
plugging and unplugging the device.

3. Using your port string (e.g. 'COM1' or '/dev/ttyUSB0')
and the number of measurments you want to make/save (e.g. 10),
start calibrating:

```
python calibrate.py COM1 10
```

You should end up with a lookup table of with a number of rows
equal to the number of trials in your ./configs/calibration.yaml
file. The first row is your trial number and the other rows
are the number of measurements saved.
