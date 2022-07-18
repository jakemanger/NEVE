using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;

public class MimicExpansionSpeedDualLoomManager : DualLoomManager
{
    // A class for controlling looming stimuli where you want to match expansion speed
    [Header("Mimic expansion speed parameters")]
    public bool mimicExpansionSpeed1 = false;
    public int mimicExpansionSpeedMethod1 = 0;
    public float referenceInitialDistance1 = 2f;
    public float referenceEndDistance1 = 2f;
    public float referenceSpeed1 = 1f;
    public float equalDistance1 = 1f;
    public float referenceDiameter1 = 1f;
    public float moveTime1 = 1f;
    public Vector2 referenceStartPolarPosition1 = Vector2.zero;
    public Vector2 referenceEndPolarPosition1 = Vector2.zero;
    
    public bool mimicExpansionSpeed2 = false;
    public int mimicExpansionSpeedMethod2 = 0;
    public float referenceInitialDistance2 = 2f;
    public float referenceEndDistance2 = 2f;
    public float referenceSpeed2 = 1f;
    public float equalDistance2 = 1f;
    public float referenceDiameter2 = 1f;
    public float moveTime2 = 1f;
    public Vector2 referenceStartPolarPosition2 = Vector2.zero;
    public Vector2 referenceEndPolarPosition2 = Vector2.zero;
    
   // public float timeElapsed=0f;

    //AAA: Add public params here for new input

    protected override void GetPropertiesFromPython() {
        base.GetPropertiesFromPython();

        mimicExpansionSpeed1 = GetBoolFromPython("mimicExpansionSpeed", false, "1");
        mimicExpansionSpeedMethod1 = GetIntFromPython("mimicExpansionSpeedMethod", 0, "1");
        referenceInitialDistance1 = GetFloatFromPython("referenceInitialDistance", 1f, "1");
        referenceEndDistance1 = GetFloatFromPython("referenceEndDistance", 1f, "1");
        referenceSpeed1 = GetFloatFromPython("referenceSpeed", 1f, "1");
        referenceDiameter1 = GetFloatFromPython("referenceDiameter", 1f, "1");
        equalDistance1 = GetFloatFromPython("equalDistance", 1f, "1");
        moveTime1 = GetFloatFromPython("moveTime", 1f, "1");
        referenceStartPolarPosition1.x = -1 * GetFloatFromPython("referenceStartElevation", 0f, "1");
        referenceStartPolarPosition1.y = GetFloatFromPython("referenceStartAzimuth", 0f, "1");
        referenceEndPolarPosition1.x = -1 * GetFloatFromPython("referenceEndElevation", 0f, "1");
        referenceEndPolarPosition1.y = GetFloatFromPython("referenceEndAzimuth", 0f, "1");

        mimicExpansionSpeed2 = GetBoolFromPython("mimicExpansionSpeed", false, "2");
        mimicExpansionSpeedMethod2 = GetIntFromPython("mimicExpansionSpeedMethod", 0, "2");
        referenceInitialDistance2 = GetFloatFromPython("referenceInitialDistance", 1f, "2");
        referenceEndDistance2 = GetFloatFromPython("referenceEndDistance", 1f, "2");
        referenceSpeed2 = GetFloatFromPython("referenceSpeed", 1f, "2");
        referenceDiameter2 = GetFloatFromPython("referenceDiameter", 1f, "2");
        equalDistance2 = GetFloatFromPython("equalDistance", 1f, "2");
        moveTime2 = GetFloatFromPython("moveTime", 1f, "2");
        referenceStartPolarPosition2.x = -1 * GetFloatFromPython("referenceStartElevation", 0f, "2");
        referenceStartPolarPosition2.y = GetFloatFromPython("referenceStartAzimuth", 0f, "2");
        referenceEndPolarPosition2.x = -1 * GetFloatFromPython("referenceEndElevation", 0f, "2");
        referenceEndPolarPosition2.y = GetFloatFromPython("referenceEndAzimuth", 0f, "2");

        //AAA: Read variables from yaml here (like above)
    }

    public override void SetupStimuli() {
        base.SetupStimuli();

        SphericalStimulusGenerator[] stimGenerators = GameObject.FindObjectsOfType<SphericalStimulusGenerator>();
        SphericalStimulusGenerator stimGenerator1 = stimGenerators[0];

        // addition for zahra's matching of expansion speed equations
        stimGenerator1.mimicExpansionSpeed = mimicExpansionSpeed1;
        stimGenerator1.mimicExpansionSpeedMethod = mimicExpansionSpeedMethod1;
        stimGenerator1.referenceInitialDistance = referenceInitialDistance1;
        stimGenerator1.referenceEndDistance = referenceEndDistance1;
        stimGenerator1.referenceSpeed = referenceSpeed1;
        stimGenerator1.equalDistance = equalDistance1;
        stimGenerator1.referenceDiameter = referenceDiameter1;
        stimGenerator1.moveTime = moveTime1;
        stimGenerator1.referenceStartPolarPosition = referenceStartPolarPosition1;
        stimGenerator1.referenceEndPolarPosition = referenceEndPolarPosition1;

        stimGenerator1.Reset();

        // stimulus 2
        if (stimGenerators.Length > 1) {
            SphericalStimulusGenerator stimGenerator2 = stimGenerators[1];
            stimGenerator2.mimicExpansionSpeed = mimicExpansionSpeed2;
            stimGenerator2.mimicExpansionSpeedMethod = mimicExpansionSpeedMethod2;
            stimGenerator2.referenceInitialDistance = referenceInitialDistance2;
            stimGenerator2.referenceEndDistance = referenceEndDistance2;
            stimGenerator2.referenceSpeed = referenceSpeed2;
            stimGenerator2.equalDistance = equalDistance2;
            stimGenerator2.referenceDiameter = referenceDiameter2;
            stimGenerator2.moveTime = moveTime2;
            stimGenerator2.referenceStartPolarPosition = referenceStartPolarPosition2;
            stimGenerator2.referenceEndPolarPosition = referenceEndPolarPosition2;
            stimGenerator2.Reset();
        }
        
        //AAA: Assign public variable to stimulus
    }
}
