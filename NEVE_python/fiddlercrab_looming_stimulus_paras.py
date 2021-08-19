
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

# an example with three stimuli: red, green, blue

paras = {
    # saving, control and syncing
    'frameDataIdCode': [11, 12, 13],  # a id code representing the experiment id. used to identify which frame save data is for what experiment.
    'animalCode': [1, 2, 3],  # a id code representing the animal id.
    'experimentDuration': [99999],  # total duration of the experiment (seconds). after this time (or if escape is pressed), unity will stall and give control back to python
    'recordFrameData': [1],  # 0 = false, 1 = true. record frame and stimulus related data?
    'recordEachFrame': [1],  # 0 = false, 1 = true. record data each frame. If false, then uses the recording frequency
    'recordingFrequency': [1],  # only used if recordFrameData=1 and recordEachFrame=0. time in seconds to record stimulus data
    'manualControl': [0],  # 0 = false, 1 = true. Give manual control to the user? if so, follow control the guide at the top of this script
    'mouseMoveSpeed': [2],  # move speed of mouse if manual_control=1
    'flickerDuration': [0.1],  # duration of flicker of sync square and stimulus when pressing f and at start of experiment
    'syncSquareColorR': [1],
    'syncSquareColorG': [0],
    'syncSquareColorB': [0],
    'syncSquareColorA': [1],
    'syncSquareDisplayNum': [0],  # 0 = first connected display, 1 = second, and so on
    'displayStimulusCode': [1],  # display the stimulus code at all times on the sync square? (in white) 0 = false, 1 = true

    # perspective
    'eyeHeight': [2.864],  # height of the animals eyes relative to the bottom PIXEL of the front/side monitors - used for calculating perspective (cm). This is always in the center of the monitors.
    'distanceToMonitors': [27],  # distance from center of eye to all monitors (cm)
    'monitorDimensionsX': [52],  # x dimensions of monitors (cm)
    'monitorDimensionsY': [32],  # y dimensions of monitors (cm)
    # WARNING unity considers display numbers from left to right
    # if using only one display, then make sure 0 is the front display num and the others are 1 or another number
    'frontDisplayNum': [0],  # 0 = first connected display, 1 = second, and so on
    'rightDisplayNum': [1],  # front refers to front camera, right refers to right and so on
    'backDisplayNum': [2],
    'leftDisplayNum': [3],

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
    'startScaleX': [3], # only vary scale if you are not translating or moving the object
    'startScaleY': [3],
    'startScaleZ': [3], #make something small 0.01 to avoid distortion as a cube
    'endScaleX': [3],
    'endScaleY': [3],
    'endScaleZ': [3],  #make something small 0.01 to avoid distortion as a cube
    # WARNING polar positions are in rotation axes (so the opposite of what you would intuitively think)  x = up down, y = left right
    'stimulusPolarPositionX': [-15],  # stimulus starting x position in polar coordinates in degrees (-90 to 90) relative to animal eye position (treatment 7 is raised to match 8)
    'stimulusPolarPositionY': [0],  # stimulus starting y position in polar coordinates in degrees (-180 to 180) relative to animal eye position
    'targetLocationOffsetX': [0],  # x offset for the target of a looming stimulus (cm) relative to animal eye position
    'targetLocationOffsetY': [0]  # y offset for the target of a looming stimulus (cm) relative to animal eye position
    'targetLocationOffsetZ': [0],  # z offset for the target of a looming stimulus (cm) relative to animal eye position
    'startOffset': [500],  # start distance of object from eye of animal (cm)
    'endOffset': [1.5],  # end distance of object from eye of animal after looming (cm)
    'duration': [24.925],  # duration of movement based off distance/velocity
    'delayToApproach': [5],  # delay in seconds before stimulus starts looming (if manual_control=false)
    'stimulusColourR': [1, 0, 0],  # values from 0 to 1 (anything less or greater than these extremes will be rounded to 0 or 1, respectively)
    'stimulusColourG': [0, 1, 0],
    'stimulusColourB': [0, 0, 1],
    'stimulusColourA': [1],
    'drawOutline': [0],
    'outlineWidth': [0],
    'outlineColourR': [0],
    'outlineColourG': [0],
    'outlineColourB': [0],
    'outlineColourA': [1],
    'stimulusType': [0], # 0 = sphere, 1 = cube
    'darkAdaptTime': [0],
    'fixedAngularSize': [0], # 0 = no fixing of angular size, 1 = you want to keep angular size on a axis constant
    'fixXAxis': [0], # 0 = Y axis, 1 = X axis
    'minAngularAngle': [0], # the min angular size to fix (if fixedAngularSize == 1)
    'maxAngularAngle': [0]
}

# parameters for you to change for your randomisation between crabs
execution_order = [0, 1, 2]  # 0 = first, 1 = second
