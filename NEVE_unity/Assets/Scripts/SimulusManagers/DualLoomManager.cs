using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;

public class DualLoomManager : GenericStimulusManager
{
    [Header("Specific background stimulus parameters")]
    public float horizonHeight = 0f;
    public Color aboveHorizonColour = Color.grey;
    public Color belowHorizonColour = Color.white;

    [Header("Specific stimulus parameters")]

    public Vector3 startScale1 = Vector3.one;
    public Vector3 endScale1 = Vector3.one;
    public Vector3 startScale2 = Vector3.one;
    public Vector3 endScale2 = Vector3.one;
    public Vector2 startPolarPosition1 = new Vector2(0f, 0f);
    public Vector2 startPolarPosition2 = new Vector2(0f, 0f);
    public Vector2 endPolarPosition1 = new Vector2(0f, 0f);
    public Vector2 endPolarPosition2 = new Vector2(0f, 0f);
    public Vector3 targetLocationOffset1 = new Vector3(0f, 0f, 0f);
    public Vector3 targetLocationOffset2 = new Vector3(0f, 0f, 0f);
    public float startOffset1 = 10f;
    public float startOffset2 = 10f;
    public float endOffset1 = 10f;
    public float endOffset2 = 10f;
    public float delayToApproach1 = 5f;
    public float delayToApproach2 = 5f;
    public float numReps1 = 2;
    public float numReps2 = 2;
    public Color stimulusColour1 = Color.white;
    public Color stimulusColour2 = Color.white;
    public float stimulusDuration1 = 5f;
    public float stimulusDuration2 = 5f;
    public int stimulusType1 = 0;
    public bool drawOutline1 = false;
    public float outlineWidth1 = 5f;
    public Color outlineColor1 = Color.black;
    public int stimulusType2 = 0;
    public bool drawOutline2 = false;
    public float outlineWidth2 = 5f;
    public Color outlineColor2 = Color.black;
    public float gratingNum = 100f; // only used if the stimulusType has a grating material
    public bool gratingIsSquare = false;
    public float gratingMaxIntensity = 0.1f;
    public float gratingMinIntensity = 0f;

    public bool fixedAngularSize1 = false;
    public bool fixXAxis1 = true; // otherwise fix the Y axis
    public float minAngularAngle1 = -30f;
    public float maxAngularAngle1 = 30f;

    public bool fixedAngularSize2 = false;
    public bool fixXAxis2 = true; // otherwise fix the Y axis
    public float minAngularAngle2 = -30f;
    public float maxAngularAngle2 = 30f;


    protected override void GetPropertiesFromPython() {
        // get generic stimulus parameters from python
        base.GetPropertiesFromPython();

        // now get those specific to this stimuli
        horizonHeight = floatChannel.GetWithDefault("horizonHeight", horizonHeight);
        aboveHorizonColour = GetColorFromPython("aboveHorizonColour", aboveHorizonColour);
        belowHorizonColour = GetColorFromPython("belowHorizonColour", belowHorizonColour);

        gratingNum = GetFloatFromPython("gratingNum", gratingNum);
        gratingIsSquare = GetBoolFromPython("gratingIsSquare", false);
        gratingMaxIntensity = floatChannel.GetWithDefault("gratingMaxIntensity", gratingMaxIntensity);
        gratingMinIntensity = floatChannel.GetWithDefault("gratingMinIntensity", gratingMinIntensity);

        startScale1 = GetVector3FromPython("startScale", startScale1);
        endScale1 = GetVector3FromPython("endScale", endScale1);
        stimulusDuration1 = GetFloatFromPython("duration", stimulusDuration1, "1");
        startPolarPosition1 = GetVector2FromPython("startPolarPosition", startPolarPosition1, "1");
        endPolarPosition1 = GetVector2FromPython("endPolarPosition", endPolarPosition1, "1");
        targetLocationOffset1 = GetVector3FromPython("targetLocationOffset", targetLocationOffset1, "1");
        startOffset1 = GetFloatFromPython("startOffset", 50f, "1");
        endOffset1 = GetFloatFromPython("endOffset", 1f, "1");
        delayToApproach1 = GetFloatFromPython("delayToApproach", 5f, "1");
        numReps1 = GetFloatFromPython("numReps", 1f, "1");
        stimulusColour1 = GetColorFromPython("stimulusColour", stimulusColour1, "1");
        stimulusType1 = GetIntFromPython("stimulusType", 0, "1");
        drawOutline1 = GetBoolFromPython("drawOutline", false, "1");
        outlineWidth1 = GetFloatFromPython("outlineWidth", 5f, "1");
        outlineColor1 = GetColorFromPython("outlineColour", outlineColor1, "1");
        fixedAngularSize1 = GetBoolFromPython("fixedAngularSize", false, "1");
        fixXAxis1 = GetBoolFromPython("fixXAxis", false, "1"); // otherwise fix the Y axis
        minAngularAngle1 = GetFloatFromPython("minAngularAngle", -30f, "1");
        maxAngularAngle1 = GetFloatFromPython("maxAngularAngle", 30f, "1");

        startScale2 = GetVector3FromPython("startScale", startScale2, "2");
        endScale2 = GetVector3FromPython("endScale", endScale2, "2");
        stimulusDuration2 = GetFloatFromPython("duration", 5f, "2");
        startPolarPosition2 = GetVector2FromPython("startPolarPosition", startPolarPosition2, "2");
        endPolarPosition2 = GetVector2FromPython("endPolarPosition", endPolarPosition2, "2");
        targetLocationOffset2 = GetVector3FromPython("targetLocationOffset", targetLocationOffset2, "2");
        startOffset2 = GetFloatFromPython("startOffset", 50f, "2");
        endOffset2 = GetFloatFromPython("endOffset", 1f, "2");
        delayToApproach2 = GetFloatFromPython("delayToApproach", 5f, "2");
        numReps2 = GetFloatFromPython("numReps", 1f, "2");
        stimulusColour2 = GetColorFromPython("stimulusColour", stimulusColour2, "2");
        stimulusType2 = GetIntFromPython("stimulusType", 0, "2"); // 0 = icosphere, 1 = unity cube
        drawOutline2 = GetBoolFromPython("drawOutline", false, "2");
        outlineWidth2 = GetFloatFromPython("outlineWidth", 5f, "2");
        outlineColor2 = GetColorFromPython("outlineColour", outlineColor2, "2");
        fixedAngularSize2 = GetBoolFromPython("fixedAngularSize", false, "2");
        fixXAxis2 = GetBoolFromPython("fixXAxis", false, "2"); // otherwise fix the Y axis
        minAngularAngle2 = GetFloatFromPython("minAngularAngle", -30f, "2");
        maxAngularAngle2 = GetFloatFromPython("maxAngularAngle", 30f, "2");
    }

    public override void SetupStimuli() {
        // skybox
        Material mat = RenderSettings.skybox;
        mat.SetFloat("_horizonHeight", horizonHeight);
        mat.SetColor("_aboveHorizonColour", aboveHorizonColour);
        mat.SetColor("_belowHorizonColour", belowHorizonColour);
        RenderSettings.skybox = mat;

        SphericalStimulusGenerator[] stimGenerators = GameObject.FindObjectsOfType<SphericalStimulusGenerator>();
        SphericalStimulusGenerator stimGenerator1 = stimGenerators[0];

        // stimulus 1
        stimGenerator1.flickerDuration = flickerDuration;
        stimGenerator1.stimulusColour = stimulusColour1;
        stimGenerator1.startScale = startScale1;
        stimGenerator1.endScale = endScale1;
        stimGenerator1.startOffset = startOffset1;
        stimGenerator1.endOffset = endOffset1;
        stimGenerator1.delayToApproach = delayToApproach1;
        stimGenerator1.targetLocationOffset = targetLocationOffset1;
        stimGenerator1.startPolarPosition = startPolarPosition1;
        stimGenerator1.endPolarPosition = endPolarPosition1;
        stimGenerator1.numReps = numReps1;
        stimGenerator1.duration = stimulusDuration1; 
        stimGenerator1.stimulusType = stimulusType1;
        stimGenerator1.drawOutline = drawOutline1;
        stimGenerator1.outlineWidth = outlineWidth1;
        stimGenerator1.outlineColor = outlineColor1;
        stimGenerator1.gratingNum = gratingNum;
        stimGenerator1.gratingIsSquare = gratingIsSquare ? 1 : 0;
        stimGenerator1.gratingMaxIntensity = gratingMaxIntensity;
        stimGenerator1.gratingMinIntensity = gratingMinIntensity;
        stimGenerator1.fixedAngularSize = fixedAngularSize1;
        stimGenerator1.fixXAxis = fixXAxis1; // otherwise fix the Y axis
        stimGenerator1.minAngularAngle = minAngularAngle1;
        stimGenerator1.maxAngularAngle = maxAngularAngle1;
        stimGenerator1.manualControl = manualControl;
        stimGenerator1.mouseMoveSpeed = mouseMoveSpeed;

        stimGenerator1.Reset();

        // stimulus 2
        if (stimGenerators.Length > 1) {
            SphericalStimulusGenerator stimGenerator2 = stimGenerators[1];
            stimGenerator2.flickerDuration = flickerDuration;
            stimGenerator2.stimulusColour = stimulusColour2;
            stimGenerator2.startScale = startScale2;
            stimGenerator2.endScale = endScale2;
            stimGenerator2.startOffset = startOffset2;
            stimGenerator2.endOffset = endOffset2;
            stimGenerator2.delayToApproach = delayToApproach2;
            stimGenerator2.targetLocationOffset = targetLocationOffset2;
            stimGenerator2.startPolarPosition = startPolarPosition2;
            stimGenerator2.endPolarPosition = endPolarPosition2;
            stimGenerator2.numReps = numReps2;
            stimGenerator2.duration = stimulusDuration2; 
            stimGenerator2.stimulusType = stimulusType2;
            stimGenerator2.drawOutline = drawOutline2;
            stimGenerator2.outlineWidth = outlineWidth2;
            stimGenerator2.outlineColor = outlineColor2;
            stimGenerator2.gratingNum = gratingNum;
            stimGenerator2.gratingIsSquare = gratingIsSquare ? 1 : 0;
            stimGenerator2.gratingMaxIntensity = gratingMaxIntensity;
            stimGenerator2.gratingMinIntensity = gratingMinIntensity;
            stimGenerator2.fixedAngularSize = fixedAngularSize2;
            stimGenerator2.fixXAxis = fixXAxis2; // otherwise fix the Y axis
            stimGenerator2.minAngularAngle = minAngularAngle2;
            stimGenerator2.maxAngularAngle = maxAngularAngle2;
            stimGenerator2.manualControl = manualControl;
            stimGenerator2.mouseMoveSpeed = mouseMoveSpeed;

            stimGenerator2.Reset();
        }
    }
}