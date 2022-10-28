using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;

public class LoomManager : GenericStimulusManager
{
    // A class for controlling looming stimuli

    [Header("Looming Background stimulus parameters")]
    public Material skyboxMaterial;
    public float horizonHeight = 0f;
    public Color aboveHorizonColour = Color.grey;
    public Color belowHorizonColour = Color.white;

    public float[] horizonHeights = new float[4] { -9999f, -9999f, -9999f, -9999f };
    public Color[] aboveHorizonColours = new Color[4] { Color.grey, Color.grey, Color.grey, Color.grey };
    public Color[] belowHorizonColours = new Color[4] { Color.white, Color.white, Color.white, Color.white };

    [Header("Looming transform parameters")]
    public Vector2 startPolarPosition = Vector2.zero;
    public Vector2 endPolarPosition = Vector2.zero;
    public Vector3 startScale = Vector3.one;
    public Vector3 endScale = Vector3.one;
    public Vector3 origin = Vector3.zero;
    public Vector2 rotationOffset = Vector2.zero;
    public float startDistance = 10f;
    public float endDistance = 10f;
    public float duration = 1f; // units (cm) per second
    public float numReps = 0.5f;
    public float delayToApproach = 5f;
    public bool fixedAngularSize = false;
    public bool fixXAxis = true; // otherwise fix the Y axis
    public float minAngularAngle = -30f;
    public float maxAngularAngle = 30f;

    [Header("Looming object appearance parameters")]
    public int stimulusType = 0; // 0 = icosphere, 1 = unity cube
    public Color stimulusColour = Color.white;

    public bool opaqueObject = false;

    // settings if stimulus type is 2 (grating stimulus)
    public float gratingNum = 100f;
    public int gratingIsSquare = 0;
    public float gratingMaxIntensity = 0.1f;
    public float gratingMinIntensity = 0f;

    public bool drawOutline = false;
    public float outlineWidth = 5f;
    public int outlineType = 0; // 0 = world space fixed size, 1 = pixel space fixed size
    public Color outlineColor = Color.black;
    public float delayToAppear = 0f;
    public bool directPath = true;
    public bool hideAtEnd = true;

    [Header("Looming Components")]
    public SphericalStimulusGenerator stimGenerator;

    protected override void GetPropertiesFromPython() {
        print("Getting properties from python...");
        // get generic stimulus parameters from python
        base.GetPropertiesFromPython();

        // now get those specific to this stimuli
        // object
        origin = GetVector3FromPython("origin", Vector3.zero);
        rotationOffset = GetVector2FromPython("rotationOffset", Vector2.zero);
        startPolarPosition.x = -1 * GetFloatFromPython("startElevation", 0f);
        startPolarPosition.y = GetFloatFromPython("startAzimuth", 0f);
        endPolarPosition.x = -1 * GetFloatFromPython("endElevation", 0f);
        endPolarPosition.y = GetFloatFromPython("endAzimuth", 0f);
        startDistance = GetFloatFromPython("startDistance", 50f);
        endDistance = GetFloatFromPython("endDistance", 1f);
        stimulusType = (int)GetFloatFromPython("stimulusType", 0); // 0 = icosphere, 1 = unity cube
        numReps = GetFloatFromPython("numReps", 0.5f);
        drawOutline = GetBoolFromPython("drawOutline", false);
        outlineWidth = GetFloatFromPython("outlineWidth", 5f);
        outlineColor = GetColorFromPython("outlineColour", Color.black);
        outlineType = GetIntFromPython("outlineType", outlineType);
        stimulusColour = GetColorFromPython("stimulusColour", Color.grey);
        opaqueObject = GetBoolFromPython("opaqueObject", false);
        gratingNum = GetFloatFromPython("gratingNum", 100f);
        gratingIsSquare = (int)GetFloatFromPython("gratingIsSquare", 0f);
        gratingMaxIntensity = GetFloatFromPython("gratingMaxIntensity", 0.1f);
        gratingMinIntensity = GetFloatFromPython("gratingMinIntensity", 0f);
        startScale = GetVector3FromPython("startScale", Vector3.one);
        endScale = GetVector3FromPython("endScale", Vector3.one);
        duration = GetFloatFromPython("duration", 1f);
        fixedAngularSize = GetBoolFromPython("fixedAngularSize", false);
        fixXAxis = GetBoolFromPython("fixElevation", true);
        if (fixXAxis) {
            minAngularAngle = -1 * GetFloatFromPython("maxAngularAngle", -30f);
            maxAngularAngle = -1 * GetFloatFromPython("minAngularAngle", 30f);
        } else {
            minAngularAngle = GetFloatFromPython("minAngularAngle", -30f);
            maxAngularAngle = GetFloatFromPython("maxAngularAngle", 30f);
        }
        delayToApproach = GetFloatFromPython("delayToApproach", 5f);
        delayToAppear = GetFloatFromPython("delayToAppear", 0f);
        directPath = GetBoolFromPython("directPath", true);
        hideAtEnd = GetBoolFromPython("hideAtEnd", false);

        // background 
        horizonHeight = GetFloatFromPython("horizonHeight", 0f);
        aboveHorizonColour = GetColorFromPython("aboveHorizonColour", Color.white);
        belowHorizonColour = GetColorFromPython("belowHorizonColour", Color.grey);
        // specific overrides for backgrounds on different cameras
        string[] sides = new string[] { "Front", "Right", "Back", "Left" };
        for (int i = 0; i < sides.Length; i++) {
            string side = sides[i];
            horizonHeights[i] = GetFloatFromPython("horizonHeight", -9999f, side);
            aboveHorizonColours[i] = GetColorFromPython("aboveHorizonColour", Color.white, side);
            belowHorizonColours[i] = GetColorFromPython("belowHorizonColour", Color.grey, side);
        }
    }

    public override void SetupStimuli() {
        // overall skybox
        Material mat = new Material(RenderSettings.skybox);
        mat.SetFloat("_horizonHeight", horizonHeight);
        mat.SetColor("_aboveHorizonColour", aboveHorizonColour);
        mat.SetColor("_belowHorizonColour", belowHorizonColour);
        RenderSettings.skybox = mat;

        // if specified, override the skybox for individual cameras
        // check if skybox component exists
        SetSkybox(camMon.frontCam.gameObject, horizonHeights[0], aboveHorizonColours[0], belowHorizonColours[0]);
        SetSkybox(camMon.rightCam.gameObject, horizonHeights[1], aboveHorizonColours[1], belowHorizonColours[1]);
        SetSkybox(camMon.backCam.gameObject, horizonHeights[2], aboveHorizonColours[2], belowHorizonColours[2]);
        SetSkybox(camMon.leftCam.gameObject, horizonHeights[3], aboveHorizonColours[3], belowHorizonColours[3]);

        // sphere
        stimGenerator.stimulusColour = stimulusColour;
        stimGenerator.opaqueObject = opaqueObject;
        stimGenerator.startScale = startScale;
        stimGenerator.endScale = endScale;
        stimGenerator.startPolarPosition = startPolarPosition + rotationOffset;
        stimGenerator.endPolarPosition = endPolarPosition + rotationOffset;
        stimGenerator.startDistance = startDistance;
        stimGenerator.endDistance = endDistance;
        stimGenerator.delayToApproach = delayToApproach;
        stimGenerator.origin = origin;
        stimGenerator.rotationOffset = rotationOffset;
        stimGenerator.flickerDuration = base.flickerDuration;
        stimGenerator.numReps = numReps;
        stimGenerator.stimulusType = stimulusType;
        stimGenerator.drawOutline = drawOutline;
        stimGenerator.outlineWidth = outlineWidth;
        stimGenerator.outlineColor = outlineColor;
        stimGenerator.outlineType = outlineType;
        stimGenerator.gratingNum = gratingNum;
        stimGenerator.gratingIsSquare = gratingIsSquare;
        stimGenerator.gratingMaxIntensity = gratingMaxIntensity;
        stimGenerator.gratingMinIntensity = gratingMinIntensity;
        stimGenerator.fixedAngularSize = fixedAngularSize;
        stimGenerator.fixXAxis = fixXAxis; // otherwise fix the Y axis
        stimGenerator.minAngularAngle = minAngularAngle;
        stimGenerator.maxAngularAngle = maxAngularAngle;
        stimGenerator.delayToAppear = delayToAppear;
        stimGenerator.directPath = directPath;
        stimGenerator.hideAtEnd = hideAtEnd;

        stimGenerator.duration = duration; 

        stimGenerator.manualControl = manualControl;
        stimGenerator.mouseMoveSpeed = mouseMoveSpeed;
        stimGenerator.Reset();
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
