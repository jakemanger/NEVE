# Configs guide

This document provides an explanation of NEVE's configuration parameters. See Generic parameters (parameters used in every stimulus) and specific parameters for your experiment, below.

## Generic 
*Parameters used in every stimulus*
| Parameter name | Description | Options | Default value |
| -------------- | ----------- | ------- | ------------- |
| `buildDir` | The local path to the directory containing the Unity executable. Set this to `None` if you want to test inside the Unity Editor (note, the Unity Editor must be running with the relevant experiment Playing for this to work). | Any directory found in `./builds` | *Stimulus dependent* e.g., `./builds/Loom/` |
| `frameDataIdCode` | The experiment ID used when saving log files and displayed on the syncSquare | Any integer | `1` |
| `animalCode` | The animal code used when saving log files and displayed on the syncSquare | Any integer | `1` |
| `experimentDuration` | The duration of each trial in an experiment in seconds. After this time (or if escape is pressed), unity will stall and give control back to python. | `0`-`99999` | `99999` |
| `recordFrameData` | Whether to record frame data or not. | `0` (false) or `1` (true) | `0` |
| `recordEachFrame` | Whether to record each frame or not. If false, then uses the `recordingFrequency`. | `0` (false) or `1` (true) | `1` |
| `recordingFrequency` | The frequency at which to record frame data in seconds. Only used if `recordFrameData: 0` and `recordEachFrame: 0`. | `0`-`99999` | `1` |
| `manualControl` | Whether to allow manual mouse control of the experiment or not. | `0` (false) or `1` (true) | `0` |
| `mouseMoveSpeed` | The speed at which the mouse moves objects in the experiment. | `0`-`99999` | `1` |
| `flickerDuration` | The duration of a flicker on the sync square in seconds when the `f` key is pressed. | `0`-`99999` | `0.1` |
| `syncSquareColourR`, `syncSquareColourG`, `syncSquareColourB`, `syncSquareColourA` | The RGBA values of the sync square. | `0`-`1` | Red |
| `syncSquareDisplayNum` | The display number to put the sync square on. | `0`-`3` | `0` |
| `syncSquarePosX` `syncSquarePosY` | The X and Y positions of the sync square. | a really small number-a really big number | `syncSquarePosX: -29.84` `syncSquarePosY: 18.17102`|
| `syncSquareScalar` | A scalar to modify the size of the sync square | `0`-a really big number (but you probably don't want it to be too big) | `1` |
| `displayStimulusCode` | Whether to display the sync square or not. | `0` (false) or `1` (true) | `1` |
| `eyeHeight` | The height of the eye of the animal relative to the bottom PIXEL of the front/side monitors (cm). This is always in the center of the monitors. | `0`-`99999` | `0.1` |
| `distanceToMonitors` | The distance from the center of the eye to all monitors (cm). | `0`-a very large number | `27` |
|  `monitorDimensionsX`, `monitorDimensionsY` | The dimensions of the monitors in cm. | `0`-`99999` | `monitorDimensionsX: 52` `monitorsDimensionsY: 32` |
| `frontDisplayNum`, `rightDisplayNum`, `backDisplayNum`, `leftDisplayNum` | The display numbers for front, right, back and left displays. | `0`-`3` | `frontDisplayNum: 0`, `rightDisplayNum: 1`, `backDisplayNum: 2`, `leftDisplayNum: 3` |
| `darkAdaptTime` | The time in seconds to display a black screen for dark Adaptation (at the start of a trial). | `0`-`99999` | `0` |
| `fictracFeedback` | Whether to use a closed-loop experiment that moves the animal's position using a socket connection with FicTrac. Note, fictrac must be sending socket info to `localhost:1111` | `0` (false) or `1` (true) | `0` |
| `mustIncludeEveryParameter` | Whether you should enforce specifying every parameter in a config file. This can be useful for debugging, or if you want to be extra-cautious when specifying your stimuli. | `0` (false) or `1` (true) | `0` |


## Loom specific
*Parameters used in the Loom stimulus*
| Parameter name | Description | Options | Default value |
| -------------- | ----------- | ------- | ------------- |
| `aboveHorizonColourR`, `aboveHorizonColourG`, `aboveHorizonColourB`, `aboveHorizonColourA` | The RGBA values of the above horizon colour. | `0`-`1` | White |
| `belowHorizonColourR`, `belowHorizonColourG`, `belowHorizonColourB`, `belowHorizonColourA` | The RGBA values of the below horizon colour. | `0`-`1` | Grey |
| `horizonHeight` | The height of the horizon in degrees (`0` is exactly horizontal relative to the eye position). | `-90`-`90` | `0` |
| `aboveHorizonColourRFront`, `aboveHorizonColourGFront`, `aboveHorizonColourBFront`, `aboveHorizonColourAFront` | An override of the RGBA values of the above horizon colour for the front display. Can also override `Right`, `Left` and `Back` displays by substituting them in for `Front` in the parameter name. | `0`-`1` | *Ignored* |
| `belowHorizonColourRFront`, `belowHorizonColourGFront`, `belowHorizonColourBFront`, `belowHorizonColourAFront` | An override of the RGBA values of the below horizon colour for the front display. Can also override `Right`, `Left` and `Back` displays by substituting them in for `Front` in the parameter name. | `0`-`1` | *Ignored* |
| `horizonHeightFront` | An override of the height of the horizon in degrees for the front display. Can also override `Right`, `Left` and `Back` displays by substituting them in for `Front` in the parameter name. | `-90`-`90` | *Ignored* |
| `startScaleX`, `startScaleY`, `startScaleZ` | The scale of the stimulus at the start of the loom. A scale of 1 is 1cm in Unity. | `0`-a very large number | `startScaleX: 1`, `startScaleY: 1`, `startScaleZ: 1` |
| `endScaleX`, `endScaleY`, `endScaleZ` | The scale of the stimulus at the end of the loom. A scale of 1 is 1cm in Unity.  | `0`-a very large number | `endScaleX: 1`, `endScaleY: 1`, `endScaleZ: 1` |
| `startElevation`, `startAzimuth` | The stimulus polar position at the start of the loom. | startElevation: `-90`-`90`, startAzimuth: `-180`-`180` | `startElevation: 0`, `startAzimuth: 0` |
| `endElevation`, `endAzimuth` | The stimulus polar position at the end of the loom. Note, X and Y are rotational axes, so X is up and down, Y is left and right. X is the equivalent of negative elevation, Y is the equivalent of azimuth. | X: `-90`-`90`, Y: `-180`-`180` | `endElevation: 0`, `endAzimuth: 0` |
| `originX`, `originY`, `originZ` | The offset of the target location from the eye position in cartesian coordinates and cm. | `0`-a very large number | `0` |
| `startDistance` | The offset of the stimulus from the eye position in cm at the start of the loom. | `0`-a very large number | `50` |
| `endDistance` | The offset of the stimulus from the eye position in cm at the end of the loom. | `0`-a very large number | `1` |
| `duration` | The duration of one cycle of the loom in seconds. | `0`-`99999` | `2` |
| `numReps` | The number of times to repeat the stimulus movement (`0.5` is start-finish, `1` is start-finish-start). | `0`-`99999` | `0.5` |
| `delayToApproach` | The delay in seconds before the loom starts. | `0`-`99999` | `5` |
| `delayToAppear` | The delay in seconds before the looming object appears. | `0`-`99999` | `0` |
| `stimulusColourR`, `stimulusColourG`, `stimulusColourB`, `stimulusColourA` | The RGBA values of the looming object in the stimulus. | `0`-`1` | Grey |
| `drawOutline` | Whether to draw an outline around the stimulus or not. | `0` (false) or `1` (true) | `0` |
| `outlineColourR`, `outlineColourG`, `outlineColourB`, `outlineColourA` | The RGBA values of the outline. | `0`-`1` | Black |
| `outlineWidth` | The width of the outline. | `0`-`99999` | `5` |
| `stimulusType` | The type of stimulus to display. Note, square will always face the origin | `0` (sphere), `1` (square) `2` (a sphere with a grating)) | `0` |
| `gratingNum` | The number of grating cycles to display if `stimulusType` is `2`. | `0`-`99999` | `100` |
| `gratingIsSquare` | Whether the grating is square (true) or a sinewave (false). | `0` (false) or `1` (true) | `0` |
| `gratingMaxIntensity` | The maximum intensity of the grating. | `0`-`1` | `1` |
| `gratingMinIntensity` | The minimum intensity of the grating. | `0`-`1` | `0` |
| `fixedAngularSize` | Whether to use a fixed angular size or not. | `0` (false) or `1` (true) | `0` |
| `fixElevation` | Whether to fix the elevation (up and down) if true or the azimuth if false (right and left). | `0` (false) or `1` (true) | `0` |
| `hideAtEnd` | Whether to hide the stimulus at the end of the loom. | `0` (false) or `1` (true) | `0` |
| `directPath` | Whether to loom with a direct path from the start to end polar coordinates. If false, then will use the greater circle distance. | `0` (false) or `1` (true) | `1` |
| `` | Whether to fix the X axis (up and down) if true or the Y axis if false (right and left). | `0` (false) or `1` (true) | `0` |
| `minAngularAngle`, `maxAngularAngle` | The minimum and maximum angular degrees to allow the stimulus object to be displayed in if `fixedAngularSize` is true. If the object is outside this range, then it will become partly or wholely invisible (will not be rendered). 0 is forwards, -ve is up/left and +ve is down/right | `-180`-`180` | `minAngularSize: -30`, `maxAngularSize: 30` |

## DualLoom specific
*Parameters used in the DualLoom stimulus*

Same as Loom, but the following parameters are modified for each looming object (designated by its suffix, `1` for the first object and `2` for the second). The below parameters have the `1` suffix for an example:

| Parameter name | Description | Options | Default value |
| -------------- | ----------- | ------- | ------------- |
| `startScaleX1`, `startScaleY1`, `startScaleZ1` | The scale of the stimulus at the start of the loom. | `0`-a very large number | `startScaleX1: 1`, `startScaleY1: 1`, `startScaleZ1: 1` |
| `endScaleX1`, `endScaleY1`, `endScaleZ1` | The scale of the stimulus at the end of the loom. | `0`-a very large number | `endScaleX1: 1`, `endScaleY1: 1`, `endScaleZ1: 1` |
| `startElevation1`, `startAzimuth1` | The stimulus polar position at the start of the loom. | elevation: `-90`-`90`, azimuth: `-180`-`180` | `startElevation1: 0`, `startAzimuth1: 0` |
| `endElevation1`, `endAzimuth1` | The stimulus polar position at the end of the loom. Note, X and Y are rotational axes, so X is up and down, Y is left and right. | X: `-90`-`90`, Y: `-180`-`180` | `endElevation1: 0`, `endAzimuth1: 0` |
| `originX1`, `originY1`, `originZ1` | The offset of the target location from the eye position in cartesian coordinates and cm. | `0`-a very large number | `0` |
| `startDistance1` | The offset of the stimulus from the eye position in cm at the start of the loom. | `0`-a very large number | `0` |
| `endDistance1` | The offset of the stimulus from the eye position in cm at the end of the loom. | `0`-a very large number | `0` |
| `duration1` | The duration of one cycle of the loom in seconds. | `0`-`99999` | `0` |
| `numReps1` | The number of times to repeat the stimulus movement (`0.5` is start-finish, `1` is start-finish-start). | `0`-a very large number | `1` |
| `delayToApproach1` | The delay in seconds before the loom starts. | `0`-`99999` | `5` |
| `delayToAppear1` | The delay in seconds before the looming object appears. | `0`-`99999` | `0` |
| `stimulusColourR1`, `stimulusColourG1`, `stimulusColourB1`, `stimulusColourA1` | The RGBA values of the looming object in the stimulus. | `0`-`1` | Grey |
| `drawOutline1` | Whether to draw an outline around the stimulus or not. | `0` (false) or `1` (true) | `0` |
| `outlineColourR1`, `outlineColourG1`, `outlineColourB1`, `outlineColourA1` | The RGBA values of the outline. | `0`-`1` | Black |
| `outlineWidth1` | The width of the outline in pixels. | `0`-`99999` | `1` |
| `stimulusType1` | The type of stimulus to display. | `0` (sphere), `1` (square) `2` (a sphere with a grating)) | `0` |
| `minAngularAngle1`, `maxAngularAngle1` | The minimum and maximum angular degrees to allow the stimulus object to be displayed in if `fixedAngularSize1` is true. If the object is outside this range, then it will become partly or wholely invisible (will not be rendered). 0 is forwards, -ve is up/left and +ve is down/right | `-180`-`180` | `minAngularSize1: -30`, `maxAngularSize1: 30` |

## Mimic Expansion Speed Loom specific
*Parameters used in the MimicExpansionSpeedLoom stimulus*

Same as Loom, but with the following additional parameters:
| Parameter name | Description | Options | Default value |
| -------------- | ----------- | ------- | ------------- |
| `mimicExpansionSpeed` | Whether or not to mimic expansion speed using the reference parameters (below) | `0` (false) or `1` (true) | `1` |
| `mimicExpansionSpeedMethod` | The method to use to mimic the expansion speed. | `1` (match expansionSpeed of directly approaching loom to another direct approaching looming stimuli) or `2` (match a near miss stimulus with the expansion speed of a directly looming stimulus or vice versa by adjusting the current stimuli's size over time) | `0` |
| `referenceInitialDistance` | Initial distance of the reference stimulus | Any number | `1` |
| `referenceEndDistance` | End distance of the reference stimulus | Any number | `1` |
| `equalDistance` | ? (Ask Zahra) | Any number | `1` |
| `moveTime` | ? (Ask Zahra) | Any number | `1` |
| `referenceStartElevation` | Starting elevation of the reference stimulus loom | Any number | `0` |
| `referenceEndElevation` | Ending elevation of the reference stimulus loom | Any number | `0` |
| `referenceStartAzimuth` | Starting Azimuth of the reference stimulus loom | Any number | `0` |
| `referenceEndAzimuth` | Ending Azimuth of the reference stimulus loom | Any number | `0` |

## Mimic Expansion Speed Dual Loom specific
*Parameters used in the MimicExpansionSpeedDualLoom stimulus*

Same as DualLoom with additional MimicExpansionSpeedLoom parameters that are modified for each looming object (designated by its suffix, `1` for the first object and `2` for the second).


## Moving Rectangle specific
*Parameters used in the Moving Rectangle stimulus*

| Parameter name | Description | Options | Default value |
| -------------- | ----------- | ------- | ------------- |
| `aboveHorizonColourR`, `aboveHorizonColourG`, `aboveHorizonColourB`, `aboveHorizonColourA` | The RGBA values of the above horizon colour. | `0`-`1` | White |
| `belowHorizonColourR`, `belowHorizonColourG`, `belowHorizonColourB`, `belowHorizonColourA` | The RGBA values of the below horizon colour. | `0`-`1` | Grey |
| `horizonHeight` | The height of the horizon in degrees (`0` is exactly horizontal relative to the eye position). | `-90`-`90` | `0` |
| `width` | The width of the rectangle. | `0`-a very large number | `300` |
| `height` | The height of the rectangle. | `0`-a very large number | `300` |
| `startPosX`, `startPosY` | The starting position of the rectangle on the display. | `-a very large number`-`a very large number` | `startPosX: 0`, `startPosY: 0` |
| `endPosX`, `endPosY` | The ending position of the rectangle on the display. | `-a very large number`-`a very large number` | `endPosX: 0`, `endPosY: 0` |
| `numReps` | The number of times to repeat the stimulus movement (`0.5` is start-finish, `1` is start-finish-start). | `0`-a very large number | `1` |
| `duration` | The duration of one half cycle of the stimulus in seconds. | `0`-`99999` | `0` |
| `delayToApproach` | The delay in seconds before the stimulus starts. | `0`-`99999` | `5` |
| `stimulusColourR`, `stimulusColourG`, `stimulusColourB`, `stimulusColourA` | The RGBA values of the rectangle. | `0`-`1` | White |

## Optomotor specific

| Parameter name | Description | Options | Default value |
| -------------- | ----------- | ------- | ------------- |
| `density` | The number of vertical black or white bars around the horizon (360 degrees) | `0`-a very large number | `100` |
| `offset` | The offset of the bars from the 0 degrees. | `-180`-`180` | `0` |
| `angle` | The angle of the bars in degrees. | `-180`-`180` | `0` |
| `speed` | The speed of the bars in degrees per second. Negative speed means the bars move in the opposite direction. | `-a very large number`-`a very large number` | `5` |
| `square` | Whether to draw square waves (true) or sinewaves (false). | `0` (false) or `1` (true) | `0` |
| `minimumVal` | The minimum intensity of the bars. | `0`-`1` | `0` |
| `maximumVal` | The maximum intensity of the bars. | `0`-`1` | `1` |
