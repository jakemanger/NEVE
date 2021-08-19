# Below are the parameters used to alter a unity experiment with the name
# HyperiidManualControlArena.exe
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
# Space - start movement/reset the position of the stimulus
# IF manualControl==1,
# Move the mouse - Move the stimulus in polar coordinates around the center of the aquariums
# Scroll the mouse wheel - Make the stimulus larger or smaller
# 0 - recenter stimulus to the center of the front screen (polar coordinates: 0, 0).

paras = {
    # saving, control and syncing
    'frameDataIdCode': [800001, 800002],  # a id code representing the experiment id. used to identify which frame save data is for what experiment.
    'animalCode': [1, 2],  # a id code representing the animal
    'experimentDuration': [99999, 99999],  # total duration of the experiment (seconds). after this time (or if escape is pressed), unity will stall and give control back to python
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
    'syncSquareDisplayNum': [1, 1],  # 0 = first connected display, 1 = second, and so on
    'displayStimulusCode': [0, 0],  # display the stimulus code at all times on the sync square? (in white) 0 = false, 1 = true

    # perspective
    'eyeHeight': [3.435, 3.435],  # height of the animals eyes relative to the bottom PIXEL of the front/side monitors - used for calculating perspective (cm). This is always in the center of the monitors.
    'distanceToMonitors': [7, 7],  # distance from center of eye to all monitors (cm)
    'monitorDimensionsX': [12.176, 12.176],  # x dimensions of monitors (cm)
    'monitorDimensionsY': [6.87, 6.87],  # y dimensions of monitors (cm)
    # WARNING unity considers display numbers from left to right
    'frontDisplayNum': [1, 1],  # 0 = first connected display, 1 = second, and so on
    'rightDisplayNum': [0, 0],  # front refers to front camera, right refers to right and so on
    'backDisplayNum': [3, 3],
    'leftDisplayNum': [4, 4],

    # stimuli
    'frontBackgroundColourR': [0, 0.1],  # values from 0 to 1 (anything less or greater than these extremes will be rounded to 0 or 1, respectively)
    'frontBackgroundColourG': [0, 0.1],
    'frontBackgroundColourB': [0, 0.1],
    'frontBackgroundColourA': [1, 1],
    'rightBackgroundColourR': [0, 0.1],  # values from 0 to 1 (anything less or greater than these extremes will be rounded to 0 or 1, respectively)
    'rightBackgroundColourG': [0, 0.1],
    'rightBackgroundColourB': [0, 0.1],
    'rightBackgroundColourA': [1, 1],
    'backBackgroundColourR': [0, 0.1],  # values from 0 to 1 (anything less or greater than these extremes will be rounded to 0 or 1, respectively)
    'backBackgroundColourG': [0, 0.1],
    'backBackgroundColourB': [0, 0.1],
    'backBackgroundColourA': [1, 1],
    'leftBackgroundColourR': [0, 0.1],  # values from 0 to 1 (anything less or greater than these extremes will be rounded to 0 or 1, respectively)
    'leftBackgroundColourG': [0, 0.1],
    'leftBackgroundColourB': [0, 0.1],
    'leftBackgroundColourA': [1, 1],
    # sphere 1
    'stimulusSize1': [2, 1],  # size of stimulus in cm (edge to edge of sphere)
    'stimulusDuration1': [5, 5],  # duration of stimulus movement (from the start to end position. NOT a whole back and forwards loop)
    # WARNING polar positions are in rotation axes (so the opposite of what you would intuitively think) x = up down, y = left right
    'startPolarPositionX1': [0, 0],  # stimulus starting x ROTATION in polar coordinates in degrees (-90 to 90) relative to animal eye position
    'startPolarPositionY1': [-30, -20],  # stimulus starting y ROTATION in polar coordinates in degrees (-180 to 180) relative to animal eye position
    'endPolarPositionX1': [0, 0],  # stimulus ending x ROTATION in polar coordinates in degrees (-90 to 90) relative to animal eye position
    'endPolarPositionY1': [30, 20],  # stimulus ending y ROTATION in polar coordinates in degrees (-180 to 180) relative to animal eye position
    'targetLocationOffsetX1': [0, 0],  # x offset for the target of a looming stimulus (cm) relative to animal eye position
    'targetLocationOffsetY1': [0, 0],  # y offset for the target of a looming stimulus (cm) relative to animal eye position
    'targetLocationOffsetZ1': [0, 0],  # z offset for the target of a looming stimulus (cm) relative to animal eye position
    'startOffset1': [50, 50],  # start distance of object from eye of animal (cm)
    'endOffset1': [50, 50],  # end distance of object from eye of animal after looming (cm)
    'numReps1': [1, 2],  # number of full back and fourth movements of stimulus (0.5 = start to end, 1 = start to end to start, 2 = start to end to start to end to start)
    'delayToApproach1': [5, 5],  # delay in seconds before stimulus starts moving (if manual_control=false)
    'stimulusColourR1': [0.1, 0.3],  # values from 0 to 1 (anything less or greater than these extremes will be rounded to 0 or 1, respectively)
    'stimulusColourG1': [0.1, 0.3],
    'stimulusColourB1': [0.1, 0.3],
    'stimulusColourA1': [1, 1],
    # sphere 2
    'stimulusSize2': [1, 2],  # size of stimulus in cm (edge to edge of sphere)
    'stimulusDuration2': [5, 5],  # duration of stimulus movement (from the start to end position. NOT a whole back and forwards loop)
    # WARNING polar positions are in rotation axes (so the opposite of what you would intuitively think)  x = up down, y = left right
    'startPolarPositionX2': [0, 0],  # stimulus starting x position in polar coordinates in degrees (-90 to 90) relative to animal eye position
    'startPolarPositionY2': [150, 160],  # stimulus starting y position in polar coordinates in degrees (-180 to 180) relative to animal eye position
    'endPolarPositionX2': [0, 0],  # stimulus ending x position in polar coordinates in degrees (-90 to 90) relative to animal eye position
    'endPolarPositionY2': [210, 200],  # stimulus ending y position in polar coordinates in degrees (-180 to 180) relative to animal eye position
    'targetLocationOffsetX2': [0, 0],  # x offset for the target of a looming stimulus (cm) relative to animal eye position
    'targetLocationOffsetY2': [0, 0],  # y offset for the target of a looming stimulus (cm) relative to animal eye position
    'targetLocationOffsetZ2': [0, 0],  # z offset for the target of a looming stimulus (cm) relative to animal eye position
    'startOffset2': [50, 50],  # start distance of object from eye of animal (cm)
    'endOffset2': [50, 50],  # end distance of object from eye of animal after looming (cm)
    'numReps2': [1, 2],  # number of full back and fourth movements of stimulus (0.5 = start to end, 1 = start to end to start, 2 = start to end to start to end to start)
    'delayToApproach2': [5, 5],  # delay in seconds before stimulus starts moving (if manual_control=false)
    'stimulusColourR2': [0.2, 0.3],  # values from 0 to 1 (anything less or greater than these extremes will be rounded to 0 or 1, respectively)
    'stimulusColourG2': [0.2, 0.3],
    'stimulusColourB2': [0.2, 0.3],
    'stimulusColourA2': [1, 1],
}

execution_order = [0, 1]  # 0 = first, 1 = second
