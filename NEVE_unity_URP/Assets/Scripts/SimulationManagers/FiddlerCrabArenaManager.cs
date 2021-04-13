using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [Header("Saving parameters")]
    public bool recordExperimentData = true;

    [Header("Components")]
    public BowlStimulusController horizonGround;
    public CameraMonitorController camMon;
    public SphericalStimulusGenerator stimGenerator;
    public FrameWriter frameWriter;

    // use OnEnable as it is executed before stimGenerators Start() function
    // and can restart the stimulus if you disable and enable this gameObject
    void OnEnable() {
       SetupStimuli();
    }
    
    void Start() {
       SetupBelowHorizonStimuli();
       // Setup cameras and above horizon stimuli
       camMon.SetupCams(distanceToMonitors, -crabEyeHeight, monitorDimensions, aboveHorizonColour);
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

        float duration = Mathf.Abs(startOffset - endOffset) / loomingStimulusMoveSpeed;
        stimGenerator.duration = duration; 

        stimGenerator.manualControl = manualControl;
        stimGenerator.mouseMoveSpeed = mouseMoveSpeed;
    }
}
