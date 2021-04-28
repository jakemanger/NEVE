using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;

public class FiddlerCrabArenaManager : MonoBehaviour
{
    [Header("Background parameters")]
    public Color aboveHorizonColour = Color.white;
    public Color belowHorizonColour = Color.grey;
    [Range(-90, 90)]
    public float horizonHeight = 0f; // relative to crab eye height

    [Header("Camera view parameters")]
    public float crabEyeHeight = 4f; // cm vertically relative to bottom of front facing monitors
    public float distanceToMonitors = 28; // cm
    public Vector2 monitorDimensions = new Vector2(51.5f, 32f);

    [Header("Stimulus parameters")]
    public float stimulusSize = 1f;
    public Vector2 stimulusPolarPosition = new Vector2(0f, 0f);
    public Vector3 targetLocationOffset = new Vector3(0f, 0f, 0f);
    public float startOffset = 100f;
    public float endOffset = 1f;
    public float loomingStimulusMoveSpeed = 1f; // units (cm) per second
    public float delayToApproach = 5f;

    public bool manualControl = false;
    public float mouseMoveSpeed = 2f;

    public Color stimlusColour = Color.white;

    // the time in seconds that the stimulus will run for until it waits for
    // further input from python
    public float stimulusDuration = 10f;

    [Header("Saving parameters")]
    public bool recordFrameData = true;
    public bool recordEachFrame = true;
    public float recordingFrequency = 1f; // in seconds if recordEachFrame is false

    [Header("Components")]
    public BowlStimulusController horizonGround;
    public CameraMonitorController camMon;
    public SphericalStimulusGenerator stimGenerator;
    public FrameWriter frameWriter;
    public StimulusAgent stimAgent; // for controlling when a stimulus has finished and a new one should be loaded

    // use OnEnable as it is executed before stimGenerators Start() function
    // and can restart the stimulus if you disable and enable this gameObject
    // void OnEnable() {
    //     Setup();
    // }

    public void Setup() {
        stimAgent.stimulusDuration = stimulusDuration;
        GetPropertiesFromPython();
        SetupStimuli();
        print("Reset");

        // Below horizon stimuli (should be dark contrast to sky)
        SetupBelowHorizonStimuli();

        // Setup cameras and above horizon stimuli
        camMon.SetupCams(distanceToMonitors, -crabEyeHeight, monitorDimensions, aboveHorizonColour);
        
        // Setup frameWriter to write data related to the experiment each frame
        frameWriter.gameObject.SetActive(recordFrameData);
        frameWriter.recordEachFrame = recordFrameData;
        frameWriter.recordingFrequency = recordingFrequency;
    }

    void GetPropertiesFromPython() {
        // load properties from python
        var floatChannel = Academy.Instance.EnvironmentParameters;
        // set properties from python
        float r = floatChannel.GetWithDefault("aboveHorizonColorR", 0.5f);
        float g = floatChannel.GetWithDefault("aboveHorizonColorG", 0.5f);
        float b = floatChannel.GetWithDefault("aboveHorizonColorB", 0.5f);
        float a = floatChannel.GetWithDefault("aboveHorizonColorA", 1f);
        aboveHorizonColour = new Color(r, g, b, a);
    }

    void SetupBelowHorizonStimuli() {
        // spawn ground horizon
        horizonGround.materialColor = belowHorizonColour;
        horizonGround.croppedAngle = horizonHeight;
        horizonGround.CreateBowl();
    }

    void SetupStimuli() {
        stimGenerator.stimulusColour = stimlusColour;
        stimGenerator.stimulusSize = stimulusSize;
        stimGenerator.stimulusPolarPosition = stimulusPolarPosition;
        stimGenerator.startOffset = startOffset;
        stimGenerator.endOffset = endOffset;
        stimGenerator.delayToApproach = delayToApproach;
        stimGenerator.targetLocationOffset = targetLocationOffset;

        float duration = Mathf.Abs(startOffset - endOffset) / loomingStimulusMoveSpeed;
        stimGenerator.duration = duration; 

        stimGenerator.manualControl = manualControl;
        stimGenerator.mouseMoveSpeed = mouseMoveSpeed;
        stimGenerator.Setup();
    }
}
