using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;

public class FiddlerCrabLoomingStimulusArenaManager : MonoBehaviour
{
    public bool recieveParametersFromPython = true;

    [Header("Camera view parameters")]
    public float eyeHeight = 2f; // cm vertically relative to bottom of front facing monitors
    public float distanceToMonitors = 7; // cm
    public Vector2 monitorDimensions = new Vector2(12.176f, 6.87f);
    public int frontDisplayNum = 0;
    public int rightDisplayNum = 1;
    public int backDisplayNum = 2;
    public int leftDisplayNum = 3;
    public Vector3 cameraRotation = Vector3.zero;
    public bool fixedAngularSize = false;
    public bool fixXAxis = true; // otherwise fix the Y axis
    public float minAngularAngle = -30f;
    public float maxAngularAngle = 30f;

    [Header("Stimulus parameters")]
    public float horizonHeight = 0f;
    public Color aboveHorizonColour = Color.grey;
    public Color belowHorizonColour = Color.white;

    public float stimulusSize = 1f; // is ignored if startScale != endScale
    public Vector3 startScale = Vector3.one;
    public Vector3 endScale = Vector3.one;
    public Vector2 stimulusPolarPosition = new Vector2(0f, 0f);
    public Vector3 targetLocationOffset = new Vector3(0f, 0f, 0f);
    public float startOffset = 10f;
    public float endOffset = 10f;
    public float duration = 1f; // units (cm) per second
    public float delayToApproach = 5f;
    public float flickerDuration = 0.1f; // time sphere renderer is off in seconds
    
    public float gratingNum = 100f;
    public int gratingIsSquare = 0;
    public float gratingMaxIntensity = 0.1f;
    public float gratingMinIntensity = 0f;

    public bool manualControl = true;
    public float mouseMoveSpeed = 2f;

    public Color stimulusColour = Color.white;

    public int stimulusType = 0; // 0 = icosphere, 1 = unity cube
    public bool drawOutline = false;
    public float outlineWidth = 5f;
    public Color outlineColor = Color.black;

    // the time in seconds that the stimulus will run for until it waits for
    // further input from python
    public float experimentDuration = 60f;

    public GameObject blackOutCanvases;
    public float darkAdaptTime = 5f;
    float timeSinceDarkAdaptStart = 0f;

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

        blackOutCanvases.SetActive(true);

        episodeController.experimentDuration = experimentDuration;

        SetupStimuli();

        // Setup cameras and frame writer
        camMon.frontDisplayNum = frontDisplayNum;
        camMon.rightDisplayNum = rightDisplayNum;
        camMon.backDisplayNum = backDisplayNum;
        camMon.leftDisplayNum = leftDisplayNum;
        camMon.transform.localEulerAngles = cameraRotation;
        camMon.SetupCams(distanceToMonitors, -eyeHeight, monitorDimensions, false, new Color[] {Color.clear, Color.clear, Color.clear, Color.clear});
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
        timeSinceDarkAdaptStart = 0f;
    }

    void GetPropertiesFromPython() {
        // load properties from python
        var floatChannel = Academy.Instance.EnvironmentParameters;
        // set properties from python
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
        float r = floatChannel.GetWithDefault("outlineColourR", 0f);
        float g = floatChannel.GetWithDefault("outlineColourG", 0f);
        float b = floatChannel.GetWithDefault("outlineColourB", 0f);
        float a = floatChannel.GetWithDefault("outlineColourA", 1f);
        outlineColor = new Color(r, g, b, a);
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
        horizonHeight = floatChannel.GetWithDefault("horizonHeight", 0f);
        r = floatChannel.GetWithDefault("aboveHorizonColourR", 0.1f);
        g = floatChannel.GetWithDefault("aboveHorizonColourG", 0.1f);
        b = floatChannel.GetWithDefault("aboveHorizonColourB", 0.1f);
        a = floatChannel.GetWithDefault("aboveHorizonColourA", 1f);
        aboveHorizonColour = new Color(r, g, b, a);
        r = floatChannel.GetWithDefault("belowHorizonColourR", 0.1f);
        g = floatChannel.GetWithDefault("belowHorizonColourG", 0.1f);
        b = floatChannel.GetWithDefault("belowHorizonColourB", 0.1f);
        a = floatChannel.GetWithDefault("belowHorizonColourA", 1f);
        belowHorizonColour = new Color(r, g, b, a);
        experimentDuration = floatChannel.GetWithDefault("experimentDuration", 60f);
        recordFrameData = floatChannel.GetWithDefault("recordFrameData", 1f) != 0;
        recordEachFrame = floatChannel.GetWithDefault("recordEachFrame", 1f) != 0;
        recordingFrequency = floatChannel.GetWithDefault("recordingFrequency", 1f);
        frameDataIdCode = floatChannel.GetWithDefault("frameDataIdCode", 9999f);
        frontDisplayNum = (int)floatChannel.GetWithDefault("frontDisplayNum", 0f);
        rightDisplayNum = (int)floatChannel.GetWithDefault("rightDisplayNum", 1f);
        backDisplayNum = (int)floatChannel.GetWithDefault("backDisplayNum", 2f);
        leftDisplayNum = (int)floatChannel.GetWithDefault("leftDisplayNum", 3f);
        gratingNum = floatChannel.GetWithDefault("gratingNum", 100f);
        gratingIsSquare = (int)floatChannel.GetWithDefault("gratingIsSquare", 0f);
        gratingMaxIntensity = floatChannel.GetWithDefault("gratingMaxIntensity", 0.1f);
        gratingMinIntensity = floatChannel.GetWithDefault("gratingMinIntensity", 0f);
        float x = floatChannel.GetWithDefault("startScaleX", 1f);
        float y = floatChannel.GetWithDefault("startScaleY", 1f);
        float z = floatChannel.GetWithDefault("startScaleZ", 1f);
        startScale = new Vector3(x, y, z);
        x = floatChannel.GetWithDefault("endScaleX", 1f);
        y = floatChannel.GetWithDefault("endScaleY", 1f);
        z = floatChannel.GetWithDefault("endScaleZ", 1f);
        endScale = new Vector3(x, y, z);
        duration = floatChannel.GetWithDefault("duration", 1f);
        x = floatChannel.GetWithDefault("cameraRotationX", 0f);
        y = floatChannel.GetWithDefault("cameraRotationX", 0f);
        z = floatChannel.GetWithDefault("cameraRotationX", 0f);
        cameraRotation = new Vector3(x, y, z);
        darkAdaptTime = floatChannel.GetWithDefault("darkAdaptTime", 5f);
        fixedAngularSize = floatChannel.GetWithDefault("fixedAngularSize", 0) != 0;
        fixXAxis = floatChannel.GetWithDefault("fixXAxis", 1) != 0; // otherwise fix the Y axis
        minAngularAngle = floatChannel.GetWithDefault("minAngularAngle", -30f);
        maxAngularAngle = floatChannel.GetWithDefault("maxAngularAngle", 30f);
    }

    void Update() {
        if (timeSinceDarkAdaptStart < darkAdaptTime) {
            timeSinceDarkAdaptStart += Time.deltaTime;
        } else {
            blackOutCanvases.SetActive(false);
        }
    }

    void SetupStimuli() {
        // skybox
        Material mat = RenderSettings.skybox;
        mat.SetFloat("_horizonHeight", horizonHeight);
        mat.SetColor("_aboveHorizonColour", aboveHorizonColour);
        mat.SetColor("_belowHorizonColour", belowHorizonColour);
        RenderSettings.skybox = mat;

        // sphere
        stimGenerator.stimulusColour = stimulusColour;
        stimGenerator.stimulusSize = stimulusSize;
        stimGenerator.startScale = startScale;
        stimGenerator.endScale = endScale;
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
        stimGenerator.gratingNum = gratingNum;
        stimGenerator.gratingIsSquare = gratingIsSquare;
        stimGenerator.gratingMaxIntensity = gratingMaxIntensity;
        stimGenerator.gratingMinIntensity = gratingMinIntensity;
        stimGenerator.fixedAngularSize = fixedAngularSize;
        stimGenerator.fixXAxis = fixXAxis; // otherwise fix the Y axis
        stimGenerator.minAngularAngle = minAngularAngle;
        stimGenerator.maxAngularAngle = maxAngularAngle;

        stimGenerator.duration = duration; 

        stimGenerator.manualControl = manualControl;
        stimGenerator.mouseMoveSpeed = mouseMoveSpeed;
        stimGenerator.Reset();
    }
}
