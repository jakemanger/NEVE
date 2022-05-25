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
    public float darkAdaptTime = 1f;
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
    public bool displayStimulusCode = true;
    public float flickerDuration = 0.1f; // time sphere renderer is off in seconds

    [Header("Generic Components")]
    public CameraMonitorController camMon;
    public FrameWriter frameWriter;
    public EpisodeControllerAgent episodeController; // for controlling when a stimulus has finished and a new one should be loaded
    public GameObject blackOutCanvases;

    [Header("Generic manual control parameters")]
    public bool manualControl = false;
    public float mouseMoveSpeed = 2f;

    public EnvironmentParameters floatChannel;


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
        floatChannel = Academy.Instance.EnvironmentParameters;

        // print("Recieved " + floatChannel.Keys().Count + " properties from python:");
        // foreach (string key in floatChannel.Keys()) {
        //     float value = floatChannel.GetWithDefault(key, 0f);
        //     print(key + ": " + value);
        // }

        // set properties from python
        eyeHeight = GetFloatFromPython("eyeHeight", 2f);
        distanceToMonitors = GetFloatFromPython("distanceToMonitors", 7f);
        monitorDimensions = GetVector2FromPython("monitorDimensions", new Vector2(12.176f, 6.87f));
        flickerDuration = GetFloatFromPython("flickerDuration", 0.1f);
        syncSquareColor = GetColorFromPython("syncSquareColor", Color.red);
        syncSquareDisplayNum = GetIntFromPython("syncSquareDisplayNum", 0);
        displayStimulusCode = GetBoolFromPython("displayStimulusCode", false);
        manualControl = GetBoolFromPython("manualControl", false);
        mouseMoveSpeed = GetFloatFromPython("mouseMoveSpeed", 1f);
        experimentDuration = GetFloatFromPython("experimentDuration", 60f);
        recordFrameData = GetBoolFromPython("recordFrameData", true);
        recordEachFrame = GetBoolFromPython("recordEachFrame", true);
        recordingFrequency = GetFloatFromPython("recordingFrequency", 1f);
        frameDataIdCode = GetFloatFromPython("frameDataIdCode", 9999f);
        animalCode = GetFloatFromPython("animalCode", 1f);
        frontDisplayNum = GetIntFromPython("frontDisplayNum", 0);
        rightDisplayNum = GetIntFromPython("rightDisplayNum", 1);
        backDisplayNum = GetIntFromPython("backDisplayNum", 2);
        leftDisplayNum = GetIntFromPython("leftDisplayNum", 3);
        cameraRotation = GetVector3FromPython("cameraRotation", Vector3.zero);
        darkAdaptTime = GetFloatFromPython("darkAdaptTime", 0f);
        recieveInputFromSocket = GetBoolFromPython("fictracFeedback", false);
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


    // some helper classes
    public float GetFloatFromPython(string parameterName, float defaultValue, string extraSuffix = "") {
        return floatChannel.GetWithDefault(parameterName + extraSuffix, defaultValue);
    }

    public int GetIntFromPython(string parameterName, int defaultValue, string extraSuffix = "") {
        return (int)floatChannel.GetWithDefault(parameterName + extraSuffix, (float)defaultValue);
    }

    public Vector2 GetVector2FromPython(string parameterName, Vector2 defaultValue, string extraSuffix = "") {
        return new Vector2(
            floatChannel.GetWithDefault(parameterName + "X" + extraSuffix, defaultValue.x),
            floatChannel.GetWithDefault(parameterName + "Y" + extraSuffix, defaultValue.y)
        );
    }

    public Vector3 GetVector3FromPython(string parameterName, Vector3 defaultValue, string extraSuffix = "") {
        return new Vector3(
            floatChannel.GetWithDefault(parameterName + "X" + extraSuffix, defaultValue.x),
            floatChannel.GetWithDefault(parameterName + "Y" + extraSuffix, defaultValue.y),
            floatChannel.GetWithDefault(parameterName + "Z" + extraSuffix, defaultValue.z)
        );
    }

    public Color GetColorFromPython(string parameterName, Color defaultValue, string extraSuffix = "") {
        return new Color(
            floatChannel.GetWithDefault(parameterName + "R" + extraSuffix, defaultValue.r),
            floatChannel.GetWithDefault(parameterName + "G" + extraSuffix, defaultValue.g),
            floatChannel.GetWithDefault(parameterName + "B" + extraSuffix, defaultValue.b),
            floatChannel.GetWithDefault(parameterName + "A" + extraSuffix, defaultValue.a)
        );
    }

    public bool GetBoolFromPython(string parameterName, bool defaultValue, string extraSuffix = "") {
        return floatChannel.GetWithDefault(parameterName + extraSuffix, defaultValue ? 1f : 0f) != 0f;
    }
}