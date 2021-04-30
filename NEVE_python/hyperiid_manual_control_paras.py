# Below are the parameters used to alter a unity experiment with the name
# HyperiidManualControlArena.exe
# All parameter values should be a list (denoted by []) and should have the SAME
# length (i.e. one should not have 1, while others have 2).
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
# IF manualControl==1,
# Move the mouse - to move the stimulus in polar coordinates around the center of the aquariums
# Scroll the mouse wheel - Make the stimulus larger or smaller
# Space - start/reset looming of the stimulus

paras = {
    'frameDataIdCode': [900001, 900002],  # a id code representing the experiment id. used to identify which frame save data is for what experiment.
    'recordFrameData': [1, 1], # 0 = false, 1 = true. record frame and stimulus related data?
    'recordEachFrame': [1, 1], # 0 = false, 1 = true. record data each frame. If false, then uses the recording frequency
    'recordingFrequency': [1, 1], # only used if recordFrameData=1 and recordEachFrame=0. time in seconds to record stimulus data
    'manualControl': [1, 1],  # 0 = false, 1 = true. Give manual control to the user? if so, follow control guide at the top of this script
    'mouseMoveSpeed': [2, 2],  # move speed of mouse if manual_control=true
    'flickerDuration': [0.1, 0.1],  # duration of flicker of sync square and stimulus when pressing f and at start of experiment

    'backgroundColourR': [0, 0.1],  # values from 0 to 1 (anything less or greater than these extremes will be rounded to 0 or 1, respectively)
    'backgroundColourG': [0, 0.1],
    'backgroundColourB': [0, 0.1],
    'backgroundColourA': [1, 1],
    'eyeHeight': [2, 2],  # height of the animals eyes relative to the bottom PIXEL of the front/side monitors - used for calculating perspective (cm). This is always in the center of the monitors.
    'distanceToMonitors': [7, 7],  # distance from center of eye to all monitors (cm)
    'monitorDimensionsX': [12.176, 12.176],  # x dimensions of monitors (cm)
    'monitorDimensionsY': [6.87, 6.87],  # y dimensions of monitors (cm)
    'stimulusSize': [1, 1],  # size of stimulus in cm (edge to edge of sphere)
    'stimulusPolarPositionX': [0, 0],  # stimulus starting x position in polar coordinates in degrees (-90 to 90) relative to animal eye position
    'stimulusPolarPositionY': [0, 0],  # stimulus starting y position in polar coordinates in degrees (-180 to 180) relative to animal eye position
    'targetLocationOffsetX': [0, 0],  # x offset for the target of a looming stimulus (cm) relative to animal eye position
    'targetLocationOffsetY': [0, 0],  # y offset for the target of a looming stimulus (cm) relative to animal eye position
    'targetLocationOffsetZ': [0, 0],  # z offset for the target of a looming stimulus (cm) relative to animal eye position
    'startOffset': [50, 50],  # start distance of object from eye of animal (cm)
    'endOffset': [1, 1],  # end distance of object from eye of animal after looming (cm)
    'stimulusMoveSpeed': [1, 1],  # move speed of stimulus when looming (cm/s)
    'delayToApproach': [5, 5],  # delay in seconds before stimulus starts looming (if manual_control=false)
    'stimulusColourR': [0.1, 0.3],  # values from 0 to 1 (anything less or greater than these extremes will be rounded to 0 or 1, respectively)
    'stimulusColourG': [0.1, 0.3],
    'stimulusColourB': [0.1, 0.3],
    'stimulusColourA': [1, 1],
    'stimulusDuration': [99999, 99999],  # total duration of the experiment (seconds). after this time (or if escape is pressed), unity will stall and give control back to python
    'frontDisplayNum': [0, 0],  # 0 = first connected display, 1 = second, and so on
    'rightDisplayNum': [1, 1],
    'backDisplayNum': [2, 2],
    'leftDisplayNum': [3, 3],
}