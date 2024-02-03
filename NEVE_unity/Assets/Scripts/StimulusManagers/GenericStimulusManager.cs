using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.IO;
using UnityEngine.SceneManagement;

public abstract class GenericStimulusManager : MonoBehaviour
{
    // A generic class for managing stimuli


    public bool recieveInputFromSocket = false;
    public bool startFictracFromStart = false;

    public bool mustIncludeEveryParameter = false;
    public bool use32BitColor = false;

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
    public Vector2 syncSquarePos = new Vector2(-29.84f, 18.17102f);
    public float syncSquareScalar = 1f;
    public Color syncSquareColor = Color.red;
    public Color syncSquareWaitingColor = Color.black;
    public Color syncSquareStartedColor = new Color(0.7f, 0.2f, 0.2f);
    public Color syncSquareEndedColor = new Color(0.4f, 0.1f, 0.1f);
    public Color syncSquareTextColor = Color.white;
    public SyncSquare syncSquare;
    public int syncSquareDisplayNum = 0;
    public bool displayStimulusCode = true;
    public float flickerDuration = 0.1f; // time sphere renderer is off in seconds
    public bool flashingSyncSquare = false;
    public float flashingSyncSquareFrequency = 1;
    // use milliseconds instead of frames
    public bool flashingSyncSquareUseMS = true;

    [Header("Generic Components")]
    public CameraMonitorController camMon;
    public FrameWriter frameWriter;
    public EpisodeControllerAgent episodeController; // for controlling when a stimulus has finished and a new one should be loaded
    public GameObject blackOutCanvases;

    [Header("Generic manual control parameters")]
    public bool manualControl = false;
    public float mouseMoveSpeed = 2f;

    public EnvironmentParameters floatChannel;

    public List<string> parametersExpected = new List<string>();

    public GameObject errorMessageObject;
    Text errorText;

    public float xMultiplier = 1f;
    public float zMultiplier = 1f;

    public bool darkAdaptFirstTrialOnly = false;
    public bool darkAdaptNow = false;
    public int trialNumber = 0;


    public virtual void Reset() {
        blackOutCanvases.SetActive(true);

        GetPropertiesFromPython();
        SetupStimuli();

        episodeController.experimentDuration = experimentDuration;

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
        frameWriter.flashingSyncSquare = flashingSyncSquare;
        frameWriter.flashingSyncFrequency = flashingSyncSquareFrequency;
        // note, flashing sync squares with frames instead of milliseconds really only make sense if
        // v-sync is on, as unity will otherwise usually run faster than the monitor refresh rate
        frameWriter.flashingSyncSquareUseMS = flashingSyncSquareUseMS;
        frameWriter.syncSquareWaitingColor = syncSquareWaitingColor;
        frameWriter.syncSquareStartedColor = syncSquareStartedColor;
        frameWriter.syncSquareEndedColor = syncSquareEndedColor;
        frameWriter.Reset();
        syncSquare.transform.parent.GetComponent<Canvas>().targetDisplay = syncSquareDisplayNum;
        syncSquare.flickerDuration = flickerDuration;
        syncSquare.flickerColor = syncSquareColor;
        syncSquare.textColor = syncSquareTextColor;
        syncSquare.displayStimulusCode = displayStimulusCode;
        syncSquare.stimulusCode = frameDataIdCode;
        syncSquare.animalCode = animalCode;
        RectTransform syncSquareRect = syncSquare.GetComponent<RectTransform>();
        syncSquareRect.anchoredPosition = syncSquarePos;
        syncSquare.transform.parent.GetComponent<CanvasScaler>().scaleFactor = syncSquareScalar;
        syncSquare.Reset();
        timeSinceDarkAdaptStart = 0f;

        SocketMovementController socketMovementController = GameObject.FindObjectOfType<SocketMovementController>();
        socketMovementController.recieveInputFromSocket = recieveInputFromSocket;
        socketMovementController.startFictracFromStart = startFictracFromStart;
        socketMovementController.waitTimeBeforeStartMovement = GetFloatFromPython("delayToApproach", 5f);
        socketMovementController.xMultiplier = xMultiplier;
        socketMovementController.zMultiplier = zMultiplier;
        socketMovementController.minMovementDistance = GetFloatFromPython("minMovementDistance", 0.1f);
        socketMovementController.maxDistanceDelta = GetFloatFromPython("maxDistanceDelta", 80f);
        socketMovementController.Reset();

        string lutPath = "LUTs/lut.png";
        int attempts = 0;
        while (true)
        {
            // search for the lut directory (changes depending on platform or if in editor)
            print("Searching for lut path at: " + lutPath);
            if (File.Exists(lutPath)) {
                SetLUT(lutPath);
                break;
            }
            lutPath = "../" + lutPath;
            attempts += 1;
            if (attempts > 10) {
                RaiseError("Could not find the lookup texture (LUT). Place this texture at the following path: LUTs/lut.png");
                break;
            }
        }

        CheckParameters();
        trialNumber += 1;
    }

    void CheckParameters() {
        IList<string> parametersReceived = floatChannel.Keys();

        string errorBeg = "<b>The following errors were found in your config file:</b>\n\n";
        string errorMessage = "";
        string errorEnd = "\n<b>Update your config file!\nSee github.com/jakemanger/NEVE/docs/configs_guide.md\nfor a guide on how to write config files.</b>\n";

        // foreach (string parameter in parametersReceived) {
        //     if (!parametersExpected.Contains(parameter)) {
        //         errorMessage = errorMessage + "Unknown parameter: " + parameter + "\n";
        //     }
        // }

        if (mustIncludeEveryParameter) {
            foreach (string parameter in parametersExpected) {
                if (!parametersReceived.Contains(parameter)) {
                    errorMessage = errorMessage + "Missing parameter: " + parameter + "\n";
                }
            }
        }

        Scene currentScene = SceneManager.GetActiveScene();
        string sceneError = (
            "<b>If these variables do not match those defined for this experiment, ensure you have the correct scene parameter.</b>\n"
            + "The current scene parameter is " + currentScene.buildIndex + ": " + currentScene.name + "\n"
            + "Scene parameter options are: "
        );
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            sceneError = sceneError + ", " + i + ": " + SceneUtility.GetScenePathByBuildIndex(i);
        }
        sceneError += "\n";

        if (errorMessage != "") {
            RaiseError(errorBeg + errorMessage + sceneError + errorEnd);
        }
    }

    public void RaiseError(string text) {
        if (errorMessageObject != null) {
            errorMessageObject = Instantiate(errorMessageObject, Vector3.zero, Quaternion.identity);
            errorMessageObject.SetActive(true);
            errorText = errorMessageObject.transform.GetChild(0).GetChild(1).GetComponent<Text>();
            errorText.text = text;
        } else {
            print("Could not find errorMessageObject, so printing error to the console.");
            print(text);
        }
    }

    protected virtual void GetPropertiesFromPython() {
        // load properties from python
        floatChannel = Academy.Instance.EnvironmentParameters;

        int scene = GetIntFromPython("scene", 0);  // just check that it is there

        print("Recieved " + floatChannel.Keys().Count + " properties from python:");
        foreach (string key in floatChannel.Keys()) {
            float value = floatChannel.GetWithDefault(key, 0f);
            print(key + ": " + value);
        }

        // set properties from python
        use32BitColor = GetBoolFromPython("use32BitColor", false); // needs to be before any colors are set
        eyeHeight = GetFloatFromPython("eyeHeight", 2f);
        distanceToMonitors = GetFloatFromPython("distanceToMonitors", 7f);
        monitorDimensions = GetVector2FromPython("monitorDimensions", new Vector2(12.176f, 6.87f));
        flickerDuration = GetFloatFromPython("flickerDuration", 0.1f);
        syncSquareColor = GetColorFromPython("syncSquareColour", Color.red);
        syncSquareWaitingColor = GetColorFromPython("syncSquareWaitingColour", Color.black);
        syncSquareStartedColor = GetColorFromPython("syncSquareStartedColour", new Color(0.7f, 0.2f, 0.2f));
        syncSquareEndedColor = GetColorFromPython("syncSquareEndedColour", new Color(0.4f, 0.1f, 0.1f));
        syncSquareTextColor = GetColorFromPython("syncSquareTextColour", new Color(0.3f, 0.1f, 0.1f));
        syncSquareDisplayNum = GetIntFromPython("syncSquareDisplayNum", 0);
        syncSquarePos = GetVector2FromPython("syncSquarePos", new Vector2(-29.84f, 18.17102f));
        syncSquareScalar = GetFloatFromPython("syncSquareScalar", 1f);
        flashingSyncSquare = GetBoolFromPython("flashingSyncSquare", false);
        flashingSyncSquareFrequency = GetFloatFromPython("flashingSyncSquareFrequency", 100f);
        flashingSyncSquareUseMS = GetBoolFromPython("flashingSyncSquareUseMS", true);
        displayStimulusCode = GetBoolFromPython("displayStimulusCode", true);
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
        recieveInputFromSocket = GetBoolFromPython("fictracFeedback", true);
        startFictracFromStart = GetBoolFromPython("startFictracFromStart", false);
        mustIncludeEveryParameter = GetBoolFromPython("mustIncludeEveryParameter", false);
        xMultiplier = GetFloatFromPython("xMultiplier", 1f);
        zMultiplier = GetFloatFromPython("zMultiplier", 1f);
        darkAdaptFirstTrialOnly = GetBoolFromPython("darkAdaptFirstTrialOnly", true);
        darkAdaptNow = GetBoolFromPython("darkAdaptNow", false);
    }

    public void Update() {
        if (darkAdaptFirstTrialOnly && (trialNumber < 2 || darkAdaptNow)) {
            if (timeSinceDarkAdaptStart < darkAdaptTime) {
                timeSinceDarkAdaptStart += Time.deltaTime;
            } else {
                blackOutCanvases.SetActive(false);
            }
        } else if (!darkAdaptFirstTrialOnly && !darkAdaptNow) {
            if (timeSinceDarkAdaptStart < darkAdaptTime) {
                timeSinceDarkAdaptStart += Time.deltaTime;
            } else {
                blackOutCanvases.SetActive(false);
            }
        } else {
            blackOutCanvases.SetActive(false);
        }
    }

    // should be overridden by child classes
    public abstract void SetupStimuli();


    // some helper classes
    public float GetFloatFromPython(string parameterName, float defaultValue, string extraSuffix = "") {
        string name = parameterName + extraSuffix;
        parametersExpected.Add(name);
        return floatChannel.GetWithDefault(name, defaultValue);
    }

    public int GetIntFromPython(string parameterName, int defaultValue, string extraSuffix = "") {
        string name = parameterName + extraSuffix;
        parametersExpected.Add(name);
        return (int)floatChannel.GetWithDefault(name, (float)defaultValue);
    }

    public Vector2 GetVector2FromPython(string parameterName, Vector2 defaultValue, string extraSuffix = "") {
        string nameX = parameterName + "X" + extraSuffix;
        string nameY = parameterName + "Y" + extraSuffix;
        parametersExpected.Add(nameX);
        parametersExpected.Add(nameY);
        return new Vector2(
            floatChannel.GetWithDefault(nameX, defaultValue.x),
            floatChannel.GetWithDefault(nameY, defaultValue.y)
        );
    }

    public Vector3 GetVector3FromPython(string parameterName, Vector3 defaultValue, string extraSuffix = "") {
        string nameX = parameterName + "X" + extraSuffix;
        string nameY = parameterName + "Y" + extraSuffix;
        string nameZ = parameterName + "Z" + extraSuffix;
        parametersExpected.Add(nameX);
        parametersExpected.Add(nameY);
        parametersExpected.Add(nameZ);
        return new Vector3(
            floatChannel.GetWithDefault(nameX, defaultValue.x),
            floatChannel.GetWithDefault(nameY, defaultValue.y),
            floatChannel.GetWithDefault(nameZ, defaultValue.z)
        );
    }

    public Color GetColorFromPython(string parameterName, Color defaultValue, string extraSuffix = "") {
        string nameR = parameterName + "R" + extraSuffix;
        string nameG = parameterName + "G" + extraSuffix;
        string nameB = parameterName + "B" + extraSuffix;
        string nameA = parameterName + "A" + extraSuffix;
        parametersExpected.Add(nameR);
        parametersExpected.Add(nameG);
        parametersExpected.Add(nameB);
        parametersExpected.Add(nameA);

        if (use32BitColor) {
            return new Color32(
                (byte)floatChannel.GetWithDefault(nameR, defaultValue.r * 255f),
                (byte)floatChannel.GetWithDefault(nameG, defaultValue.g * 255f),
                (byte)floatChannel.GetWithDefault(nameB, defaultValue.b * 255f),
                (byte)floatChannel.GetWithDefault(nameA, defaultValue.a * 255f)
            );
        }

        return new Color(
            floatChannel.GetWithDefault(nameR, defaultValue.r),
            floatChannel.GetWithDefault(nameG, defaultValue.g),
            floatChannel.GetWithDefault(nameB, defaultValue.b),
            floatChannel.GetWithDefault(nameA, defaultValue.a)
        );
    }

    public bool GetBoolFromPython(string parameterName, bool defaultValue, string extraSuffix = "") {
        string name = parameterName + extraSuffix;
        parametersExpected.Add(name);
        return floatChannel.GetWithDefault(name, defaultValue ? 1f : 0f) != 0f;
    }

    public void SetLUT(string lutPath) {
        // set the lut of the color lookup on lutVolume
        UnityEngine.Rendering.VolumeProfile volumeProfile = transform.GetChild(0).GetComponent<UnityEngine.Rendering.Volume>()?.profile;
        if(!volumeProfile) throw new System.NullReferenceException(nameof(UnityEngine.Rendering.VolumeProfile));

        UnityEngine.Rendering.Universal.ColorLookup colorLookup;
        if(!volumeProfile.TryGet(out colorLookup)) throw new System.NullReferenceException(nameof(colorLookup));

        // create a RGB8 Unorm texture from the LUT file
        Texture2D texture = new Texture2D(1024, 32, TextureFormat.RGBA32, false, true);
        texture.wrapMode = TextureWrapMode.Clamp;

        texture.LoadImage(File.ReadAllBytes(lutPath));
        
        colorLookup.texture.Override(texture);
    }
}