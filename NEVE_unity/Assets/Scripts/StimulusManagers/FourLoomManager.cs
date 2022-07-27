using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;

public class FourLoomManager : GenericStimulusManager
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
    public Vector3 startScale3 = Vector3.one;
    public Vector3 endScale3 = Vector3.one;
    public Vector3 startScale4 = Vector3.one;
    public Vector3 endScale4 = Vector3.one;
    public Vector2 startPolarPosition1 = new Vector2(0f, 0f);
    public Vector2 startPolarPosition2 = new Vector2(0f, 0f);
    public Vector2 startPolarPosition3 = new Vector2(0f, 0f);
    public Vector2 startPolarPosition4 = new Vector2(0f, 0f);
    public Vector2 endPolarPosition1 = new Vector2(0f, 0f);
    public Vector2 endPolarPosition2 = new Vector2(0f, 0f);
    public Vector2 endPolarPosition3 = new Vector2(0f, 0f);
    public Vector2 endPolarPosition4 = new Vector2(0f, 0f);
    public Vector3 origin1 = new Vector3(0f, 0f, 0f);
    public Vector3 origin2 = new Vector3(0f, 0f, 0f);
    public Vector3 origin3 = new Vector3(0f, 0f, 0f);
    public Vector3 origin4 = new Vector3(0f, 0f, 0f);
    public Vector2 rotationOffset1 = Vector2.zero;
    public Vector2 rotationOffset2 = Vector2.zero;
    public Vector2 rotationOffset3 = Vector2.zero;
    public Vector2 rotationOffset4 = Vector2.zero;
    public float startDistance1 = 10f;
    public float startDistance2 = 10f;
    public float startDistance3 = 10f;
    public float startDistance4 = 10f;
    public float endDistance1 = 10f;
    public float endDistance2 = 10f;
    public float endDistance3 = 10f;
    public float endDistance4 = 10f;
    public float delayToApproach1 = 5f;
    public float delayToApproach2 = 5f;
    public float delayToApproach3 = 5f;
    public float delayToApproach4 = 5f;
    public float numReps1 = 2;
    public float numReps2 = 2;
    public float numReps3 = 2;
    public float numReps4 = 2;
    public Color stimulusColour1 = Color.white;
    public Color stimulusColour2 = Color.white;
    public Color stimulusColour3 = Color.white;
    public Color stimulusColour4 = Color.white;
    public bool opaqueObject1 = false;
    public bool opaqueObject2 = false;
    public bool opaqueObject3 = false;
    public bool opaqueObject4 = false;
    public float stimulusDuration1 = 5f;
    public float stimulusDuration2 = 5f;
    public float stimulusDuration3 = 5f;
    public float stimulusDuration4 = 5f;
    public int stimulusType1 = 0;
    public bool drawOutline1 = false;
    public float outlineWidth1 = 5f;
    public Color outlineColor1 = Color.black;
    public int outlineType1 = 0; // 0 = world space fixed size, 1 = pixel space fixed size 
    public int stimulusType2 = 0;
    public bool drawOutline2 = false;
    public int outlineType2 = 0;
    public float outlineWidth2 = 5f;
    public Color outlineColor2 = Color.black;
    public int stimulusType3 = 0;
    public bool drawOutline3 = false;
    public int outlineType3 = 0;
    public float outlineWidth3 = 5f;
    public Color outlineColor3 = Color.black;
    public int stimulusType4 = 0;
    public bool drawOutline4 = false;
    public int outlineType4 = 0;
    public float outlineWidth4 = 5f;
    public Color outlineColor4 = Color.black;
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
    public bool fixedAngularSize3 = false;
    public bool fixXAxis3 = true; // otherwise fix the Y axis
    public float minAngularAngle3 = -30f;
    public float maxAngularAngle3 = 30f;

    public bool fixedAngularSize4 = false;
    public bool fixXAxis4 = true; // otherwise fix the Y axis
    public float minAngularAngle4 = -30f;
    public float maxAngularAngle4 = 30f;
    public float delayToAppear1 = 0f;
    public float delayToAppear2 = 0f;
    public float delayToAppear3 = 0f;
    public float delayToAppear4 = 0f;
    public bool directPath1 = true;
    public bool directPath2 = true;
    public bool directPath3 = true;
    public bool directPath4 = true;
    public bool hideAtEnd1 = false;
    public bool hideAtEnd2 = false;
    public bool hideAtEnd3 = false;
    public bool hideAtEnd4 = false;


    protected override void GetPropertiesFromPython() {
        // get generic stimulus parameters from python
        base.GetPropertiesFromPython();

        // now get those specific to this stimuli
        horizonHeight = GetFloatFromPython("horizonHeight", horizonHeight);
        aboveHorizonColour = GetColorFromPython("aboveHorizonColour", aboveHorizonColour);
        belowHorizonColour = GetColorFromPython("belowHorizonColour", belowHorizonColour);

        gratingNum = GetFloatFromPython("gratingNum", gratingNum);
        gratingIsSquare = GetBoolFromPython("gratingIsSquare", false);
        gratingMaxIntensity = GetFloatFromPython("gratingMaxIntensity", gratingMaxIntensity);
        gratingMinIntensity = GetFloatFromPython("gratingMinIntensity", gratingMinIntensity);

        startScale1 = GetVector3FromPython("startScale", startScale1, "1");
        endScale1 = GetVector3FromPython("endScale", endScale1, "1");
        stimulusDuration1 = GetFloatFromPython("duration", stimulusDuration1, "1");
        origin1 = GetVector3FromPython("origin", Vector3.zero, "1");
        rotationOffset1 = GetVector2FromPython("rotationOffset", Vector2.zero, "1");
        startPolarPosition1.x = -1 * GetFloatFromPython("startElevation", 0f, "1");
        startPolarPosition1.y = GetFloatFromPython("startAzimuth", 0f, "1");
        endPolarPosition1.x = -1 * GetFloatFromPython("endElevation", 0f, "1");
        endPolarPosition1.y = GetFloatFromPython("endAzimuth", 0f, "1");
        startDistance1 = GetFloatFromPython("startDistance", 50f, "1");
        endDistance1 = GetFloatFromPython("endDistance", 1f, "1");
        delayToApproach1 = GetFloatFromPython("delayToApproach", 5f, "1");
        numReps1 = GetFloatFromPython("numReps", 1f, "1");
        stimulusColour1 = GetColorFromPython("stimulusColour", stimulusColour1, "1");
        opaqueObject1 = GetBoolFromPython("opaqueObject", false, "1");
        stimulusType1 = GetIntFromPython("stimulusType", 0, "1");
        drawOutline1 = GetBoolFromPython("drawOutline", false, "1");
        outlineWidth1 = GetFloatFromPython("outlineWidth", 5f, "1");
        outlineColor1 = GetColorFromPython("outlineColour", outlineColor1, "1");
        outlineType1 = GetIntFromPython("outlineType", outlineType1, "1");
        fixedAngularSize1 = GetBoolFromPython("fixedAngularSize", false, "1");
        fixXAxis1 = GetBoolFromPython("fixElevation", false, "1"); // otherwise fix the Y axis
        if (fixXAxis1) {
            minAngularAngle1 = -1 * GetFloatFromPython("maxAngularAngle", -30f, "1");
            maxAngularAngle1 = -1 * GetFloatFromPython("minAngularAngle", 30f, "1");
        } else {
            minAngularAngle1 = GetFloatFromPython("minAngularAngle", -30f, "1");
            maxAngularAngle1 = GetFloatFromPython("maxAngularAngle", 30f, "1");
        }
        delayToAppear1 = GetFloatFromPython("delayToAppear", 0f, "1");
        directPath1 = GetBoolFromPython("directPath", true, "1");
        hideAtEnd1 = GetBoolFromPython("hideAtEnd", false, "1");

        startScale2 = GetVector3FromPython("startScale", startScale2, "2");
        endScale2 = GetVector3FromPython("endScale", endScale2, "2");
        stimulusDuration2 = GetFloatFromPython("duration", 5f, "2");
        origin2 = GetVector3FromPython("origin", Vector3.zero, "2");
        rotationOffset2 = GetVector2FromPython("rotationOffset", Vector2.zero, "2");
        startPolarPosition2.x = -1 * GetFloatFromPython("startElevation", 0f, "2");
        startPolarPosition2.y = GetFloatFromPython("startAzimuth", 0f, "2");
        endPolarPosition2.x = -1 * GetFloatFromPython("endElevation", 0f, "2");
        endPolarPosition2.y = GetFloatFromPython("endAzimuth", 0f, "2");
        startDistance2 = GetFloatFromPython("startDistance", 50f, "2");
        endDistance2 = GetFloatFromPython("endDistance", 1f, "2");
        delayToApproach2 = GetFloatFromPython("delayToApproach", 5f, "2");
        numReps2 = GetFloatFromPython("numReps", 1f, "2");
        stimulusColour2 = GetColorFromPython("stimulusColour", stimulusColour2, "2");
        opaqueObject2 = GetBoolFromPython("opaqueObject", false, "2");
        stimulusType2 = GetIntFromPython("stimulusType", 0, "2"); // 0 = icosphere, 1 = unity cube
        drawOutline2 = GetBoolFromPython("drawOutline", false, "2");
        outlineWidth2 = GetFloatFromPython("outlineWidth", 5f, "2");
        outlineColor2 = GetColorFromPython("outlineColour", outlineColor2, "2");
        outlineType2 = GetIntFromPython("outlineType", outlineType2, "2");
        fixedAngularSize2 = GetBoolFromPython("fixedAngularSize", false, "2");
        fixXAxis2 = GetBoolFromPython("fixElevation", false, "2"); // otherwise fix the Y axis
        if (fixXAxis2) {
            minAngularAngle2 = -1 * GetFloatFromPython("maxAngularAngle", -30f, "2");
            maxAngularAngle2 = -1 * GetFloatFromPython("minAngularAngle", 30f, "2");
        } else {
            minAngularAngle2 = GetFloatFromPython("minAngularAngle", -30f, "2");
            maxAngularAngle2 = GetFloatFromPython("maxAngularAngle", 30f, "2");
        }
        delayToAppear2 = GetFloatFromPython("delayToAppear", 0f, "2");
        directPath2 = GetBoolFromPython("directPath", true, "2");
        hideAtEnd2 = GetBoolFromPython("hideAtEnd", false, "2");

        startScale3 = GetVector3FromPython("startScale", startScale3, "3");
        endScale3 = GetVector3FromPython("endScale", endScale3, "3");
        stimulusDuration3 = GetFloatFromPython("duration", 5f, "3");
        origin3 = GetVector3FromPython("origin", Vector3.zero, "3");
        rotationOffset3 = GetVector2FromPython("rotationOffset", Vector2.zero, "3");
        startPolarPosition3.x = -1 * GetFloatFromPython("startElevation", 0f, "3");
        startPolarPosition3.y = GetFloatFromPython("startAzimuth", 0f, "3");
        endPolarPosition3.x = -1 * GetFloatFromPython("endElevation", 0f, "3");
        endPolarPosition3.y = GetFloatFromPython("endAzimuth", 0f, "3");
        startDistance3 = GetFloatFromPython("startDistance", 50f, "3");
        endDistance3 = GetFloatFromPython("endDistance", 1f, "3");
        delayToApproach3 = GetFloatFromPython("delayToApproach", 5f, "3");
        numReps3 = GetFloatFromPython("numReps", 1f, "3");
        stimulusColour3 = GetColorFromPython("stimulusColour", stimulusColour3, "3");
        opaqueObject3 = GetBoolFromPython("opaqueObject", false, "3");
        stimulusType3 = GetIntFromPython("stimulusType", 0, "3"); // 0 = icosphere, 1 = unity cube
        drawOutline3 = GetBoolFromPython("drawOutline", false, "3");
        outlineWidth3 = GetFloatFromPython("outlineWidth", 5f, "3");
        outlineColor3 = GetColorFromPython("outlineColour", outlineColor3, "3");
        outlineType3 = GetIntFromPython("outlineType", outlineType3, "3");
        fixedAngularSize3 = GetBoolFromPython("fixedAngularSize", false, "3");
        fixXAxis3 = GetBoolFromPython("fixElevation", false, "3"); // otherwise fix the Y axis
        if (fixXAxis3) {
            minAngularAngle3 = -1 * GetFloatFromPython("maxAngularAngle", -30f, "3");
            maxAngularAngle3 = -1 * GetFloatFromPython("minAngularAngle", 30f, "3");
        } else {
            minAngularAngle3 = GetFloatFromPython("minAngularAngle", -30f, "3");
            maxAngularAngle3 = GetFloatFromPython("maxAngularAngle", 30f, "3");
        }
        delayToAppear3 = GetFloatFromPython("delayToAppear", 0f, "3");
        directPath3 = GetBoolFromPython("directPath", true, "3");
        hideAtEnd3 = GetBoolFromPython("hideAtEnd", false, "3");

        startScale4 = GetVector3FromPython("startScale", startScale4, "4");
        endScale4 = GetVector3FromPython("endScale", endScale4, "4");
        stimulusDuration4 = GetFloatFromPython("duration", 5f, "4");
        origin4 = GetVector3FromPython("origin", Vector3.zero, "4");
        rotationOffset4 = GetVector2FromPython("rotationOffset", Vector2.zero, "4");
        startPolarPosition4.x = -1 * GetFloatFromPython("startElevation", 0f, "4");
        startPolarPosition4.y = GetFloatFromPython("startAzimuth", 0f, "4");
        endPolarPosition4.x = -1 * GetFloatFromPython("endElevation", 0f, "4");
        endPolarPosition4.y = GetFloatFromPython("endAzimuth", 0f, "4");
        startDistance4 = GetFloatFromPython("startDistance", 50f, "4");
        endDistance4 = GetFloatFromPython("endDistance", 1f, "4");
        delayToApproach4 = GetFloatFromPython("delayToApproach", 5f, "4");
        numReps4 = GetFloatFromPython("numReps", 1f, "4");
        stimulusColour4 = GetColorFromPython("stimulusColour", stimulusColour4, "4");
        opaqueObject4 = GetBoolFromPython("opaqueObject", false, "4");
        stimulusType4 = GetIntFromPython("stimulusType", 0, "4"); // 0 = icosphere, 1 = unity cube
        drawOutline4 = GetBoolFromPython("drawOutline", false, "4");
        outlineWidth4 = GetFloatFromPython("outlineWidth", 5f, "4");
        outlineColor4 = GetColorFromPython("outlineColour", outlineColor4, "4");
        outlineType4 = GetIntFromPython("outlineType", outlineType4, "4");
        fixedAngularSize4 = GetBoolFromPython("fixedAngularSize", false, "4");
        fixXAxis4 = GetBoolFromPython("fixElevation", false, "4"); // otherwise fix the Y axis
        if (fixXAxis4) {
            minAngularAngle4 = -1 * GetFloatFromPython("maxAngularAngle", -30f, "4");
            maxAngularAngle4 = -1 * GetFloatFromPython("minAngularAngle", 30f, "4");
        } else {
            minAngularAngle4 = GetFloatFromPython("minAngularAngle", -30f, "4");
            maxAngularAngle4 = GetFloatFromPython("maxAngularAngle", 30f, "4");
        }
        delayToAppear4 = GetFloatFromPython("delayToAppear", 0f, "4");
        directPath4 = GetBoolFromPython("directPath", true, "4");
        hideAtEnd4 = GetBoolFromPython("hideAtEnd", false, "4");
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
        stimGenerator1.opaqueObject = opaqueObject1;
        stimGenerator1.startScale = startScale1;
        stimGenerator1.endScale = endScale1;
        stimGenerator1.startDistance = startDistance1;
        stimGenerator1.endDistance = endDistance1;
        stimGenerator1.delayToApproach = delayToApproach1;
        stimGenerator1.origin = origin1;
        stimGenerator1.rotationOffset = rotationOffset1;
        stimGenerator1.startPolarPosition = startPolarPosition1 + rotationOffset1;
        stimGenerator1.endPolarPosition = endPolarPosition1 + rotationOffset1;
        stimGenerator1.numReps = numReps1;
        stimGenerator1.duration = stimulusDuration1; 
        stimGenerator1.stimulusType = stimulusType1;
        stimGenerator1.drawOutline = drawOutline1;
        stimGenerator1.outlineWidth = outlineWidth1;
        stimGenerator1.outlineType = outlineType1;
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
        stimGenerator1.delayToAppear = delayToAppear1;
        stimGenerator1.directPath = directPath1;
        stimGenerator1.hideAtEnd = hideAtEnd1;

        stimGenerator1.Reset();

        // stimulus 2
        if (stimGenerators.Length > 1) {
            SphericalStimulusGenerator stimGenerator2 = stimGenerators[1];
            stimGenerator2.flickerDuration = flickerDuration;
            stimGenerator2.stimulusColour = stimulusColour2;
            stimGenerator2.opaqueObject = opaqueObject2;
            stimGenerator2.startScale = startScale2;
            stimGenerator2.endScale = endScale2;
            stimGenerator2.startDistance = startDistance2;
            stimGenerator2.endDistance = endDistance2;
            stimGenerator2.delayToApproach = delayToApproach2;
            stimGenerator2.origin = origin2;
            stimGenerator2.rotationOffset = rotationOffset2;
            stimGenerator2.startPolarPosition = startPolarPosition2 + rotationOffset2;
            stimGenerator2.endPolarPosition = endPolarPosition2 + rotationOffset2;
            stimGenerator2.numReps = numReps2;
            stimGenerator2.duration = stimulusDuration2; 
            stimGenerator2.stimulusType = stimulusType2;
            stimGenerator2.drawOutline = drawOutline2;
            stimGenerator2.outlineWidth = outlineWidth2;
            stimGenerator2.outlineColor = outlineColor2;
            stimGenerator2.outlineType = outlineType2;
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
            stimGenerator2.delayToAppear = delayToAppear2;
            stimGenerator2.directPath = directPath2;
            stimGenerator2.hideAtEnd = hideAtEnd2;

            stimGenerator2.Reset();

            SphericalStimulusGenerator stimGenerator3 = stimGenerators[2];
            stimGenerator3.flickerDuration = flickerDuration;
            stimGenerator3.stimulusColour = stimulusColour3;
            stimGenerator3.opaqueObject = opaqueObject3;
            stimGenerator3.startScale = startScale3;
            stimGenerator3.endScale = endScale3;
            stimGenerator3.startDistance = startDistance3;
            stimGenerator3.endDistance = endDistance3;
            stimGenerator3.delayToApproach = delayToApproach3;
            stimGenerator3.origin = origin3;
            stimGenerator3.rotationOffset = rotationOffset3;
            stimGenerator3.startPolarPosition = startPolarPosition3 + rotationOffset3;
            stimGenerator3.endPolarPosition = endPolarPosition3 + rotationOffset3;
            stimGenerator3.numReps = numReps3;
            stimGenerator3.duration = stimulusDuration3; 
            stimGenerator3.stimulusType = stimulusType3;
            stimGenerator3.drawOutline = drawOutline3;
            stimGenerator3.outlineWidth = outlineWidth3;
            stimGenerator3.outlineColor = outlineColor3;
            stimGenerator3.outlineType = outlineType3;
            stimGenerator3.gratingNum = gratingNum;
            stimGenerator3.gratingIsSquare = gratingIsSquare ? 1 : 0;
            stimGenerator3.gratingMaxIntensity = gratingMaxIntensity;
            stimGenerator3.gratingMinIntensity = gratingMinIntensity;
            stimGenerator3.fixedAngularSize = fixedAngularSize3;
            stimGenerator3.fixXAxis = fixXAxis3; // otherwise fix the Y axis
            stimGenerator3.minAngularAngle = minAngularAngle3;
            stimGenerator3.maxAngularAngle = maxAngularAngle3;
            stimGenerator3.manualControl = manualControl;
            stimGenerator3.mouseMoveSpeed = mouseMoveSpeed;
            stimGenerator3.delayToAppear = delayToAppear3;
            stimGenerator3.directPath = directPath3;
            stimGenerator3.hideAtEnd = hideAtEnd3;

            stimGenerator3.Reset();

            SphericalStimulusGenerator stimGenerator4 = stimGenerators[3];
            stimGenerator4.flickerDuration = flickerDuration;
            stimGenerator4.stimulusColour = stimulusColour4;
            stimGenerator4.opaqueObject = opaqueObject4;
            stimGenerator4.startScale = startScale4;
            stimGenerator4.endScale = endScale4;
            stimGenerator4.startDistance = startDistance4;
            stimGenerator4.endDistance = endDistance4;
            stimGenerator4.delayToApproach = delayToApproach4;
            stimGenerator4.origin = origin4;
            stimGenerator4.rotationOffset = rotationOffset4;
            stimGenerator4.startPolarPosition = startPolarPosition4 + rotationOffset4;
            stimGenerator4.endPolarPosition = endPolarPosition4 + rotationOffset4;
            stimGenerator4.numReps = numReps4;
            stimGenerator4.duration = stimulusDuration4; 
            stimGenerator4.stimulusType = stimulusType4;
            stimGenerator4.drawOutline = drawOutline4;
            stimGenerator4.outlineWidth = outlineWidth4;
            stimGenerator4.outlineColor = outlineColor4;
            stimGenerator4.outlineType = outlineType4;
            stimGenerator4.gratingNum = gratingNum;
            stimGenerator4.gratingIsSquare = gratingIsSquare ? 1 : 0;
            stimGenerator4.gratingMaxIntensity = gratingMaxIntensity;
            stimGenerator4.gratingMinIntensity = gratingMinIntensity;
            stimGenerator4.fixedAngularSize = fixedAngularSize4;
            stimGenerator4.fixXAxis = fixXAxis4; // otherwise fix the Y axis
            stimGenerator4.minAngularAngle = minAngularAngle4;
            stimGenerator4.maxAngularAngle = maxAngularAngle4;
            stimGenerator4.manualControl = manualControl;
            stimGenerator4.mouseMoveSpeed = mouseMoveSpeed;
            stimGenerator4.delayToAppear = delayToAppear4;
            stimGenerator4.directPath = directPath4;
            stimGenerator4.hideAtEnd = hideAtEnd4;

            stimGenerator4.Reset();
        }
    }
}