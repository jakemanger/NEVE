using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;

public class HyperiidLoomingStimulusArenaManager : MonoBehaviour
{
    public bool recieveParametersFromPython = true;

    [Header("Background parameters")]
    public Color backgroundColour = new Color(0f, 0f, 0f, 1f);

    [Header("Camera view parameters")]
    public float eyeHeight = 2f; // cm vertically relative to bottom of front facing monitors
    public float distanceToMonitors = 7; // cm
    public Vector2 monitorDimensions = new Vector2(12.176f, 6.87f);
    public int frontDisplayNum = 0;
    public int rightDisplayNum = 1;
    public int backDisplayNum = 2;
    public int leftDisplayNum = 3;

    [Header("Stimulus parameters")]
    public float stimulusSize = 1f;
    public Vector2 stimulusPolarPosition = new Vector2(0f, 0f);
    public Vector3 targetLocationOffset = new Vector3(0f, 0f, 0f);
    public float startOffset = 10f;
    public float endOffset = 10f;
    public float stimulusMoveSpeed = 1f; // units (cm) per second
    public float delayToApproach = 5f;
    public float flickerDuration = 0.1f; // time sphere renderer is off in seconds
    public int stimulusType = 0;
    public bool drawOutline = false;
    public float outlineWidth = 5f;
    public Color outlineColor = Color.black;

    public bool manualControl = true;
    public float mouseMoveSpeed = 2f;

    public Color stimulusColour = Color.white;

    // the time in seconds that the stimulus will run for until it waits for
    // further input from python
    public float experimentDuration = 60f;

    [Header("Saving parameters")]
    public bool recordFrameData = true;
    public bool recordEachFrame = true;
    public float recordingFrequency = 1f; // in seconds if recordEachFrame is false
    public float frameDataIdCode = 9999; // a code to identify the frame data recording

    [Header("SyncSquare parameters")]
    public Color syncSquareColor = Color.red;
    public SyncSquare syncSquare;
    public int syncSquareDisplayNum = 0;
    public bool displayStimulusCode = false;

    [Header("Components")]
    public CameraMonitorController camMon;
    public SphericalStimulusGenerator stimGenerator;
    public FrameWriter frameWriter;
    public EpisodeControllerAgent episodeController; // for controlling when a stimulus has finished and a new one should be loaded

    // use OnEnable as it is executed before stimGenerators Start() function
    // and can restart the stimulus if you disable and enable this gameObject
    // void OnEnable() {
    //     Reset();
    // }

    public void Reset() {
        if (recieveParametersFromPython) {
            GetPropertiesFromPython();
        }

        episodeController.experimentDuration = experimentDuration;

        SetupStimuli();

        // Setup cameras and frame writer
        camMon.frontDisplayNum = frontDisplayNum;
        camMon.rightDisplayNum = rightDisplayNum;
        camMon.backDisplayNum = backDisplayNum;
        camMon.leftDisplayNum = leftDisplayNum;
        camMon.SetupCams(distanceToMonitors, -eyeHeight, monitorDimensions, true, new Color[] {backgroundColour, backgroundColour, backgroundColour, backgroundColour});
        frameWriter.recordEachFrame = recordFrameData;
        frameWriter.recordingFrequency = recordingFrequency;
        frameWriter.experimentId = frameDataIdCode.ToString();
        frameWriter.Reset();
        syncSquare.transform.parent.GetComponent<Canvas>().targetDisplay = syncSquareDisplayNum;
        syncSquare.flickerDuration = flickerDuration;
        syncSquare.flickerColor = syncSquareColor;
        syncSquare.displayStimulusCode = displayStimulusCode;
        syncSquare.stimulusCode = frameDataIdCode;
        syncSquare.Reset();
    }

    void GetPropertiesFromPython() {
        // load properties from python
        var floatChannel = Academy.Instance.EnvironmentParameters;
        // set properties from python
        float r = floatChannel.GetWithDefault("backgroundColourR", 0f);
        float g = floatChannel.GetWithDefault("backgroundColourG", 0f);
        float b = floatChannel.GetWithDefault("backgroundColourB", 0f);
        float a = floatChannel.GetWithDefault("backgroundColourA", 1f);
        backgroundColour = new Color(r, g, b, a);
        eyeHeight = floatChannel.GetWithDefault("eyeHeight", 2f);
        distanceToMonitors = floatChannel.GetWithDefault("distanceToMonitors", 7f);
        float monitorDimensionsX = floatChannel.GetWithDefault("monitorDimensionsX", 12.176f);
        float monitorDimensionsY = floatChannel.GetWithDefault("monitorDimensionsY", 6.87f);
        monitorDimensions = new Vector2(monitorDimensionsX, monitorDimensionsY);
        stimulusSize = floatChannel.GetWithDefault("stimulusSize", 1f);
        float stimulusPolarPositionX = floatChannel.GetWithDefault("stimulusPolarPositionX", 0f);
        float stimulusPolarPositionY = floatChannel.GetWithDefault("stimulusPolarPositionY", 0f);
        stimulusPolarPosition = new Vector2(stimulusPolarPositionX, stimulusPolarPositionY);
        float targetLocationOffsetX = floatChannel.GetWithDefault("targetLocationOffsetX", 0f);
        float targetLocationOffsetY = floatChannel.GetWithDefault("targetLocationOffsetY", 0f);
        float targetLocationOffsetZ = floatChannel.GetWithDefault("targetLocationOffsetZ", 0f);
        targetLocationOffset = new Vector3(targetLocationOffsetX, targetLocationOffsetY, targetLocationOffsetZ);
        startOffset = floatChannel.GetWithDefault("startOffset", 50f);
        endOffset = floatChannel.GetWithDefault("endOffset", 1f);
        stimulusType = (int)floatChannel.GetWithDefault("stimulusType", 0); // 0 = icosphere, 1 = unity cube
        drawOutline = floatChannel.GetWithDefault("drawOutline", 0) != 0;
        outlineWidth = floatChannel.GetWithDefault("outlineWidth", 5f);
        r = floatChannel.GetWithDefault("outlineColourR", 0f);
        g = floatChannel.GetWithDefault("outlineColourG", 0f);
        b = floatChannel.GetWithDefault("outlineColourB", 0f);
        a = floatChannel.GetWithDefault("outlineColourA", 1f);
        outlineColor = new Color(r, g, b, a);
        stimulusMoveSpeed = floatChannel.GetWithDefault("stimulusMoveSpeed", 1f);
        delayToApproach = floatChannel.GetWithDefault("delayToApproach", 5f);
        flickerDuration = floatChannel.GetWithDefault("flickerDuration", 0.1f);
        r = floatChannel.GetWithDefault("syncSquareColorR", 1f);
        g = floatChannel.GetWithDefault("syncSquareColorG", 0f);
        b = floatChannel.GetWithDefault("syncSquareColorB", 0f);
        a = floatChannel.GetWithDefault("syncSquareColorA", 1f);
        syncSquareColor = new Color(r, g, b, a);
        syncSquareDisplayNum = (int)floatChannel.GetWithDefault("syncSquareDisplayNum", 0f);
        displayStimulusCode = floatChannel.GetWithDefault("displayStimulusCode", 1f) != 0;
        manualControl = floatChannel.GetWithDefault("manualControl", 1f) != 0;
        mouseMoveSpeed = floatChannel.GetWithDefault("mouseMoveSpeed", 2f);
        r = floatChannel.GetWithDefault("stimulusColourR", 0.1f);
        g = floatChannel.GetWithDefault("stimulusColourG", 0.1f);
        b = floatChannel.GetWithDefault("stimulusColourB", 0.1f);
        a = floatChannel.GetWithDefault("stimulusColourA", 1f);
        stimulusColour = new Color(r, g, b, a);
        experimentDuration = floatChannel.GetWithDefault("experimentDuration", 60f);
        recordFrameData = floatChannel.GetWithDefault("recordFrameData", 1f) != 0;
        recordEachFrame = floatChannel.GetWithDefault("recordEachFrame", 1f) != 0;
        recordingFrequency = floatChannel.GetWithDefault("recordingFrequency", 1f);
        frameDataIdCode = floatChannel.GetWithDefault("frameDataIdCode", 9999f);
        frontDisplayNum = (int)floatChannel.GetWithDefault("frontDisplayNum", 0f);
        rightDisplayNum = (int)floatChannel.GetWithDefault("rightDisplayNum", 1f);
        backDisplayNum = (int)floatChannel.GetWithDefault("backDisplayNum", 2f);
        leftDisplayNum = (int)floatChannel.GetWithDefault("leftDisplayNum", 3f);
    }

    void SetupStimuli() {
        stimGenerator.stimulusColour = stimulusColour;
        stimGenerator.stimulusSize = stimulusSize;
        stimGenerator.startPolarPosition = stimulusPolarPosition;
        stimGenerator.endPolarPosition = stimulusPolarPosition;
        stimGenerator.startOffset = startOffset;
        stimGenerator.endOffset = endOffset;
        stimGenerator.delayToApproach = delayToApproach;
        stimGenerator.targetLocationOffset = targetLocationOffset;
        stimGenerator.flickerDuration = flickerDuration;
        stimGenerator.numReps = 0.5f;
        stimGenerator.stimulusType = stimulusType;
        stimGenerator.drawOutline = drawOutline;
        stimGenerator.outlineWidth = outlineWidth;
        stimGenerator.outlineColor = outlineColor;

        float duration = Mathf.Abs(startOffset - endOffset) / stimulusMoveSpeed;
        stimGenerator.duration = duration; 

        stimGenerator.manualControl = manualControl;
        stimGenerator.mouseMoveSpeed = mouseMoveSpeed;
        stimGenerator.Reset();
    }
}
