using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;

public class OLDFiddlerCrabArenaManager : MonoBehaviour
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

    public Color stimulusColour = Color.white;

    // the time in seconds that the stimulus will run for until it waits for
    // further input from python
    public float experimentDuration = 10f;

    [Header("Saving parameters")]
    public bool recordFrameData = true;
    public bool recordEachFrame = true;
    public float recordingFrequency = 1f; // in seconds if recordEachFrame is false

    [Header("Components")]
    public BowlStimulusController horizonGround;
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
        episodeController.experimentDuration = experimentDuration;
        GetPropertiesFromPython();
        SetupStimuli();
        print("Reset");

        // Below horizon stimuli (should be dark contrast to sky)
        SetupBelowHorizonStimuli();

        // Setup cameras and above horizon stimuli
        camMon.SetupCams(distanceToMonitors, -crabEyeHeight, monitorDimensions, true, new Color[] {aboveHorizonColour, aboveHorizonColour, aboveHorizonColour, aboveHorizonColour});
        
        // Setup frameWriter to write data related to the experiment each frame
        frameWriter.gameObject.SetActive(recordFrameData);
        frameWriter.recordEachFrame = recordFrameData;
        frameWriter.recordingFrequency = recordingFrequency;
    }

    void GetPropertiesFromPython() {
        // load properties from python
        var floatChannel = Academy.Instance.EnvironmentParameters;
        // set properties from python
        float r = floatChannel.GetWithDefault("aboveHorizonColourR", 0.5f);
        float g = floatChannel.GetWithDefault("aboveHorizonColourG", 0.5f);
        float b = floatChannel.GetWithDefault("aboveHorizonColourB", 0.5f);
        float a = floatChannel.GetWithDefault("aboveHorizonColourA", 1f);
        aboveHorizonColour = new Color(r, g, b, a);
        r = floatChannel.GetWithDefault("belowHorizonColourR", 0.7f);
        g = floatChannel.GetWithDefault("belowHorizonColourG", 0.7f);
        b = floatChannel.GetWithDefault("belowHorizonColourB", 0.7f);
        a = floatChannel.GetWithDefault("belowHorizonColourA", 1f);
        belowHorizonColour = new Color(r, g, b, a);
        horizonHeight = floatChannel.GetWithDefault("horizonHeight", 0f);
        crabEyeHeight = floatChannel.GetWithDefault("crabEyeHeight", 4f);
        distanceToMonitors = floatChannel.GetWithDefault("distanceToMonitors", 28f);
        float monitorDimensionsX = floatChannel.GetWithDefault("monitorDimensionsX", 51.5f);
        float monitorDimensionsY = floatChannel.GetWithDefault("monitorDimensionsY", 32f);
        monitorDimensions = new Vector2(monitorDimensionsX, monitorDimensionsY);
        stimulusSize = floatChannel.GetWithDefault("stimulusSize", 1f);
        float stimulusPolarPositionX = floatChannel.GetWithDefault("stimulusPolarPositionX", 0f);
        float stimulusPolarPositionY = floatChannel.GetWithDefault("stimulusPolarPositionY", 0f);
        stimulusPolarPosition = new Vector2(stimulusPolarPositionX, stimulusPolarPositionY);
        float targetLocationOffsetX = floatChannel.GetWithDefault("targetLocationOffsetX", 0f);
        float targetLocationOffsetY = floatChannel.GetWithDefault("targetLocationOffsetY", 0f);
        float targetLocationOffsetZ = floatChannel.GetWithDefault("targetLocationOffsetZ", 0f);
        targetLocationOffset = new Vector3(targetLocationOffsetX, targetLocationOffsetY, targetLocationOffsetZ);
        startOffset = floatChannel.GetWithDefault("startOffset", 100f);
        endOffset = floatChannel.GetWithDefault("endOffset", 1f);
        loomingStimulusMoveSpeed = floatChannel.GetWithDefault("loomingStimulusMoveSpeed", 1f);
        delayToApproach = floatChannel.GetWithDefault("delayToApproach", 5f);
        manualControl = floatChannel.GetWithDefault("manualControl", 0f) != 0;
        mouseMoveSpeed = floatChannel.GetWithDefault("mouseMoveSpeed", 2f);
        r = floatChannel.GetWithDefault("stimulusColourR", 1f);
        g = floatChannel.GetWithDefault("stimulusColourG", 1f);
        b = floatChannel.GetWithDefault("stimulusColourB", 1f);
        a = floatChannel.GetWithDefault("stimulusColourA", 1f);
        stimulusColour = new Color(r, g, b, a);
        experimentDuration = floatChannel.GetWithDefault("experimentDuration", 30f);
        recordFrameData = floatChannel.GetWithDefault("recordFrameData", 1f) != 0;
        recordEachFrame = floatChannel.GetWithDefault("recordEachFrame", 1f) != 0;
        recordingFrequency = floatChannel.GetWithDefault("recordingFrequency", 1f);
    }

    void SetupBelowHorizonStimuli() {
        // spawn ground horizon
        horizonGround.materialColor = belowHorizonColour;
        horizonGround.croppedAngle = horizonHeight;
        horizonGround.CreateBowl();
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

        float duration = Mathf.Abs(startOffset - endOffset) / loomingStimulusMoveSpeed;
        stimGenerator.duration = duration; 

        stimGenerator.manualControl = manualControl;
        stimGenerator.mouseMoveSpeed = mouseMoveSpeed;
        stimGenerator.Reset();
    }
}
