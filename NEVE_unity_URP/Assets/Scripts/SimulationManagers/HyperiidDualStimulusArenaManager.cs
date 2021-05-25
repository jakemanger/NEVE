using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;

public class HyperiidDualStimulusArenaManager : MonoBehaviour
{
    public bool recieveParametersFromPython = true;

    [Header("Background parameters")]
    public Color frontBackgroundColour = new Color(0f, 0f, 0f, 1f);
    public Color rightBackgroundColour = new Color(0f, 0f, 0f, 1f);
    public Color backBackgroundColour = new Color(0f, 0f, 0f, 1f);
    public Color leftBackgroundColour = new Color(0f, 0f, 0f, 1f);

    [Header("Camera view parameters")]
    public float eyeHeight = 2f; // cm vertically relative to bottom of front facing monitors
    public float distanceToMonitors = 7; // cm
    public Vector2 monitorDimensions = new Vector2(12.176f, 6.87f);
    public int frontDisplayNum = 0;
    public int rightDisplayNum = 1;
    public int backDisplayNum = 2;
    public int leftDisplayNum = 3;

    [Header("Stimulus parameters")]
    public float stimulusSize1 = 1f;
    public float stimulusSize2 = 1f;
    public Vector2 startPolarPosition1 = new Vector2(0f, 0f);
    public Vector2 startPolarPosition2 = new Vector2(0f, 0f);
    public Vector2 endPolarPosition1 = new Vector2(0f, 0f);
    public Vector2 endPolarPosition2 = new Vector2(0f, 0f);
    public Vector3 targetLocationOffset1 = new Vector3(0f, 0f, 0f);
    public Vector3 targetLocationOffset2 = new Vector3(0f, 0f, 0f);
    public float startOffset1 = 10f;
    public float startOffset2 = 10f;
    public float endOffset1 = 10f;
    public float endOffset2 = 10f;
    public float delayToApproach1 = 5f;
    public float delayToApproach2 = 5f;
    public float numReps1 = 2;
    public float numReps2 = 2;
    public Color stimulusColour1 = Color.white;
    public Color stimulusColour2 = Color.white;
    public float stimulusDuration1 = 5f;
    public float stimulusDuration2 = 5f;
    public int stimulusType1 = 0;
    public bool drawOutline1 = false;
    public float outlineWidth1 = 5f;
    public Color outlineColor1 = Color.black;
    public int stimulusType2 = 0;
    public bool drawOutline2 = false;
    public float outlineWidth2 = 5f;
    public Color outlineColor2 = Color.black;

    public bool manualControl = true;
    public float mouseMoveSpeed = 2f;


    // the time in seconds that the stimulus will run for until it waits for
    // further input from python
    public float experimentDuration = 99999f;

    [Header("Saving parameters")]
    public bool recordFrameData = true;
    public bool recordEachFrame = true;
    public float recordingFrequency = 1f; // in seconds if recordEachFrame is false
    public float frameDataIdCode = 9999; // a code to identify the frame data recording

    [Header("SyncSquare parameters")]
    public Color syncSquareColor = Color.red;
    public SyncSquare syncSquare;
    public float flickerDuration = 0.1f; // time sphere renderer is off in seconds
    public int syncSquareDisplayNum = 0;
    public bool displayStimulusCode = false;

    [Header("Components")]
    public CameraMonitorController camMon;
    public SphericalStimulusGenerator stimGenerator1;
    public SphericalStimulusGenerator stimGenerator2;
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
        camMon.SetupCams(distanceToMonitors, -eyeHeight, monitorDimensions, true, new Color[]{frontBackgroundColour, rightBackgroundColour, backBackgroundColour, leftBackgroundColour});
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
        frameDataIdCode = floatChannel.GetWithDefault("frameDataIdCode", 9999f);
        experimentDuration = floatChannel.GetWithDefault("experimentDuration", 99999f);
        recordFrameData = floatChannel.GetWithDefault("recordFrameData", 1f) != 0;
        recordEachFrame = floatChannel.GetWithDefault("recordEachFrame", 1f) != 0;
        recordingFrequency = floatChannel.GetWithDefault("recordingFrequency", 1f);
        manualControl = floatChannel.GetWithDefault("manualControl", 1f) != 0;
        mouseMoveSpeed = floatChannel.GetWithDefault("mouseMoveSpeed", 2f);
        flickerDuration = floatChannel.GetWithDefault("flickerDuration", 0.1f);
        float r = floatChannel.GetWithDefault("syncSquareColorR", 1f);
        float g = floatChannel.GetWithDefault("syncSquareColorG", 0f);
        float b = floatChannel.GetWithDefault("syncSquareColorB", 0f);
        float a = floatChannel.GetWithDefault("syncSquareColorA", 1f);
        syncSquareColor = new Color(r, g, b, a);
        syncSquareDisplayNum = (int)floatChannel.GetWithDefault("syncSquareDisplayNum", 0f);
        displayStimulusCode = floatChannel.GetWithDefault("displayStimulusCode", 1f) != 0;

        eyeHeight = floatChannel.GetWithDefault("eyeHeight", 2f);
        distanceToMonitors = floatChannel.GetWithDefault("distanceToMonitors", 7f);
        float monitorDimensionsX = floatChannel.GetWithDefault("monitorDimensionsX", 12.176f);
        float monitorDimensionsY = floatChannel.GetWithDefault("monitorDimensionsY", 6.87f);
        monitorDimensions = new Vector2(monitorDimensionsX, monitorDimensionsY);
        frontDisplayNum = (int)floatChannel.GetWithDefault("frontDisplayNum", 0f);
        rightDisplayNum = (int)floatChannel.GetWithDefault("rightDisplayNum", 1f);
        backDisplayNum = (int)floatChannel.GetWithDefault("backDisplayNum", 2f);
        leftDisplayNum = (int)floatChannel.GetWithDefault("leftDisplayNum", 3f);

        r = floatChannel.GetWithDefault("frontBackgroundColourR", 0f);
        g = floatChannel.GetWithDefault("frontBackgroundColourG", 0f);
        b = floatChannel.GetWithDefault("frontBackgroundColourB", 0f);
        a = floatChannel.GetWithDefault("frontBackgroundColourA", 1f);
        frontBackgroundColour = new Color(r, g, b, a);
        r = floatChannel.GetWithDefault("rightBackgroundColourR", 0f);
        g = floatChannel.GetWithDefault("rightBackgroundColourG", 0f);
        b = floatChannel.GetWithDefault("rightBackgroundColourB", 0f);
        a = floatChannel.GetWithDefault("rightBackgroundColourA", 1f);
        rightBackgroundColour = new Color(r, g, b, a);
        r = floatChannel.GetWithDefault("backBackgroundColourR", 0f);
        g = floatChannel.GetWithDefault("backBackgroundColourG", 0f);
        b = floatChannel.GetWithDefault("backBackgroundColourB", 0f);
        a = floatChannel.GetWithDefault("backBackgroundColourA", 1f);
        backBackgroundColour = new Color(r, g, b, a);
        r = floatChannel.GetWithDefault("leftBackgroundColourR", 0f);
        g = floatChannel.GetWithDefault("leftBackgroundColourG", 0f);
        b = floatChannel.GetWithDefault("leftBackgroundColourB", 0f);
        a = floatChannel.GetWithDefault("leftBackgroundColourA", 1f);
        leftBackgroundColour = new Color(r, g, b, a);


        stimulusSize1 = floatChannel.GetWithDefault("stimulusSize1", 1f);
        stimulusDuration1 = floatChannel.GetWithDefault("stimulusDuration1", 5f);
        float startPolarPositionX1 = floatChannel.GetWithDefault("startPolarPositionX1", 0f);
        float startPolarPositionY1 = floatChannel.GetWithDefault("startPolarPositionY1", 0f);
        startPolarPosition1 = new Vector2(startPolarPositionX1, startPolarPositionY1);
        float endPolarPositionX1 = floatChannel.GetWithDefault("endPolarPositionX1", 0f);
        float endPolarPositionY1 = floatChannel.GetWithDefault("endPolarPositionY1", 0f);
        endPolarPosition1 = new Vector2(endPolarPositionX1, endPolarPositionY1);
        float targetLocationOffsetX1 = floatChannel.GetWithDefault("targetLocationOffsetX1", 0f);
        float targetLocationOffsetY1 = floatChannel.GetWithDefault("targetLocationOffsetY1", 0f);
        float targetLocationOffsetZ1 = floatChannel.GetWithDefault("targetLocationOffsetZ1", 0f);
        targetLocationOffset1 = new Vector3(targetLocationOffsetX1, targetLocationOffsetY1, targetLocationOffsetZ1);
        startOffset1 = floatChannel.GetWithDefault("startOffset1", 50f);
        endOffset1 = floatChannel.GetWithDefault("endOffset1", 1f);
        delayToApproach1 = floatChannel.GetWithDefault("delayToApproach1", 5f);
        numReps1 = floatChannel.GetWithDefault("numReps1", 1f);
        r = floatChannel.GetWithDefault("stimulusColourR1", 0.1f);
        g = floatChannel.GetWithDefault("stimulusColourG1", 0.1f);
        b = floatChannel.GetWithDefault("stimulusColourB1", 0.1f);
        a = floatChannel.GetWithDefault("stimulusColourA1", 1f);
        stimulusColour1 = new Color(r, g, b, a);
        stimulusType1 = (int)floatChannel.GetWithDefault("stimulusType1", 0); // 0 = icosphere, 1 = unity cube
        drawOutline1 = floatChannel.GetWithDefault("drawOutline1", 0) != 0;
        outlineWidth1 = floatChannel.GetWithDefault("outlineWidth1", 5f);
        r = floatChannel.GetWithDefault("outlineColourR1", 0f);
        g = floatChannel.GetWithDefault("outlineColourG1", 0f);
        b = floatChannel.GetWithDefault("outlineColourB1", 0f);
        a = floatChannel.GetWithDefault("outlineColourA1", 1f);
        outlineColor1 = new Color(r, g, b, a);

        stimulusSize2 = floatChannel.GetWithDefault("stimulusSize2", 1f);
        stimulusDuration2 = floatChannel.GetWithDefault("stimulusDuration2", 5f);
        float startPolarPositionX2 = floatChannel.GetWithDefault("startPolarPositionX2", 0f);
        float startPolarPositionY2 = floatChannel.GetWithDefault("startPolarPositionY2", 0f);
        startPolarPosition2 = new Vector2(startPolarPositionX2, startPolarPositionY2);
        float endPolarPositionX2 = floatChannel.GetWithDefault("endPolarPositionX2", 0f);
        float endPolarPositionY2 = floatChannel.GetWithDefault("endPolarPositionY2", 0f);
        endPolarPosition2 = new Vector2(endPolarPositionX2, endPolarPositionY2);
        float targetLocationOffsetX2 = floatChannel.GetWithDefault("targetLocationOffsetX2", 0f);
        float targetLocationOffsetY2 = floatChannel.GetWithDefault("targetLocationOffsetY2", 0f);
        float targetLocationOffsetZ2 = floatChannel.GetWithDefault("targetLocationOffsetZ2", 0f);
        targetLocationOffset2 = new Vector3(targetLocationOffsetX2, targetLocationOffsetY2, targetLocationOffsetZ2);
        startOffset2 = floatChannel.GetWithDefault("startOffset2", 50f);
        endOffset2 = floatChannel.GetWithDefault("endOffset2", 1f);
        delayToApproach2 = floatChannel.GetWithDefault("delayToApproach2", 5f);
        numReps2 = floatChannel.GetWithDefault("numReps2", 1f);
        r = floatChannel.GetWithDefault("stimulusColourR2", 0.1f);
        g = floatChannel.GetWithDefault("stimulusColourG2", 0.1f);
        b = floatChannel.GetWithDefault("stimulusColourB2", 0.1f);
        a = floatChannel.GetWithDefault("stimulusColourA2", 1f);
        stimulusColour2 = new Color(r, g, b, a);
        stimulusType2 = (int)floatChannel.GetWithDefault("stimulusType1", 0); // 0 = icosphere, 1 = unity cube
        drawOutline2 = floatChannel.GetWithDefault("drawOutline1", 0) != 0;
        outlineWidth2 = floatChannel.GetWithDefault("outlineWidth1", 5f);
        r = floatChannel.GetWithDefault("outlineColourR1", 0f);
        g = floatChannel.GetWithDefault("outlineColourG1", 0f);
        b = floatChannel.GetWithDefault("outlineColourB1", 0f);
        a = floatChannel.GetWithDefault("outlineColourA1", 1f);
        outlineColor2 = new Color(r, g, b, a);
    }

    void SetupStimuli() {
        // stimulus 1
        stimGenerator1.flickerDuration = flickerDuration;
        stimGenerator1.stimulusColour = stimulusColour1;
        stimGenerator1.stimulusSize = stimulusSize1;
        stimGenerator1.startOffset = startOffset1;
        stimGenerator1.endOffset = endOffset1;
        stimGenerator1.delayToApproach = delayToApproach1;
        stimGenerator1.targetLocationOffset = targetLocationOffset1;
        stimGenerator1.startPolarPosition = startPolarPosition1;
        stimGenerator1.endPolarPosition = endPolarPosition1;
        stimGenerator1.numReps = numReps1;
        stimGenerator1.duration = stimulusDuration1; 
        stimGenerator1.stimulusType = stimulusType1;
        stimGenerator1.drawOutline = drawOutline1;
        stimGenerator1.outlineWidth = outlineWidth1;
        stimGenerator1.outlineColor = outlineColor1;

        stimGenerator1.manualControl = manualControl;
        stimGenerator1.mouseMoveSpeed = mouseMoveSpeed;
        stimGenerator1.Reset();

        // stimulus 2
        stimGenerator2.flickerDuration = flickerDuration;
        stimGenerator2.stimulusColour = stimulusColour2;
        stimGenerator2.stimulusSize = stimulusSize2;
        stimGenerator2.startOffset = startOffset2;
        stimGenerator2.endOffset = endOffset2;
        stimGenerator2.delayToApproach = delayToApproach2;
        stimGenerator2.targetLocationOffset = targetLocationOffset2;
        stimGenerator2.startPolarPosition = startPolarPosition2;
        stimGenerator2.endPolarPosition = endPolarPosition2;
        stimGenerator2.numReps = numReps2;
        stimGenerator2.duration = stimulusDuration2; 
        stimGenerator2.stimulusType = stimulusType2;
        stimGenerator2.drawOutline = drawOutline2;
        stimGenerator2.outlineWidth = outlineWidth2;
        stimGenerator2.outlineColor = outlineColor2;

        stimGenerator2.manualControl = manualControl;
        stimGenerator2.mouseMoveSpeed = mouseMoveSpeed;
        stimGenerator2.Reset();
    }

    Vector3 PolarToCartesian(Vector2 polar, float offset) {
        Vector3 origin = new Vector3(0, 0, offset);

        // build a quaternion using euler angles for lat and lon
        Quaternion rotation = Quaternion.Euler(polar.x, polar.y, 0);
        // transform reference vector by the rotation
        Vector3 point = rotation * origin;

        return point;
    }
}
