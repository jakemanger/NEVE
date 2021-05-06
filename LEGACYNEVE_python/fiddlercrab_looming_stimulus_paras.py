# Below are the parameters used to alter a unity experiment with the name
# FiddlerCrabLoomingStimulusArena.exe
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
# Space - start movement/reset looming of the stimulus
# IF manualControl==1,
# Move the mouse - Move the stimulus in polar coordinates around the center of the aquariums
# Scroll the mouse wheel - Make the stimulus larger or smaller
# 0 - recenter stimulus to the center of the front screen (polar coordinates: 0, 0).

paras = {
    # saving, control and syncing
    'frameDataIdCode': [200001, 200002],  # a id code representing the experiment id. used to identify which frame save data is for what experiment.
    'experimentDuration': [99999, 99999],  # total duration of the experiment (seconds). after this time (or if escape is pressed), unity will stall and give control back to python
    'recordFrameData': [1, 1],  # 0 = false, 1 = true. record frame and stimulus related data?
    'recordEachFrame': [1, 1],  # 0 = false, 1 = true. record data each frame. If false, then uses the recording frequency
    'recordingFrequency': [1, 1],  # only used if recordFrameData=1 and recordEachFrame=0. time in seconds to record stimulus data
    'manualControl': [1, 1],  # 0 = false, 1 = true. Give manual control to the user? if so, follow control the guide at the top of this script
    'mouseMoveSpeed': [2, 2],  # move speed of mouse if manual_control=1
    'flickerDuration': [0.1, 0.1],  # duration of flicker of sync square and stimulus when pressing f and at start of experiment
    'syncSquareColorR': [1, 1],
    'syncSquareColorG': [0, 0],
    'syncSquareColorB': [0, 0],
    'syncSquareColorA': [1, 1],
    'syncSquareDisplayNum': [1, 1],  # 0 = first connected display, 1 = second, and so on
    'displayStimulusCode': [1, 1],  # display the stimulus code at all times on the sync square? (in white) 0 = false, 1 = true

    # perspective
    'eyeHeight': [2, 2],  # height of the animals eyes relative to the bottom PIXEL of the front/side monitors - used for calculating perspective (cm). This is always in the center of the monitors.
    'distanceToMonitors': [7, 7],  # distance from center of eye to all monitors (cm)
    'monitorDimensionsX': [12.176, 12.176],  # x dimensions of monitors (cm)
    'monitorDimensionsY': [6.87, 6.87],  # y dimensions of monitors (cm)
    'frontDisplayNum': [1, 1],  # 0 = first connected display, 1 = second, and so on
    'rightDisplayNum': [2, 2],
    'backDisplayNum': [4, 4],
    'leftDisplayNum': [3, 3],

    # stimuli
    'aboveHorizonColourR': [0.5, 0.5],
    'aboveHorizonColourG': [0.5, 0.5],
    'aboveHorizonColourB': [0.5, 0.5],
    'aboveHorizonColourA': [1, 1],
    'belowHorizonColourR': [0.3, 0.3],
    'belowHorizonColourG': [0.3, 0.3],
    'belowHorizonColourB': [0.3, 0.3],
    'belowHorizonColourA': [1, 1],
    'horizonHeight': [0, 0], # degrees
    'stimulusSize': [1, 1],  # size of stimulus in cm (edge to edge of sphere)
    # WARNING polar positions are in rotation axes (so the opposite of what you would intuitively think)  x = up down, y = left right
    'stimulusPolarPositionX': [0, 0],  # stimulus starting x position in polar coordinates in degrees (-90 to 90) relative to animal eye position
    'stimulusPolarPositionY': [0, 0],  # stimulus starting y position in polar coordinates in degrees (-180 to 180) relative to animal eye position
    'targetLocationOffsetX': [0, 0],  # x offset for the target of a looming stimulus (cm) relative to animal eye position
    'targetLocationOffsetY': [0, 0],  # y offset for the target of a looming stimulus (cm) relative to animal eye position
    'targetLocationOffsetZ': [0, 0],  # z offset for the target of a looming stimulus (cm) relative to animal eye position
    'startOffset': [50, 50],  # start distance of object from eye of animal (cm)
    'endOffset': [1, 1],  # end distance of object from eye of animal after looming (cm)
    'stimulusMoveSpeed': [5, 2],  # move speed of stimulus when looming (cm/s)
    'delayToApproach': [5, 5],  # delay in seconds before stimulus starts looming (if manual_control=false)
    'stimulusColourR': [0.1, 0.3],  # values from 0 to 1 (anything less or greater than these extremes will be rounded to 0 or 1, respectively)
    'stimulusColourG': [0.1, 0.3],
    'stimulusColourB': [0.1, 0.3],
    'stimulusColourA': [1, 1],
}

execution_order = [0, 1]  # 0 = first, 1 = second