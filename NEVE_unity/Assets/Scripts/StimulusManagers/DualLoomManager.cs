using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;

public class DualLoomManager : GenericStimulusManager
{
    public Material skyboxMaterial;

    [Header("Specific stimulus parameters")]

    public Vector3 startScale1 = Vector3.one;
    public Vector3 endScale1 = Vector3.one;
    public Vector3 startScale2 = Vector3.one;
    public Vector3 endScale2 = Vector3.one;
    public Vector2 startPolarPosition1 = new Vector2(0f, 0f);
    public Vector2 startPolarPosition2 = new Vector2(0f, 0f);
    public Vector2 endPolarPosition1 = new Vector2(0f, 0f);
    public Vector2 endPolarPosition2 = new Vector2(0f, 0f);
    public Vector3 origin1 = new Vector3(0f, 0f, 0f);
    public Vector3 origin2 = new Vector3(0f, 0f, 0f);
    public Vector2 rotationOffset1 = Vector2.zero;
    public Vector2 rotationOffset2 = Vector2.zero;
    public float startDistance1 = 10f;
    public float startDistance2 = 10f;
    public float endDistance1 = 10f;
    public float endDistance2 = 10f;
    public float delayToApproach1 = 5f;
    public float delayToApproach2 = 5f;
    public float numReps1 = 2;
    public float numReps2 = 2;
    public Color stimulusColour1 = Color.white;
    public Color stimulusColour2 = Color.white;
    public bool opaqueObject1 = false;
    public bool opaqueObject2 = false;
    public float stimulusDuration1 = 5f;
    public float stimulusDuration2 = 5f;
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

    public float delayToAppear1 = 0f;
    public float delayToAppear2 = 0f;
    public bool directPath1 = true;
    public bool directPath2 = true;
    public bool hideAtEnd1 = false;
    public bool hideAtEnd2 = false;


    protected override void GetPropertiesFromPython() {
        // get generic stimulus parameters from python
        base.GetPropertiesFromPython();

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
    }

    public override void SetupStimuli() {
        // skybox
        // overall skybox
        Material mat = new Material(RenderSettings.skybox);
        mat.SetFloat("_horizonHeight", GetFloatFromPython("horizonHeight", 0f));
        mat.SetColor("_aboveHorizonColour", GetColorFromPython("aboveHorizonColour", Color.white));
        mat.SetColor("_belowHorizonColour", GetColorFromPython("belowHorizonColour", Color.grey));
        RenderSettings.skybox = mat;

        // specific overrides for backgrounds on different cameras
        float[] horizonHeights = new float[4] { -9999f, -9999f, -9999f, -9999f };
        Color[] aboveHorizonColours = new Color[4] { Color.grey, Color.grey, Color.grey, Color.grey };
        Color[] belowHorizonColours = new Color[4] { Color.white, Color.white, Color.white, Color.white };
        string[] sides = new string[] { "Front", "Right", "Back", "Left" };
        for (int i = 0; i < sides.Length; i++) {
            string side = sides[i];
            horizonHeights[i] = GetFloatFromPython("horizonHeight", -9999f, side);
            aboveHorizonColours[i] = GetColorFromPython("aboveHorizonColour", Color.white, side);
            belowHorizonColours[i] = GetColorFromPython("belowHorizonColour", Color.grey, side);
        }
        // if specified, override the skybox for individual cameras
        // check if skybox component exists
        SetSkybox(camMon.frontCam.gameObject, horizonHeights[0], aboveHorizonColours[0], belowHorizonColours[0]);
        SetSkybox(camMon.rightCam.gameObject, horizonHeights[1], aboveHorizonColours[1], belowHorizonColours[1]);
        SetSkybox(camMon.backCam.gameObject, horizonHeights[2], aboveHorizonColours[2], belowHorizonColours[2]);
        SetSkybox(camMon.leftCam.gameObject, horizonHeights[3], aboveHorizonColours[3], belowHorizonColours[3]);

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
        }
    }

    void SetSkybox(GameObject camGameObject, float horizonHeight, Color aboveHorizonColour, Color belowHorizonColour) {
        if (horizonHeight != -9999f) {
            if (camGameObject.GetComponent<Skybox>() == null) {
                camGameObject.AddComponent<Skybox>();
            }
            Skybox skybox = camGameObject.GetComponent<Skybox>();
            skybox.material = new Material(skyboxMaterial);
            skybox.material.SetFloat("_horizonHeight", horizonHeight);
            skybox.material.SetColor("_aboveHorizonColour", aboveHorizonColour);
            skybox.material.SetColor("_belowHorizonColour", belowHorizonColour);
        }
    }
}