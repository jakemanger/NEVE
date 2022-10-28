using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;

public class MimicExpansionSpeedLoomManager : LoomManager
{
    // A class for controlling looming stimuli where you want to match expansion speed
    [Header("Mimic expansion speed parameters")]
    public bool mimicExpansionSpeed = false;
    public int mimicExpansionSpeedMethod = 0;
    public float referenceInitialDistance = 2f;
    public float referenceEndDistance = 2f;
    public float referenceSpeed = 1f;
    public float equalDistance = 1f;
    public float referenceDiameter = 1f;
    public float moveTime = 1f;
    public Vector2 referenceStartPolarPosition = Vector2.zero;
    public Vector2 referenceEndPolarPosition = Vector2.zero;
    

    //AAA: Add public params here for new input

    protected override void GetPropertiesFromPython() {
        base.GetPropertiesFromPython();

        mimicExpansionSpeed = GetBoolFromPython("mimicExpansionSpeed", false);
        mimicExpansionSpeedMethod = GetIntFromPython("mimicExpansionSpeedMethod", 0);
        referenceInitialDistance = GetFloatFromPython("referenceInitialDistance", 1f);
        referenceEndDistance = GetFloatFromPython("referenceEndDistance", 1f);
        referenceSpeed = GetFloatFromPython("referenceSpeed", 1f);
        referenceDiameter = GetFloatFromPython("referenceDiameter", 1f);
        equalDistance = GetFloatFromPython("equalDistance", 1f);
        moveTime = GetFloatFromPython("moveTime", 1f);
        referenceStartPolarPosition.x = -1 * GetFloatFromPython("referenceStartElevation", 0f);
        referenceStartPolarPosition.y = GetFloatFromPython("referenceStartAzimuth", 0f);
        referenceEndPolarPosition.x = -1 * GetFloatFromPython("referenceEndElevation", 0f);
        referenceEndPolarPosition.y = GetFloatFromPython("referenceEndAzimuth", 0f);

        //AAA: Read variables from yaml here (like above)
    }

    public override void SetupStimuli() {
        base.SetupStimuli();

        // addition for zahra's matching of expansion speed equations
        stimGenerator.mimicExpansionSpeed = mimicExpansionSpeed;
        stimGenerator.mimicExpansionSpeedMethod = mimicExpansionSpeedMethod;
        stimGenerator.referenceInitialDistance = referenceInitialDistance;
        stimGenerator.referenceEndDistance = referenceEndDistance;
        stimGenerator.referenceSpeed = referenceSpeed;
        stimGenerator.equalDistance = equalDistance;
        stimGenerator.referenceDiameter = referenceDiameter;
        stimGenerator.moveTime = moveTime;
        stimGenerator.referenceStartPolarPosition = referenceStartPolarPosition;
        stimGenerator.referenceEndPolarPosition = referenceEndPolarPosition;
        
        //AAA: Assign public variable to stimulus

        stimGenerator.Reset();
    }
}
