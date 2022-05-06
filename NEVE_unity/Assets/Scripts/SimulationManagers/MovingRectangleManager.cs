using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;

public class MovingRectangleManager : MonoBehaviour
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

    [Header("Stimulus parameters")]
    public float horizonHeight = 0f;
    public Color aboveHorizonColour = Color.grey;
    public Color belowHorizonColour = Color.white;

    public float width = 100f;
    public float height = 100f;
    public Vector2 startPos = Vector2.zero;
    public Vector2 endPos = new Vector2(5f, 5f);
    public float duration;
    public float delayToApproach;
    public Color stimulusColour;
    public float numReps;

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
    public float frameDataIdCode = 9999f; // a code to identify the frame data recording
    public float animalCode = 1f; // a code to identify the animal

    [Header("SyncSquare parameters")]
    public float flickerDuration = 0.1f; // time of sync square flicker
    public Color syncSquareColor = Color.red;
    public SyncSquare syncSquare;
    public int syncSquareDisplayNum = 0;
    public bool displayStimulusCode = false;

    [Header("Components")]
    public CameraMonitorController camMon;
    public FrameWriter frameWriter;
    public EpisodeControllerAgent episodeController; // for controlling when a stimulus has finished and a new one should be loaded
    public SquareStimulusController squareController;


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
        syncSquare.animalCode = animalCode;
        syncSquare.Reset();
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
        flickerDuration = floatChannel.GetWithDefault("flickerDuration", 0.1f);
        float r = floatChannel.GetWithDefault("syncSquareColorR", 1f);
        float g = floatChannel.GetWithDefault("syncSquareColorG", 0f);
        float b = floatChannel.GetWithDefault("syncSquareColorB", 0f);
        float a = floatChannel.GetWithDefault("syncSquareColorA", 1f);
        syncSquareColor = new Color(r, g, b, a);
        syncSquareDisplayNum = (int)floatChannel.GetWithDefault("syncSquareDisplayNum", 0f);
        displayStimulusCode = floatChannel.GetWithDefault("displayStimulusCode", 1f) != 0;

        experimentDuration = floatChannel.GetWithDefault("experimentDuration", 60f);
        recordFrameData = floatChannel.GetWithDefault("recordFrameData", 1f) != 0;
        recordEachFrame = floatChannel.GetWithDefault("recordEachFrame", 1f) != 0;
        recordingFrequency = floatChannel.GetWithDefault("recordingFrequency", 1f);
        frameDataIdCode = floatChannel.GetWithDefault("frameDataIdCode", 9999f);
        animalCode = floatChannel.GetWithDefault("animalCode", 1f);
        frontDisplayNum = (int)floatChannel.GetWithDefault("frontDisplayNum", 0f);
        rightDisplayNum = (int)floatChannel.GetWithDefault("rightDisplayNum", 1f);
        backDisplayNum = (int)floatChannel.GetWithDefault("backDisplayNum", 2f);
        leftDisplayNum = (int)floatChannel.GetWithDefault("leftDisplayNum", 3f);
        float x = floatChannel.GetWithDefault("cameraRotationX", 0f);
        float y = floatChannel.GetWithDefault("cameraRotationY", 0f);
        float z = floatChannel.GetWithDefault("cameraRotationZ", 0f);
        cameraRotation = new Vector3(x, y, z);
        darkAdaptTime = floatChannel.GetWithDefault("darkAdaptTime", 5f);

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

        width = floatChannel.GetWithDefault("width", 5f);
        height = floatChannel.GetWithDefault("height", 5f);
        float startPosX = floatChannel.GetWithDefault("startPosX", 0f);
        float startPosY = floatChannel.GetWithDefault("startPosY", 0f);
        startPos = new Vector2(startPosX, startPosY);
        float endPosX = floatChannel.GetWithDefault("endPosX", 5f);
        float endPosY = floatChannel.GetWithDefault("endPosY", 5f);
        endPos = new Vector2(endPosX, endPosY);
        numReps = floatChannel.GetWithDefault("numReps", 2f);

        duration = floatChannel.GetWithDefault("duration", 5f);
        delayToApproach = floatChannel.GetWithDefault("delayToApproach", 5f);
        r = floatChannel.GetWithDefault("stimulusColourR", 0.1f);
        g = floatChannel.GetWithDefault("stimulusColourG", 0.1f);
        b = floatChannel.GetWithDefault("stimulusColourB", 0.1f);
        a = floatChannel.GetWithDefault("stimulusColourA", 1f);
        stimulusColour = new Color(r, g, b, a);
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

        // square
        squareController.width = width;
        squareController.height = height;
        squareController.startPos = startPos;
        squareController.endPos = endPos;
        squareController.duration = duration;
        squareController.delayToApproach = delayToApproach;
        squareController.stimulusColour = stimulusColour;
        squareController.numReps = numReps;

        squareController.Reset();
    }
}
