using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;

public class LoomManager : GenericStimulusManager
{
    public SphericalStimulusGenerator stimGenerator;
    public Material skyboxMaterial;

    public override void SetupStimuli() {
        // overall skybox
        Material mat = new Material(RenderSettings.skybox);
        mat.SetFloat("_horizonHeight", GetFloatFromPython("horizonHeight", 0f));
        mat.SetColor("_aboveHorizonColour", GetColorFromPython("aboveHorizonColour", Color.black));
        mat.SetColor("_belowHorizonColour", GetColorFromPython("belowHorizonColour", Color.black));
        RenderSettings.skybox = mat;

        print("aboveHorizonColour: " + mat.GetColor("_aboveHorizonColour"));
        print("belowHorizonColour: " + mat.GetColor("_belowHorizonColour"));

        // specific overrides for backgrounds on different cameras
        float[] horizonHeights = new float[4] { -9999f, -9999f, -9999f, -9999f };
        Color[] aboveHorizonColours = new Color[4] { Color.black, Color.black, Color.black, Color.black };
        Color[] belowHorizonColours = new Color[4] { Color.black, Color.black, Color.black, Color.black };
        string[] sides = new string[] { "Front", "Right", "Back", "Left" };
        for (int i = 0; i < sides.Length; i++) {
            string side = sides[i];
            horizonHeights[i] = GetFloatFromPython("horizonHeight", -9999f, side);
            aboveHorizonColours[i] = GetColorFromPython("aboveHorizonColour", Color.black, side);
            belowHorizonColours[i] = GetColorFromPython("belowHorizonColour", Color.black, side);
        }
        // if specified, override the skybox for individual cameras
        // check if skybox component exists
        SetSkybox(camMon.frontCam.gameObject, horizonHeights[0], aboveHorizonColours[0], belowHorizonColours[0]);
        SetSkybox(camMon.rightCam.gameObject, horizonHeights[1], aboveHorizonColours[1], belowHorizonColours[1]);
        SetSkybox(camMon.backCam.gameObject, horizonHeights[2], aboveHorizonColours[2], belowHorizonColours[2]);
        SetSkybox(camMon.leftCam.gameObject, horizonHeights[3], aboveHorizonColours[3], belowHorizonColours[3]);

        // sphere
        stimGenerator.stimulusColour = GetColorFromPython("stimulusColour", Color.black);
        stimGenerator.opaqueObject = GetBoolFromPython("opaqueObject", false);
        stimGenerator.startScale = GetVector3FromPython("startScale", Vector3.one);
        stimGenerator.endScale = GetVector3FromPython("endScale", Vector3.one);
        stimGenerator.rotation = GetVector3FromPython("rotation", Vector3.zero);

        Vector2 rotationOffset = GetVector2FromPython("rotationOffset", Vector2.zero);
        Vector2 startPolarPosition = Vector2.zero;
        startPolarPosition.x = -1 * GetFloatFromPython("startElevation", 10f);
        startPolarPosition.y = GetFloatFromPython("startAzimuth", 0f);
        Vector2 endPolarPosition = Vector2.zero;
        endPolarPosition.x = -1 * GetFloatFromPython("endElevation", 0f);
        endPolarPosition.y = GetFloatFromPython("endAzimuth", 0f);
        stimGenerator.startPolarPosition = startPolarPosition + rotationOffset;
        stimGenerator.endPolarPosition = endPolarPosition + rotationOffset;
        stimGenerator.startDistance = GetFloatFromPython("startDistance", 50f);
        stimGenerator.endDistance = GetFloatFromPython("endDistance", 1f);
        stimGenerator.delayToApproach = GetFloatFromPython("delayToApproach", 5f);
        stimGenerator.origin = GetVector3FromPython("origin", Vector3.zero);
        stimGenerator.rotationOffset = rotationOffset;
        stimGenerator.flickerDuration = base.flickerDuration;
        stimGenerator.numReps = GetFloatFromPython("numReps", 0.5f);
        stimGenerator.stimulusType = (int)GetFloatFromPython("stimulusType", 3);
        stimGenerator.drawOutline = GetBoolFromPython("drawOutline", false);
        stimGenerator.outlineWidth = GetFloatFromPython("outlineWidth", 5f);
        stimGenerator.outlineColor = GetColorFromPython("outlineColour", Color.black);
        stimGenerator.outlineType = GetIntFromPython("outlineType", 0);
        stimGenerator.gratingNum = GetFloatFromPython("gratingNum", 100f);
        stimGenerator.gratingIsSquare = (int)GetFloatFromPython("gratingIsSquare", 0f);
        stimGenerator.gratingMaxIntensity = GetFloatFromPython("gratingMaxIntensity", 1f);
        stimGenerator.gratingMinIntensity = GetFloatFromPython("gratingMinIntensity", 0f);
        stimGenerator.fixedAngularSize = GetBoolFromPython("fixedAngularSize", false);
        bool fixXAxis = GetBoolFromPython("fixElevation", false);
        stimGenerator.fixXAxis = fixXAxis; 
        if (fixXAxis) {
            stimGenerator.minAngularAngle = -1 * GetFloatFromPython("maxAngularAngle", -30f);
            stimGenerator.maxAngularAngle = -1 * GetFloatFromPython("minAngularAngle", 30f);
        } else {
            stimGenerator.minAngularAngle = GetFloatFromPython("minAngularAngle", -30f);
            stimGenerator.maxAngularAngle = GetFloatFromPython("maxAngularAngle", 30f);
        }
        stimGenerator.delayToAppear = GetFloatFromPython("delayToAppear", 0f);
        stimGenerator.directPath = GetBoolFromPython("directPath", true);
        stimGenerator.hideAtEnd = GetBoolFromPython("hideAtEnd", false);

        stimGenerator.duration = GetFloatFromPython("duration", 2f); 

        stimGenerator.manualControl = manualControl;
        stimGenerator.mouseMoveSpeed = mouseMoveSpeed;
        stimGenerator.Reset();
        stimGenerator.autoStart = GetBoolFromPython("autoStart", false);
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
