# Below are the parameters used to alter a unity experiment with the name
# OptomotorArena.exe
# All parameter values should be a list (denoted by []) and should have the SAME
# length (i.e. one should not have 1, while others have 2).
# e.g. [1, 2] has the parameter value 1 for the first experiment/experimental condition and 2 for the second.
# Each list value corresponds to the values of a different experiment or experimental condition.
# These should be differentiated by the frameDataIdCode, so you can link up the saved data that unity spits out
# at the end of your experiment.
# change these values to whatever you need and ensure that you keep a seperate
# copy of these parameter files for each experiment you run, so you don't lose
# track of what frameDataIdCode corresponds to what experimental parameters.

# Controls for the experiment
# escape - stall Unity, end the experiment and give control to python.
# F - flicker the stimulus and the sync square to sync unity with the camera
# tab - Unlock/lock the cursor from Unity allowing you to see or not see your mouse 

paras = {
    'frameDataIdCode': [900001, 900002],  # a id code representing the experiment id. used to identify which frame save data is for what experiment.
    'recordFrameData': [1, 1],  # 0 = false, 1 = true. record frame and stimulus related data?
    'recordEachFrame': [1, 1],  # 0 = false, 1 = true. record data each frame. If false, then uses the recording frequency
    'recordingFrequency': [1, 1],  # only used if recordFrameData=1 and recordEachFrame=0. time in seconds to record stimulus data
    'manualControl': [0, 0],  # 0 = false, 1 = true. Give manual control to the user? if so, follow control the guide at the top of this script
    'mouseMoveSpeed': [2, 2],  # move speed of mouse if manual_control=1
    'flickerDuration': [0.1, 0.1],  # duration of flicker of sync square and stimulus when pressing f and at start of experiment
    'syncSquareColorR': [1, 1],
    'syncSquareColorG': [0, 0],
    'syncSquareColorB': [0, 0],
    'syncSquareColorA': [1, 1],

    'eyeHeight': [2, 2],  # height of the animals eyes relative to the bottom PIXEL of the front/side monitors - used for calculating perspective (cm). This is always in the center of the monitors.
    'distanceToMonitors': [7, 7],  # distance from center of eye to all monitors (cm)
    'monitorDimensionsX': [12.176, 12.176],  # x dimensions of monitors (cm)
    'monitorDimensionsY': [6.87, 6.87],  # y dimensions of monitors (cm)
    'stimulusDuration': [99999, 99999],  # total duration of the experiment (seconds). after this time (or if escape is pressed), unity will stall and give control back to python
    'frontDisplayNum': [0, 0],  # 0 = first connected display, 1 = second, and so on
    'rightDisplayNum': [1, 1],
    'backDisplayNum': [2, 2],
    'leftDisplayNum': [3, 3],

    'density': [50, 100],  # number of vertical black or white bars around horizon (360 degrees)
    'offset': [0, 0],  # offset from angle 0 at start
    'angle': [0, 0],  # changes the angle of the bars. DOESNT change the north and south poles (that still needs to be implemented)
    'speed': [5, 10],  # speed of rotation (negative speeds cause opposite direction of rotation)
    'square': [0, 0]  # whether to make a square wave instead of a sine wave
}