
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
# IF displayStimulusCode == True, then F just fades displayStimulusCode from off to on or on to off
# tab - Unlock/lock the cursor from Unity allowing you to see or not see your mouse 
# Space - start movement/reset looming of the stimulus
# IF manualControl==1,
# Move the mouse - Move the stimulus in polar coordinates around the center of the aquariums
# Scroll the mouse wheel - Make the stimulus larger or smaller
# 0 - recenter stimulus to the center of the front screen (polar coordinates: 0, 0).

import numpy

display_order = [1, 0, 2, 3] # (left to right according to unity)

paras = {
    # saving, control and syncing
    'frameDataIdCode': [1, 2, 3, 4, 5, 6, 7, 8, 9],  # a id code representing the experiment id. used to identify which frame save data is for what experiment.
    'experimentDuration': [99999, 99999, 99999, 99999, 99999, 99999, 99999, 99999, 99999],  # total duration of the experiment (seconds). after this time (or if escape is pressed), unity will stall and give control back to python
    'recordFrameData': [1, 1, 1, 1, 1, 1, 1, 1, 1],  # 0 = false, 1 = true. record frame and stimulus related data?
    'recordEachFrame': [1, 1, 1, 1, 1, 1, 1, 1, 1],  # 0 = false, 1 = true. record data each frame. If false, then uses the recording frequency
    'recordingFrequency': [1, 1, 1, 1, 1, 1, 1, 1, 1],  # only used if recordFrameData=1 and recordEachFrame=0. time in seconds to record stimulus data
    'manualControl': [0, 0, 0, 0, 0, 0, 0, 0, 0],  # 0 = false, 1 = true. Give manual control to the user? if so, follow control the guide at the top of this script
    'mouseMoveSpeed': [2, 2, 2, 2, 2, 2, 2, 2, 2],  # move speed of mouse if manual_control=1
    'flickerDuration': [0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1],  # duration of flicker of sync square and stimulus when pressing f and at start of experiment
    'syncSquareColorR': [1, 1, 1, 1, 1, 1, 1, 1, 1],
    'syncSquareColorG': [0, 0, 0, 0, 0, 0, 0, 0, 0],
    'syncSquareColorB': [0, 0, 0, 0, 0, 0, 0, 0, 0],
    'syncSquareColorA': [1, 1, 1, 1, 1, 1, 1, 1, 1],
    'syncSquareDisplayNum': [0],  # 0 = first connected display, 1 = second, and so on
    'displayStimulusCode': [1, 1, 1, 1, 1, 1, 1, 1, 1],  # display the stimulus code at all times on the sync square? (in white) 0 = false, 1 = true

    # perspective
    'eyeHeight': [2.864],  # height of the animals eyes relative to the bottom PIXEL of the front/side monitors - used for calculating perspective (cm). This is always in the center of the monitors.
    'distanceToMonitors': [27, 27, 27, 27, 27, 27, 27, 27, 27],  # distance from center of eye to all monitors (cm)
    'monitorDimensionsX': [52, 52, 52, 52, 52, 52, 52, 52, 52],  # x dimensions of monitors (cm)
    'monitorDimensionsY': [32, 32, 32, 32, 32, 32, 32, 32, 32],  # y dimensions of monitors (cm)
    # WARNING unity considers display numbers from left to right
    'frontDisplayNum': [1, 1, 1, 1, 1, 1, 1, 1, 1],  # 0 = first connected display, 1 = second, and so on
    'rightDisplayNum': [0, 0, 0, 0, 0, 0, 0, 0, 0],  # front refers to front camera, right refers to right and so on
    'backDisplayNum': [2, 2, 2, 2, 2, 2, 2, 2, 2],
    'leftDisplayNum': [3, 3, 3, 3, 3, 3, 3, 3, 3],

    # stimuli
    'aboveHorizonColourR': [0.5],
    'aboveHorizonColourG': [0.5],
    'aboveHorizonColourB': [0.5],
    'aboveHorizonColourA': [1],
    'belowHorizonColourR': [0.3],
    'belowHorizonColourG': [0.3],
    'belowHorizonColourB': [0.3],
    'belowHorizonColourA': [1],
    'horizonHeight': [0], # degrees
    
    'startScaleX': [3, 3, 3, 2.12, 3, 1000, 3, 3, 3], # only vary scale if you are not translating or moving the object
    'startScaleY': [3, 3, 3, 2.12, 1000, 3, 3, 3, 3],
    'startScaleZ': [3, 2.7, 0.01, 0.01, 0.01, 0.01, 0.01, 0.01, 0.01], #make something small 0.01 to avoid distortion as a cube
    'endScaleX': [3, 2.7, 3, 2.12, 3, 1000, 3, 3, 3],
    'endScaleY': [3, 2.7, 3, 2.12, 1000, 3, 3, 3, 3],
    'endScaleZ': [3, 2.7, 0.01, 0.01, 0.01, 0.01, 0.01, 0.01, 0.01],  #make something small 0.01 to avoid distortion as a cube
    # WARNING polar positions are in rotation axes (so the opposite of what you would intuitively think)  x = up down, y = left right
    'stimulusPolarPositionX': [-15, -15, -15, -15, -15, -15, -18, -15, -15],  # stimulus starting x position in polar coordinates in degrees (-90 to 90) relative to animal eye position (treatment 7 is raised to match 8)
    'stimulusPolarPositionY': [0, 0, 0, 0, 0, 0, 0, 0, 0],  # stimulus starting y position in polar coordinates in degrees (-180 to 180) relative to animal eye position
    'targetLocationOffsetX': [0, 0, 0, 0, 0, 0, 0, 0, 1.5],  # x offset for the target of a looming stimulus (cm) relative to animal eye position
    'targetLocationOffsetY': [0, 0, 0, 0, 0, 0, 0, 1.5, 0],  # y offset for the target of a looming stimulus (cm) relative to animal eye position
    'targetLocationOffsetZ': [0, 0, 0, 0, 0, 0, 0, 0, 0],  # z offset for the target of a looming stimulus (cm) relative to animal eye position
    'startOffset': [500],  # start distance of object from eye of animal (cm)
    'endOffset': [1.5, 1.5, 3.1, 3.1, 1.5, 1.5, 3.1, 3.1, 3.1],  # end distance of object from eye of animal after looming (cm)
    'duration': [24.925, 24.925, 24.845, 24.845, 24.925, 24.925, 24.845, 24.845, 24.845],  # duration of movement based off distance/velocity
    'delayToApproach': [5, 5, 5, 5, 5, 5, 5, 5, 5],  # delay in seconds before stimulus starts looming (if manual_control=false)
    'stimulusColourR': [0],  # values from 0 to 1 (anything less or greater than these extremes will be rounded to 0 or 1, respectively)
    'stimulusColourG': [0],
    'stimulusColourB': [0],
    'stimulusColourA': [1, 0, 1, 1, 1, 1, 1, 1, 1],
    'drawOutline': [0, 1, 0, 0, 0, 0, 0, 0, 0],
    'outlineWidth': [0, 0.111111111, 0, 0, 0, 0, 0, 0, 0],
    'outlineColourR': [0],
    'outlineColourG': [0],
    'outlineColourB': [0],
    'outlineColourA': [1],
    'stimulusType': [0, 0, 1, 1, 1, 1, 1, 1, 1], # 0 = sphere, 1 = cube
    'darkAdaptTime': [0],
    'fixedAngularSize': [0, 0, 0, 0, 1, 1, 0, 0, 0], # 0 = no fixing of angular size, 1 = you want to keep angular size on a axis constant
    'fixXAxis': [0, 0, 0, 0, 0, 1, 0, 0, 0], # 0 = Y axis, 1 = X axis
    'minAngularAngle': [0, 0, 0, 0, -21.35966024, -6.35966024, 0, 0, 0],
    'maxAngularAngle': [0, 0, 0, 0, -8.64033976, 6.35966024, 0, 0, 0]
}

# parameters for you to change for your randomisation between crabs
execution_order = [7, 4]  # 0 = first, 1 = second

display_with_stimulus = [2, 1]  #only 1(right) and 2 (Back)


# do not change this below
paras['frontDisplayNum'][execution_order[0]] = numpy.roll(display_order, -display_with_stimulus[0])[0]
paras['rightDisplayNum'][execution_order[0]] = numpy.roll(display_order, -display_with_stimulus[0])[1]
paras['backDisplayNum'][execution_order[0]] = numpy.roll(display_order, -display_with_stimulus[0])[2]
paras['leftDisplayNum'][execution_order[0]] = numpy.roll(display_order, -display_with_stimulus[0])[3]

paras['frontDisplayNum'][execution_order[1]] = numpy.roll(display_order, -display_with_stimulus[1])[0]
paras['rightDisplayNum'][execution_order[1]] = numpy.roll(display_order, -display_with_stimulus[1])[1]
paras['backDisplayNum'][execution_order[1]] = numpy.roll(display_order, -display_with_stimulus[1])[2]
paras['leftDisplayNum'][execution_order[1]] = numpy.roll(display_order, -display_with_stimulus[1])[3]

# error checking
assert len(execution_order) == len(set(execution_order)), 'STOP! display_with_stimulus will not be correct. To have different displays with the same stimulus parameters, create a new stimulus condition'
