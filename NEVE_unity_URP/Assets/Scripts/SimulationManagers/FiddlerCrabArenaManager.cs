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

    [Header("Components")]
    public BowlStimulusController horizonGround;
    public CameraMonitorController camMon;
    
    void Start() {
       SetupBGStimuli();
       camMon.SetupCams(distanceToMonitors, -crabEyeHeight, monitorDimensions, aboveHorizonColour);
    }


    void SetupBGStimuli() {
        // spawn ground horizon
        horizonGround.materialColor = belowHorizonColour;
        horizonGround.croppedAngle = horizonHeight;
        horizonGround.CreateBowl();
    }
}
