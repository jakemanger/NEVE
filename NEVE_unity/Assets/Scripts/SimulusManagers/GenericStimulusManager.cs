using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;

public abstract class GenericStimulusManager : MonoBehaviour
{
    // A generic class for managing stimuli

    public bool recieveParametersFromPython = true;

    public bool recieveInputFromSocket = false;

    [Header("Generic camera view parameters")]
    public float eyeHeight = 2f; // cm vertically relative to bottom of front facing monitors
    public float distanceToMonitors = 7; // cm
    public Vector2 monitorDimensions = new Vector2(12.176f, 6.87f);
    public int frontDisplayNum = 0;
    public int rightDisplayNum = 1;
    public int backDisplayNum = 2;
    public int leftDisplayNum = 3;
    public Vector3 cameraRotation = Vector3.zero;

    [Header("Generic timing parameters")]
    // the time in seconds that the stimulus will run for until it waits for
    // further input from python
    public float experimentDuration = 60f;
    public float darkAdaptTime = 5f;
    float timeSinceDarkAdaptStart = 0f;

    [Header("Generic Saving parameters")]
    public bool recordFrameData = true;
    public bool recordEachFrame = true;
    public float recordingFrequency = 1f; // in seconds if recordEachFrame is false
    public float frameDataIdCode = 9999f; // a code to identify the frame data recording
    public float animalCode = 1f; // a code to identify the animal

    [Header("Generic SyncSquare parameters")]
    public Color syncSquareColor = Color.red;
    public SyncSquare syncSquare;
    public int syncSquareDisplayNum = 0;
    public bool displayStimulusCode = false;
    public float flickerDuration = 0.1f; // time sphere renderer is off in seconds

    [Header("Generic Components")]
    public CameraMonitorController camMon;
    public FrameWriter frameWriter;
    public EpisodeControllerAgent episodeController; // for controlling when a stimulus has finished and a new one should be loaded
    public GameObject blackOutCanvases;

    [Header("Generic manual control parameters")]
    public bool manualControl = true;
    public float mouseMoveSpeed = 2f;


    void Start() {
        if (recieveParametersFromPython) {
            print("Recieving parameters from python. If this is not desired (e.g. you are testing in the editor), set recieveParametersFromPython to false on your Stimulus Manager.");
        }
    }


    public virtual void Reset() {
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
        timeSinceDarkAdaptStart = 0f;

        SocketMovementController socketMovementController = GameObject.FindObjectOfType<SocketMovementController>();
        socketMovementController.recieveInputFromSocket = recieveInputFromSocket;
        socketMovementController.Reset();
    }

    protected virtual void GetPropertiesFromPython() {
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
        manualControl = floatChannel.GetWithDefault("manualControl", 1f) != 0;
        mouseMoveSpeed = floatChannel.GetWithDefault("mouseMoveSpeed", 2f);
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
        float y = floatChannel.GetWithDefault("cameraRotationX", 0f);
        float z = floatChannel.GetWithDefault("cameraRotationX", 0f);
        cameraRotation = new Vector3(x, y, z);
        darkAdaptTime = floatChannel.GetWithDefault("darkAdaptTime", 5f);
        recieveInputFromSocket = floatChannel.GetWithDefault("fictracFeedback", 0f) != 0;
    }

    void Update() {
        if (timeSinceDarkAdaptStart < darkAdaptTime) {
            timeSinceDarkAdaptStart += Time.deltaTime;
        } else {
            blackOutCanvases.SetActive(false);
        }
    }

    // should be overridden by child classes
    public abstract void SetupStimuli();
}