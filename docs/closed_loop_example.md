# Closed loop example

This document provides a brief overview of how to run a closed-loop experiment with NEVE. It 
assumes you have also followed setup instructions at the main [README](../README.md).

This setup involves retrieving movement data from a program called `fictrac` and sending
that to a NEVE environment via a socket. We will use the loom experiment for this example.

Because fictrac requires the use of the command line, you will need to open a terminal
or cmd app on Windows or Linux.

## Setting up fictrac

To setup fictrac, follow installation and build instructions found at https://github.com/rjdmoore/fictrac.

To ensure everything is working, make sure the sample runs without error via:

(on windows)
```
cd fictrac\sample
..\bin\Release\fictrac.exe 
```

(on linux)
```
cd fictrac/sample
../bin/fictrac 
```

Make your own directory with your output from fictrac and logs (inside the fictrac directory in this example)
```
cd ..
mkdir output
cd output
```

Also follow instructions on fictrac's github page to ensure your setup is properly configured, see
 https://github.com/rjdmoore/fictrac#configuration.
Make sure you have created an appropriate config file (called `config.txt`)
with the following parameters:
- `src_fn`: the image source to your camera index (if running live), or video file (if testing).
- `vfov`: the vertical field of view (in degrees) of your camera
- `sock_host`: the destination IP address for socket data output. Set to `127.0.0.1`.
- `sock_port`: the destination port for socket data output. Set to `1111`.

And with correct configuration, after running:
(on windows)
```
..\bin\Release\configGui.exe
```
(on linux)
```
../bin/configGui
```

*Note, I have fixed sock_host and sock_port in this example, as NEVE has been setup to use these for input from fictrac.
If changing these values is required, please create a Github issue and I can add some customisation to this.*


## Start NEVE

Follow instructions on how to run NEVE at the main [README](../README.md).

Ensure you set the `fictracFeedback` parameter to `1` in your configuration file to let NEVE know to use sockets with fictrac.

From the command line, this involves running:

(on windows)
```
NEVE_windows/control_stimulation.exe 
```

(on linux)
```
./NEVE_linux/control_stimulation
```

and then selecting your desired configuration file. In this case, we will use `loom.yaml`.

NEVE will automatically start listening for input from fictrac once information is sent to 
the socket at `127.0.0.1:1111`.

## Start Fictrac

Start fictrac using your config file in your newly created output directory

(on windows)
```
cd fictrac\sample
..\bin\Release\fictrac.exe 
```

(on linux)
```
../bin/fictrac
```

Once fictrac is running, NEVE will be able to read its input.

In the `loom.yaml` example, you should see the animal move as movement information
is provided by fictrac.

