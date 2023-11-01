# Calibrating your screens


## Intensity measurements

We have added a simple calibration method that allows
you to use a external sensory (e.g. a photodiode
or radiometer) to get real world values for
your stimuli. Once calibrated, you should then be able to
specify the intensity you would like for each stimulus.

In the below example, we will calibrate a screen's luminance
using a International Light Technologies (ILT) NST light
measurement system ILT1700 research radiometer.

1. Setup your python environment by following [this guide](starting_an_experiment_from_python.md)

2. Install required drivers to communicate with your device.

3. If you did the above step correctly, your computer should
be able to detect the device and provide a com port number.
Check the connection with your device and find it's com port
number with:

```
python utils/list_ports.py
```

Alternatively, if on Windows, of to Device manager and see
the usb com port number that appears and disapears when
plugging and unplugging the device.

4. Using your port string (e.g. 'COM1' or '/dev/ttyUSB0')
and the number of measurements you want to make/save (e.g. 10),
start calibrating:

```
python calibrate.py ../configs/calibration.yaml -p /dev/ttyUSB0 -n 10
```
Note, on linux, your user will require additional privileges to access
serial devices at a port.

You should end up with a lookup table. The number of rows are 
equal to the number of trials in your ./configs/calibration.yaml
file multiplied by the number of measurements you specified.


## Using measured lookup table to define stimulus parameters

The simplest way to use the lookup table from the above step is to change the RGB
value in your stimulus config file to be what you desire. That is, if you measured
a particular light intensity you want to stimulate with from the above measurements,
then update the config file to have the same colour parameters to match that light
intensity.


## LUT to modify colours with colour grading

A more advanced colour calibration approach can be used with a LUT (look up texture) file. Modifying the 
image found at `NEVE/LUTS/lut.png` will modify the colours displayed in Unity.

The LUT is a 2D texture of size 1024 x 32. In video games, developers usually will add this texture
along with a screenshot of their game scene in photoshop. They will then apply desired 
effects to their screenshot to get the colour grading they desire and then apply the same 
modifications to the LUT. Loading that LUT with unity will then apply those same visual
changes to all colours that are displayed.

In our case, we likely want colours to be precise according
to our desired experiment conditions (e.g. have a linear or logarithmic ramp in colours).
For this, you will need to run measurements of the output on your screen with all RGB
combinations (using 32 bit colour space). You will then need to calculate how you need
to modify the `NEVE/LUTS/lut.png` file to achieve the desired change in displayed colours.
Note, there are 32 x 32 x 32 colours displayed in the 2d texture to cover the entire 
colour space (imagine an image sequence of depth slices). 
See the below default LUT used by NEVE:
![LUT](../LUTS/lut.png)

This process can be quite complicated. See these resources for more information:
- https://catlikecoding.com/unity/tutorials/custom-srp/color-grading/
- https://docs.unity3d.com/2017.2/Documentation/Manual/PostProcessing-UserLut.html
- https://docs.unity3d.com/540/Documentation/Manual/script-ColorCorrectionLookup.html

